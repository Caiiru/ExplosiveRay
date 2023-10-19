using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{ 

    #region Singleton
    public static PlayerInventory instance;
    public static PlayerInventory getInstance(){
        if(instance == null){
            instance = FindObjectOfType<PlayerInventory>();
            if(instance == null){
                GameObject gameObject = new GameObject ("PlayerInventory");
                instance = gameObject.AddComponent<PlayerInventory>();
            }
        }
        return instance;
    }

    #endregion
    public GameObject[] bombs;
    public Bomb currentBomb;
    [SerializeField] int currentBombIndex = 0;
    private void Awake() {
        if(instance == null) {
            instance = this as PlayerInventory;
        }else{
            if(instance != this){
                Destroy(gameObject);
            }
        }
    }
    private void Start()
    { 
        currentBomb = bombs[currentBombIndex].GetComponent<Bomb>();
        UiManager.getInstance().changeBombImage(currentBomb.bombSprite);

    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.RightArrow)){
            currentBombIndex++;
            if(currentBombIndex>=bombs.Length){
                currentBombIndex=0;
            }
            //currentBombIndex= currentBombIndex<bombs.Length?0:currentBombIndex;  
            ChangeCurrentBomb(); 
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow)){
           currentBombIndex--;
           if(currentBombIndex<0){
                currentBombIndex=bombs.Length-1; 
            }
           ChangeCurrentBomb();
        }
    }

    public Bomb GetCurrentBomb(){
        return currentBomb;
    }

    public void ChangeCurrentBomb(){
        currentBomb = bombs[currentBombIndex].GetComponent<Bomb>();
        UiManager.getInstance().changeBombImage(currentBomb.bombSprite);
    }
     

}
