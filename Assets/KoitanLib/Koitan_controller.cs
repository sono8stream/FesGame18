using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KoitanLib;

public class Koitan_controller : MonoBehaviour {
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {

        if(KoitanInput.GetButtonDown(ButtonID.A)){
            Debug.Log(KoitanInput.GetAxis(Axis.L_Horizontal));
        }

	}

}
