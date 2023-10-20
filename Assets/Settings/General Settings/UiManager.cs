using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    #region Singleton
    private static UiManager instance;
    public static UiManager getInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<UiManager>();
            if (instance == null)
            {
                GameObject gameObject = new GameObject("UiManager");
                instance = gameObject.AddComponent<UiManager>();
            }
        }
        return instance;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this as UiManager;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    #endregion
    public RawImage currentBombImage;
    private int bombIndex;

    public TextMeshProUGUI currentTimerText;
    void Start()
    {
        transform.GetChild(0).transform.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void changeBombImage(Sprite sprite)
    {
        currentBombImage.texture = sprite.texture;
    }

    public void changeCurrentTimer(int newTime)
    {
        currentTimerText.text = newTime.ToString();
    }
}
