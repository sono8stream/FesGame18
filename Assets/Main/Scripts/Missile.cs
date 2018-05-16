using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AfterCollisionState{
	NotChange,
	Disappearance,
    Bound   
}

public class Missile : MonoBehaviour {
	public AfterCollisionState afterCollisionState;
	public float lifeTime;

	// Use this for initialization
	void Start () {
		Destroy(this.gameObject, lifeTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
}
