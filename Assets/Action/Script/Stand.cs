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
    GameObject currentMoneyObject;
    
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SaleMoney();
    }

    void OnDestroy()
    {
        
    }

    void GenerateMoneyObject()
    {
        currentMoneyObject = Instantiate(moneyObjectOrigin);

        currentMoneyObject.transform.SetParent(transform);
        currentMoneyObject.transform.localPosition = Vector3.zero;

    }

    void SaleMoney()
    {
        moneyAmount += salesPerFrame;
        currentMoneyObject.transform.localScale = Vector3.one * moneyAmount * 0.001f;
    }
}