using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

    public StandManager standManager;
    private int tutorialLevel = 0;
    public Text tutorialText;
    public GameObject[] tutorialObjects;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        switch(tutorialLevel){
            //屋台を立てる
            case 0:
                if(standManager.isStand){
                    tutorialLevel++;
                }
                break;
                //お金回収
            case 1:

                break;
                //爆弾をもつ
            case 2:
                break;
                //屋台を壊す
            case 3:
                break;
            case 4:

                break;
            default:
                break;
        }
	}

}
