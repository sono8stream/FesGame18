using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectVanisher : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
        Destroy(gameObject, particleSystem.main.duration);
    }
}
