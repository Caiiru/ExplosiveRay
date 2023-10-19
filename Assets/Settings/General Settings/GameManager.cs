using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStatus
{
    PreStart,
    Running,
    Finish
}
public class GameManager : MonoBehaviour

{
    #region Singleton
    private static GameManager instance;
    public static GameManager getInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<GameManager>();
            if (instance == null)
            {
                GameObject gameObject = new GameObject("Game Manager");
                instance = gameObject.AddComponent<GameManager>();
            }
        }
        return instance;
    }


    #endregion

    public GameStatus gameStatus;
    public GameObject[] currentEnemys;
    public GameObject[] ghostEnemys;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this as GameManager;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        ChangeGameStatus(GameStatus.PreStart);
    }
    void Start()
    {

        ChangeGameStatus(GameStatus.Running);
        UpdateEnemyList();
    }
    void Update()
    {
        UpdateGhosts();
    }


    public void UpdateEnemyList()
    {

        var list = GameObject.FindGameObjectsWithTag("Enemy");
        var ghostList = GameObject.FindGameObjectsWithTag("Ghost Enemy");
        currentEnemys = new GameObject[list.Length];
        ghostEnemys = new GameObject[currentEnemys.Length];
        int i = 0;
        if(list.Length == 0){
            Debug.Log("Killing Game");
            foreach(GameObject ghost in ghostList){
                Destroy(ghost);
            }
        }
        foreach (GameObject obj in list)
        {
                if (obj.GetComponent<Unit>().isAlive() && !obj.CompareTag("Ghost"))
                {
                    currentEnemys[i] = obj;
                }
                if (ghostList[i].GetComponent<Unit>().isAlive())
                {
                    ghostEnemys[i] = ghostList[i];
                }
                ghostEnemys[i].GetComponent<Rigidbody>().mass = currentEnemys[i].GetComponent<Rigidbody>().mass;
                ghostEnemys[i].GetComponent<Rigidbody>().drag = currentEnemys[i].GetComponent<Rigidbody>().drag;
                ghostEnemys[i].GetComponent<Rigidbody>().angularDrag = currentEnemys[i].GetComponent<Rigidbody>().angularDrag;
            


            i++;
        }
        int numberOfEmptys = 0;
        for (int j = 0; i < currentEnemys.Length; j++)
        {
            if (currentEnemys[j] == null)
            {
                numberOfEmptys += 1;
            }
        }
        if (numberOfEmptys == currentEnemys.Length)
        {
            ChangeGameStatus(GameStatus.Finish);
        }
    }
    public void ChangeGameStatus(GameStatus status)
    {
        if (status == gameStatus) return;
        Debug.Log("Changing from " + gameStatus + " to " + status);
        switch (status)
        {
            case GameStatus.PreStart:
                gameStatus = status;
                break;
            case GameStatus.Running:
                gameStatus = status;
                break;
            case GameStatus.Finish:
                gameStatus = status;
                print("Finish Game");
                break;
        }

    }



    public void UpdateGhosts()
    {
        for (int i = 0; i < currentEnemys.Length; i++)
        {
            if (ghostEnemys[i] != null && currentEnemys[i] != null)
                ghostEnemys[i].transform.position = currentEnemys[i].transform.position;

        }
    }
}
