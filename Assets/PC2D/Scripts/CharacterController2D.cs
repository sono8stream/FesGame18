using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KoitanLib;

[RequireComponent(typeof(PlatformerMotor2D))]
public class CharacterController2D : MonoBehaviour {

    public PlatformerMotor2D _motor;
    public PC2D.AnimaController anim;
    public bool isPlayable;

    protected Player owner;
    [SerializeField]
    protected int orderNo;

    // Use this for initialization
    protected virtual void Start()
    {
        _motor = GetComponent<PlatformerMotor2D>();
        anim = GetComponent<PC2D.AnimaController>();
        owner = GetComponent<Player>();
        orderNo = owner.PlayerID;
        Debug.Log(orderNo);
    }
}
