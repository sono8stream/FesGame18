using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotation : MonoBehaviour {
    public float rotationSpeed;
    public float stopAngle;
    private AngleState angleState = AngleState.Left;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(angleState == AngleState.Left){
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            if((transform.rotation.eulerAngles.z > 180 ? transform.rotation.eulerAngles.z - 360 : transform.rotation.eulerAngles.z) > stopAngle){
                angleState = AngleState.Right;
            }
        }
        else{
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
            if ((transform.rotation.eulerAngles.z > 180 ? transform.rotation.eulerAngles.z -360 : transform.rotation.eulerAngles.z )< -stopAngle)
            {
                angleState = AngleState.Left;
            }
        }

	}

    private enum AngleState
    {
        Left,
        Right
    }
}
