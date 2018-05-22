using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandManager : MonoBehaviour {
	public bool isStand;
	public bool owner;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    

	private void OnCollisionStay2D(Collision2D collision)
	{		
		if(collision.collider.gameObject.tag=="Player"){
			collision.collider.GetComponent<Player>().aroundStand = this;
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.collider.gameObject.tag == "Player")
        {
            collision.collider.GetComponent<Player>().aroundStand = null;
        }
	}
}
