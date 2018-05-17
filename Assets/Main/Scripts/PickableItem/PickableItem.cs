using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickableItem : MonoBehaviour {
	public bool canPickUp;
	public Player owner;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerStay2D(Collider2D collider)
	{
		if(collider.gameObject.tag=="Player"){
			canPickUp = true;
			owner = collider.GetComponent<Player>();
			if(Input.GetKey(KeyCode.A)){
				this.transform.parent = owner.handPos;
				PickUpReaction(collider.gameObject);
			}
		}
	}

	public abstract void PickUpReaction(GameObject obj);
}
