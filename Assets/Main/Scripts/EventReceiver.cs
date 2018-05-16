using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventReceiver : MonoBehaviour {
    public GameObject receiveObj;
    public AudioClip sound;
    private AudioSource audioSource;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ResetWeaponHitList()
	{
        //Debug.Log("リセット");
        receiveObj.GetComponent<HitBox>().HitedObjects.Clear();
	}

	public void ShotMissile(){
		receiveObj.GetComponent<Weapon>().ShotMissile();
	}

    public void PlaySound(){
        audioSource.PlayOneShot(sound);
    }

    /*
    public void ShowCollision(){
        ReceiveObj.GetComponent<Collider2D>().enabled = true;
        ReceiveObj.GetComponent<Renderer>().enabled = true;
    }

    public void HideCollision(){
        ReceiveObj.GetComponent<Collider2D>().enabled = false;
        ReceiveObj.GetComponent<Renderer>().enabled = false;
    }
    */

}
