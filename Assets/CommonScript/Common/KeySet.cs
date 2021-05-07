using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySet
{
    const int keyCount = 8;

    List<KeyCode[]> keycodesList;
    int setIndex;

    public KeySet(KeySetName setName)
    {
        Initialize();

        setIndex = (int)setName;
    }

    public KeyCode GetKey(KeyName keyName)
    {
        return keycodesList[setIndex][(int)keyName];
    }

    void Initialize()
    {
        keycodesList = new List<KeyCode[]>();
        keycodesList.Add(new KeyCode[keyCount]);//ps3
        keycodesList.Add(new KeyCode[keyCount]);//ps4
        keycodesList.Add(new KeyCode[keyCount]);//swL
        keycodesList.Add(new KeyCode[keyCount]);//swR
        keycodesList.Add(new KeyCode[keyCount] {
            KeyCode.RightArrow,KeyCode.LeftArrow,KeyCode.UpArrow,
            KeyCode.Backslash,KeyCode.DownArrow,KeyCode.RightShift,
            KeyCode.Backslash,KeyCode.RightShift,});
        keycodesList.Add(new KeyCode[keyCount] {
            KeyCode.D,KeyCode.A,KeyCode.W,
            KeyCode.Q,KeyCode.S,KeyCode.E,
            KeyCode.Q,KeyCode.E});
    }
}

public enum KeySetName
{
    PS3, PS4, SwitchLeft, SwitchRight, KeyboardL, KeyboardR
}

public enum KeyName
{
    Right = 0, Left, Jump, Attack, ChangeWeapon, ChangeStand, Enter, Cancel
}