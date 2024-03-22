// Copyright 2023 Niantic, Inc. All Rights Reserved.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillHelpText : MonoBehaviour
{
    [SerializeField] TextAsset textFile;
    [SerializeField] Text uiTextBox;
    // Scene info text to text box.
    void Start()
    {
        if(textFile != null)
            uiTextBox.text = textFile.text;
    }
    
    public void moveToBottom(){
        Vector2 pos = gameObject.transform.parent.GetComponent<RectTransform>().anchoredPosition;
        gameObject.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(pos.x,1332);
    }
}
