using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStatus
{
    public readonly string name,description;
    public readonly int range, power, rapid;
    public readonly Sprite imageSprite;

    public WeaponStatus(string name,string description,
        int range, int power, int rapid, Sprite image = null)
    {
        this.name = name;
        this.description = description;
        this.range = range;
        this.power = power;
        this.rapid = rapid;
        imageSprite = image;
    }
}
