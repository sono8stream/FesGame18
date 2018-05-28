using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    new Vector3 pos;

    // Use this for initialization
    void Start()
    {
        pos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        pos.x = pos.x - 0.3f;
        transform.position = pos;
    }
}
