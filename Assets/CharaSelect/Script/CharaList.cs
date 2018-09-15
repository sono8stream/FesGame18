﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaList : MonoBehaviour {

    [SerializeField]
    GameObject[] previewOrigins;

    public int charaCnt;
    public bool[] isSelected;

    CharacterID[] charaArray;
    bool onEnd;

    private void Awake()
    {
        charaArray = new CharacterID[charaCnt];
        for(int i = 0; i < charaCnt; i++)
        {
            charaArray[i] = (CharacterID)i;
        }
        isSelected = new bool[charaCnt];
    }

    private void Update()
    {
        if (onEnd) return;

        bool allSelect = true;
        for(int i = 0; i < charaCnt; i++)
        {
            allSelect = allSelect & isSelected[i];
        }
        if (allSelect)
        {
            LoadManager.Find().LoadScene(6);
            onEnd = true;
        }
    }

    public void SwitchSelectState(bool on, int index, Color color)
    {
        isSelected[index] = on;
        transform.GetChild(index).GetComponent<Image>().color = color;
    }

    public CharacterID this[int i]
    {
        get { return charaArray[i]; }
    }

    public GameObject GetPreview(int index)
    {
        return previewOrigins[index];
    }
}
