using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AfterCollisionState{
	NotChange,
	Disappearance,
    Bound   
}

public class Missile : Attacker {
	public AfterCollisionState afterCollisionState;
	public float lifeTime;

	// Use this for initialization
	void Start () {
		Destroy(this.gameObject, lifeTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void HitReaction(ContactPoint2D contact, SubHitBox subHitBox)
	{
		if(contact.collider.gameObject.tag=="Player"){
			GameObject tmpObj = Instantiate(subHitBox.Effect, contact.point, Quaternion.identity);
            AudioSource audioSource = tmpObj.AddComponent<AudioSource>();//音
            audioSource.PlayOneShot(subHitBox.HitSound);
			if(afterCollisionState==AfterCollisionState.Disappearance){
				Destroy(this.gameObject);
			}            
		}      
	}
}
