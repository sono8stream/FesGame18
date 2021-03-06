﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Attacker
{
    public InstantiateMissile[] instantiateMissiles;
    public HitBox hitBox;
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

    void Start()
    {

    }


    void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(clipName))
        {
            eventReceiver.receiveObj = this.gameObject;
        }
    }

    public void ShotMissile()
    {
        foreach (InstantiateMissile IM in instantiateMissiles)
        {
            int muki = player.anim.muki;
            Quaternion rot = IM.bullet.transform.rotation * Quaternion.Euler(0, 90f - 90f * muki, IM.angle);
            GameObject missile = Instantiate(IM.bullet, IM.transform.position, rot);
            missile.GetComponent<Rigidbody2D>().velocity
                = Quaternion.Euler(0, 90f - 90f * muki, IM.angle)
                * new Vector2(IM.speed, 0);
            missile.GetComponent<Attacker>().player = this.player;
        }
    }

    public override void HitReaction(ContactPoint2D contact, SubHitBox subHitBox)
    {
        GameObject obj = contact.collider.gameObject;
        if (obj.tag == "Player")
        {
                Debug.Log("あたったエフェクト");
                GameObject tmpObj = Instantiate(
                subHitBox.Effect, contact.point, Quaternion.identity);
                AudioSource audioSource = tmpObj.AddComponent<AudioSource>();//音
                audioSource.PlayOneShot(subHitBox.HitSound);
        }
    }
}
