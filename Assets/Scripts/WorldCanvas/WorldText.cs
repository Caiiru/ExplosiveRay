using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
public enum textFontSize
{
    Small,
    Medium,
    Big
}
public class WorldText : MonoBehaviour
{
    public TextMeshProUGUI[] worldTexts;
    [Space]
    [SerializeField] bool createText;

    [SerializeField] UnityEngine.Vector3 position;

    [SerializeField] bool deleteText;
    [SerializeField] GameObject currentCanvas;
    #region Singleton
    public static WorldText instance;
    public static WorldText getInstance()
    {
        if (instance == null)
        {
            instance = new WorldText();
        }

        return instance;

    }
    private void Awake() {
        instance = this;
    }

    #endregion

    public int createWorldText(UnityEngine.Vector3 pos, String initialText, textFontSize fontSize)
    {
        var textGameObject = new GameObject("TextCreated");
        textGameObject.AddComponent<TextMeshProUGUI>();
        var textCreated = textGameObject.GetComponent<TextMeshProUGUI>();
        switch (fontSize)
        {
            case textFontSize.Small:
                textCreated.fontSize = 2;
                break;
            case textFontSize.Medium:
                textCreated.fontSize = 6;
                break;
            case textFontSize.Big:
                textCreated.fontSize = 8;
                break;
        }
        Debug.Log("Pré Create");
        textCreated.transform.SetParent(currentCanvas.transform);
        textCreated.text = initialText;
        textCreated.alignment = TextAlignmentOptions.Center;
        textCreated.alignment = TextAlignmentOptions.Midline;
        textCreated.transform.position = pos;

        Debug.Log("Criando texto");
        for (int i = 0; i < worldTexts.Length; i++)
        {
            if (worldTexts[i] == null)
            {
                worldTexts[i] = textCreated;
                return i;
            }
        }
        return 0;
    }

    void Start()
    {
        currentCanvas = transform.GetChild(0).gameObject;
        worldTexts = new TextMeshProUGUI[30];
    }

    // Update is called once per frame
    void Update()
    {
        if (createText)
        {
            createText = false;
            createWorldText(position, "Testando", textFontSize.Small);
        }
        if (deleteText)
        {
            deleteText = false;
            int numToDelete = 0;
            for (int i = 0; i < worldTexts.Length; i++)
            {
                if (worldTexts[i] != null)
                    numToDelete = i;

            }
            DeleteText(numToDelete);
        }
    }

    public void UpdateTextsPositions(int textIndex, UnityEngine.Vector3 position)
    {
        var obj = worldTexts[textIndex];
        obj.transform.position = position;

    }

    public void UpdateText(int textIndex, String text)
    {
        worldTexts[textIndex].text = text;
    }
    public void DeleteText(int textIndex)
    {
        worldTexts[textIndex].enabled = false;
        worldTexts[textIndex] = null;
    }

    public void GetStarted()
    {
        Debug.Log("World text acessed");
    }
}
