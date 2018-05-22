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

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player"){
			collision.collider.GetComponent<Player>().aroundItem = this;
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player")
        {
			collision.collider.GetComponent<Player>().aroundItem = null;
        }
	}

    

	public abstract void PickUpReaction(Player owner);
	public abstract void ThrowReaction();
}
