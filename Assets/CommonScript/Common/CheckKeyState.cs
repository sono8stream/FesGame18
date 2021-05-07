using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckKeyState : MonoBehaviour
{
    void Update()
    {
        DownKeyCheck();
    }

    void DownKeyCheck()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            //Debug.Log(Input.GetAxisRaw("Horizontal"));
        }

        if (Input.anyKeyDown)
        {
            foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(code))
                {
                    //処理を書く
                    Debug.Log(code);
                    break;
                }
            }
        }
    }
}
