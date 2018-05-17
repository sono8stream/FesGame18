using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stand : MonoBehaviour
{
    [SerializeField]
    GameObject moneyObjectOrigin;
    [SerializeField]
    int onceSales = 50;
    [SerializeField]
    int maxMoneyAmount = 1000;
    [SerializeField]
    int saleInterval = 5;
    [SerializeField]
    int salesIncrement = 10;
    [SerializeField]
    int[] requiredMaterialCounts;
    [SerializeField]
    int requiredMaterialIncrement;

    int moneyAmount;
    GameObject currentMoneyObject;
    Counter saleIntervalCounter;
    
    // Use this for initialization
    void Start()
    {
        saleIntervalCounter = new Counter(saleInterval);
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
        moneyAmount = 0;

    }

    void SaleMoney()
    {
        if (currentMoneyObject == null)
        {
            GenerateMoneyObject();
        }

        if (saleIntervalCounter.Count())
        {
            moneyAmount += onceSales;
            currentMoneyObject.transform.localScale
                = Vector3.one * (1 + moneyAmount * 0.001f);
            saleIntervalCounter.Initialize();
        }
    }

    public void UpdateSales(int newOnceSales)
    {
        onceSales = newOnceSales;

    }
}