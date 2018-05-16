using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stand : MonoBehaviour
{
    [SerializeField]
    GameObject moneyObjectOrigin;
    [SerializeField]
    int salesPerFrame;

    int moneyAmount;
    GameObject moneyObject;
    
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        moneyAmount += salesPerFrame;
    }

    void OnDestroy()
    {
        
    }

    void GenerateMoneyObject()
    {
        moneyObject = Instantiate(moneyObjectOrigin);

        moneyObject.transform.SetParent(transform);
    }
}