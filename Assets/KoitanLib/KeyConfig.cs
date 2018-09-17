using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KoitanLib;

public class KeyConfig : MonoBehaviour
{
    [SerializeField]
    Text text;

    bool onConfig;

    // Use this for initialization
    void Start()
    {
        text.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (onConfig)
            {
                KoitanInput.EndReconnection();
                text.gameObject.SetActive(false);
            }
            else
            {
                KoitanInput.StartReconnection();
                text.gameObject.SetActive(true);
            }
            onConfig = !onConfig;
        }

        if (onConfig)
        {
            text.text= KoitanInput.ControllerNames();
        }
    }
}