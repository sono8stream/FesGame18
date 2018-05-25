using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum DestroyTrigger{
	Animation,
    Animator,
    Particle,
    Time
}

public class EffectDestroyer : MonoBehaviour {
    
	public DestroyTrigger dt;
	public float lifeTime = 0;
	private Animation animation;
	private Animator animator;

	// Use this for initialization
	void Start () {
		switch(dt){
			case DestroyTrigger.Animation:
				//アニメーション
                animation = GetComponent<Animation>();
				break;
			case DestroyTrigger.Animator:
				AnimatorClipInfo clipInfo = animator.GetCurrentAnimatorClipInfo(0)[0];
				Destroy(this.gameObject, clipInfo.clip.length);
				break;
			case DestroyTrigger.Particle:
				//パーティクル
                var tmpPS = gameObject.GetComponentsInChildren<ParticleSystem>();
				float maxL = tmpPS.Max(x => (x.startLifetime + x.duration));
                Destroy(gameObject, maxL);
				break;
			case DestroyTrigger.Time:
				Destroy(gameObject, lifeTime);
				break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(dt==DestroyTrigger.Animation){
			if (!animation.isPlaying) Destroy(this.gameObject);
		}
	}
}
