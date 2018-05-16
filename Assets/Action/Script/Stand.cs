using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stand : MonoBehaviour
{
    [SerializeField]
    GameObject moneyObject;
    [SerializeField]
    int salesPerFrame;

    public int Money { get; private set; }

    
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Money += salesPerFrame;
    }

    private void OnDestroy()
    {
        
    }

}