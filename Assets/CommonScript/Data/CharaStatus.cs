using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaStatus
{
    public int power, defence, speed;
    public string name, description;
    public CharacterID id;

    public CharaStatus(string name, string description,
        int power, int defence, int speed)
    {
        this.name = name;
        this.description = description;
        this.power = power;
        this.defence = defence;
        this.speed = speed;
    }
}

public enum CharacterID
{
    Boy1, Boy2, Girl1, Girl2
}