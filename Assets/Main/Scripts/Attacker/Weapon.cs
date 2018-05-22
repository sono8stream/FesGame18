using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Attacker {
	public InstantiateMissile[] instantiateMissiles;
	public HitBox hitBox;
	//public Player player;
	public Animator animator;
	public EventReceiver eventReceiver;
	public string clipName;

	// Use this for initialization
	private void Awake()
	{
		hitBox = GetComponent<HitBox>();
        player = transform.root.GetComponent<Player>();
        animator = transform.root.GetComponent<Animator>();
        eventReceiver = transform.root.GetComponent<EventReceiver>();
        instantiateMissiles = GetComponentsInChildren<InstantiateMissile>();
	}

	void Start () {		
		
	}


	void FixedUpdate () {
		if(animator.GetCurrentAnimatorStateInfo(0).IsName(clipName))
		{
			eventReceiver.receiveObj = this.gameObject;
		}
	}

	public void ShotMissile()
    {
        foreach (InstantiateMissile IM in instantiateMissiles)
        {
            int muki = player.anim.muki;
			//float kakudo = IM.
            //vec = new Vector2(vec.x * muki, vec.y);
			//float kakudo = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
			Quaternion rot = IM.bullet.transform.rotation * Quaternion.Euler(0, 90f - 90f * muki, IM.angle);
			GameObject missile = Instantiate(IM.bullet, IM.transform.position, rot);
            //Vector2 scale = missile.transform.localScale;
            //scale = new Vector2(scale.x * muki, scale.y);
            //missile.transform.localScale = scale;            
			missile.GetComponent<Rigidbody2D>().velocity = Quaternion.Euler(0,90f-90f*muki,IM.angle) * new Vector2(IM.speed, 0);
			missile.GetComponent<Attacker>().player = this.player;
        }
    }

	public override void HitReaction(ContactPoint2D contact, SubHitBox subHitBox)
	{
		if (contact.collider.gameObject.tag == "Player")
		{ 
			GameObject tmpObj = Instantiate(subHitBox.Effect, contact.point, Quaternion.identity);
        AudioSource audioSource = tmpObj.AddComponent<AudioSource>();//音
        audioSource.PlayOneShot(subHitBox.HitSound);            
		}
	}
}
