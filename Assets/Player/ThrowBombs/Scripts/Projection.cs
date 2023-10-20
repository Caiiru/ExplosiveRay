using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Projection : MonoBehaviour
{
    private int id;
    #region Singleton
    public static Projection instance;

    public static Projection getInstance()
    {
        if (instance == null)
        {
            instance = new Projection();
        }
        return instance;
    }
    private void Awake()
    {

    }

    #endregion
    private Scene _simulationScene;
    private PhysicsScene _physicsScene;
    [SerializeField] private Transform _obstacleParents;

    [SerializeField] LineRenderer line;

    [Range(1, 300)]
    [SerializeField] int _maxPhysicsFrameIterations;


    void Start()
    {
        CreatePhysicsScene();
    }

    void CreatePhysicsScene()
    {
        _simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        _physicsScene = _simulationScene.GetPhysicsScene();

        foreach (Transform obj in _obstacleParents)
        {
            //Tratatamento para cada tipo de 'ghost object'
            var ghostObj = Instantiate(obj.gameObject, obj.transform.position, obj.rotation);
            if (ghostObj.CompareTag("Player"))
            {
                ghostObj.GetComponent<AimCannon>().enabled = false;
                ghostObj.GetComponent<Projection>().enabled = false;
            }
            if (ghostObj.CompareTag("Enemy"))
            {
                ghostObj.tag = "Ghost Enemy";
                ghostObj.GetComponent<Rigidbody>().freezeRotation = true;
            }
            else
                ghostObj.tag = "Ghost";

            //Transformar ele em ghost: mudar o nome e a tag (mudar a tag para não dar conflito com as outras classes que puxam alguma informação por tag!

            ghostObj.name = "Ghost " + ghostObj.name;
            if (ghostObj.transform.childCount != 0)
            {
                foreach (Transform childs in ghostObj.transform)
                {
                    if (childs.transform.childCount != 0)
                    {
                        foreach (Transform secondChilds in childs.transform)
                        {
                            if (secondChilds.transform.GetComponent<MeshRenderer>() != null)
                                secondChilds.GetComponent<MeshRenderer>().enabled = false;
                        }
                    }
                    if (childs.transform.GetComponent<MeshRenderer>() != null)
                        childs.GetComponent<MeshRenderer>().enabled = false;
                }
            }
            else
            {

                if (ghostObj.transform.GetComponent<MeshRenderer>() != null)
                    ghostObj.GetComponent<MeshRenderer>().enabled = false;

            }



            SceneManager.MoveGameObjectToScene(ghostObj, _simulationScene);
        }
    }


    public void SimulateTrajectory(GameObject bomb, Vector3 pos, Vector3 velocity, int initialTimer)
    {
        var ghostObj = Instantiate(bomb, pos, Quaternion.identity);
        ghostObj.layer = LayerMask.NameToLayer("BombLayer");
        ghostObj.tag = "Bomb";
        for (int i = 0; i < ghostObj.transform.childCount; i++)
        {
            var ghostChild = ghostObj.transform.GetChild(i);
            if (ghostChild.GetComponent<MeshRenderer>() != null)
                ghostChild.GetComponent<MeshRenderer>().enabled = false;
        }
        SceneManager.MoveGameObjectToScene(ghostObj.gameObject, _simulationScene);

        ghostObj.GetComponent<Bomb>().Init(velocity, true,initialTimer);
        ghostObj.GetComponent<Bomb>().isGhost = true;

        line.positionCount = _maxPhysicsFrameIterations;
        line.SetPosition(0, pos);
        var denseGravityBodies = FindObjectsOfType<BlackholeBomb>();

        for (int i = 1; i < _maxPhysicsFrameIterations; i++)
        {
            /* var ghostPosition = ghostObj.GetComponent<Rigidbody>().position;
            if (denseGravityBodies != null)
            {
                foreach (var obj in denseGravityBodies)
                {
                    if (obj.isActive)
                    {
                        var acceleration = Vector3.zero;
                        acceleration += obj.CalculateForceAboveOther(ghostPosition, _maxPhysicsFrameIterations);
                        ghostObj.GetComponent<Rigidbody>().AddForce(acceleration,ForceMode.Acceleration);
                    }
                }
            } */


            _physicsScene.Simulate(Time.fixedDeltaTime);

            line.SetPosition(i, ghostObj.transform.position);
            //line.SetPosition(i, NBombSimulation.Instance.CalculateGhostTrajectory(i,_maxPhysicsFrameIterations,ghostObj.GetComponent<Bomb>()));
        }

        Destroy(ghostObj.gameObject);
        //ghostObj.transform.position = line.GetPosition(line.positionCount-1);
    }

}
