using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandManager : MonoBehaviour {
	public bool isStand;
	public Player owner;
	public GameObject stand;
	public GameObject smokeEffect;
	private GameObject subjectObj;
	private SpriteRenderer spriteRenderer;
	private Stand yatai;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		yatai = GetComponentInChildren<Stand>();
		yatai.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(yatai.owner==null){
			isStand = false;
			spriteRenderer.enabled = true;
		}
	}

    //Collisionの方はいらない予定
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.tag == "Player")
        {			
			this.owner = collision.GetComponent<Player>();
			owner.aroundStand = this;
        }
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
        {			
            collision.GetComponent<Player>().aroundStand = null;
			owner = null;
        }
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

	public void CreateStand(){
		Instantiate(smokeEffect, transform.position, Quaternion.identity);
		yatai.gameObject.SetActive(true);
		yatai.owner = this.owner;
		isStand = true;
		spriteRenderer.enabled = false;
	}
}
