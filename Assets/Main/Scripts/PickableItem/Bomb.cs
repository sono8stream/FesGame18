using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : PickableItem {
	public float lifeTime;
	private Rigidbody2D rb;
	private bool isFire;
	public GameObject explosionEffect;

    [SerializeField]
    AudioClip explodeSE;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {		
		if(isFire){
			lifeTime -= Time.deltaTime;
			if(lifeTime<=0){
				Explosion();
			}
		}
	}

	public override void PickUpReaction(Player owner)
	{
		isFire = true;
		this.owner = owner;
		this.transform.parent = owner.handPos;
        this.transform.position = owner.handPos.position;
        owner.anim._animator.Play("pick_up");
        owner.anim._animator.CrossFade("Idle", 1f);
        owner.havingItem = this;
        GetComponent<Rigidbody2D>().simulated = false;
	}

	public override void ReleaseReaction(Player owner)
	{
		this.owner = null;
		this.transform.SetParent(null);
		owner.havingItem = null;
		GetComponent<Rigidbody2D>().simulated=true;
	}

	public void Explosion(){		
		GameObject tmpObj = Instantiate(explosionEffect, transform.position, Quaternion.identity);
		tmpObj.GetComponent<Attacker>().player = this.owner;
		tmpObj.GetComponent<HitBox>().owner = this.owner;
		Destroy(this.gameObject);
        SoundPlayer.Find().PlaySE(explodeSE);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(isFire){
			if(collision.collider.GetComponent<Player>()!=owner){
				Explosion();
			}				
		}
        if (collision.collider.gameObject.tag == "Attacker")
        {
            Explosion();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //めっちゃ重い
        /*
        if (collision.gameObject.tag == "Attacker")
        {
            Explosion();
        }
        */
    }

    public override void ThrowReaction()
	{
		transform.parent = null;
		rb.velocity = new Vector2(10f * owner.anim.muki, 2);
		rb.simulated = true;
		owner.havingItem = null;      
	}
}
