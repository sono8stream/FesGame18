using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class displayChange : MonoBehaviour {

    // Use this for initialization
    void Awake()
    {
#if UNITY_STANDALONE_OSX
        Screen.SetResolution(1440, 810, Screen.fullScreen);

#elif UNITY_STANDALONE_WIN
        Screen.SetResolution(1920, 1080, Screen.fullScreen);
#endif
    }

    // Update is called once per frame
    void Update () {
		
	}
}
