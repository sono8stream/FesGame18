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
        GameObject obj = contact.collider.gameObject;
        if (obj.tag=="Player"){
            if (obj.GetComponent<Player>().state == State.HUTU)
            {
                GameObject tmpObj = Instantiate(subHitBox.Effect, contact.point, Quaternion.identity);
                AudioSource audioSource = tmpObj.AddComponent<AudioSource>();//音
                audioSource.PlayOneShot(subHitBox.HitSound);
            }
			if(afterCollisionState==AfterCollisionState.Disappearance){
				Destroy(this.gameObject);
			}            
		}      
	}
}
