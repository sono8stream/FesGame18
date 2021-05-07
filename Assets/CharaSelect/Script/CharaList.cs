using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaList : MonoBehaviour {

    [SerializeField]
    GameObject[] previewOrigins;
    [SerializeField]
    AudioClip transitSE;

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

        if (KoitanLib.KoitanInput.ControllerCount() < 2) return;
        int selectCnt = 0;
        for(int i = 0; i < charaCnt; i++)
        {
            selectCnt += isSelected[i] ? 1 : 0;
        }
        if (selectCnt == KoitanLib.KoitanInput.ControllerCount())
        {
            UserData.instance.playerCount = selectCnt;
            LoadManager.Find().LoadScene(UserData.instance.selectedStageSceneNo);
            onEnd = true;
            SoundPlayer.Find().PlaySE(transitSE);
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
