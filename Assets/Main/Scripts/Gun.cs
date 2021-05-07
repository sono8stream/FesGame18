using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
	public InstantiateMissile[] instantiateMissiles;
	public Player player;
    public Animator animator;
    public EventReceiver eventReceiver;
    public string clipName;

    // Use this for initialization
    void Start()
    {
		instantiateMissiles = GetComponentsInChildren<InstantiateMissile>();
        player = transform.root.GetComponent<Player>();
        animator = transform.root.GetComponent<Animator>();
        eventReceiver = transform.root.GetComponent<EventReceiver>();
    }
	
	void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(clipName))
        {
            eventReceiver.receiveObj = this.gameObject;
        }
    }
    /*
	public void ShotMissile(){
		foreach(InstantiateMissile IM in instantiateMissiles){
			int muki = player.anim.muki;
			Vector2 vec = IM.speed;
            vec = new Vector2(vec.x * muki, vec.y);
			GameObject missile = Instantiate(IM.bullet, IM.transform.position, Quaternion.EulerAngles(vec));
			Vector2 scale = missile.transform.localScale;
			scale = new Vector2(scale.x * muki, scale.y);
			missile.transform.localScale = scale;           
            missile.GetComponent<Rigidbody2D>().velocity = vec;
			missile.GetComponent<HitBox>().player = this.player;
		}			     
	}
	*/
}
