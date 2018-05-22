using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KeyInput : MonoBehaviour {
	public bool isPlayable;

	public abstract bool GetButton(string keyString);
	public abstract bool GetButtonDown(string keyString);
	public abstract bool GetButtonUp(string keyString);
	public abstract float GetAxis(string keyString);
}
