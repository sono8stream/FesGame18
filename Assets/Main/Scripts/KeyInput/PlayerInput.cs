using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : KeyInput {
	private Player owner;
	private int pID;

	private void Awake()
	{
		owner = GetComponent<Player>();
		pID = owner.PlayerID;
	}

	public override bool GetButton(string keyString)
	{
		return (isPlayable) ? Input.GetButton(keyString + pID) : false;
	}

	public override bool GetButtonDown(string keyString)
	{
		return (isPlayable) ? Input.GetButtonDown(keyString + pID) : false;
	}
    
	public override bool GetButtonUp(string keyString)
	{
		return (isPlayable) ? Input.GetButtonUp(keyString + pID) : false;
	}

	public override float GetAxis(string keyString){
		return (isPlayable) ? Input.GetAxis(keyString + pID) : 0;
	}
}
