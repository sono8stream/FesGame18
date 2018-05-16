using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDestroyer : MonoBehaviour {

	private Animation animation;

	// Use this for initialization
	void Start () {
		animation = GetComponent<Animation>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!animation.isPlaying) Destroy(this.gameObject);
	}
}
