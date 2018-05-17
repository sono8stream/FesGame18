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
    int[] requiredMaterialIndexes;
    [SerializeField]
    int requiredMaterialCounts;
    [SerializeField]
    int requiredMaterialIncrement;
    [SerializeField]
    int regenerateInterval = 200;

    int moneyAmount;
    GameObject currentMoneyObject;
    Counter saleIntervalCounter;
    Counter regenerateCounter;

    // Use this for initialization
    void Start()
    {
        saleIntervalCounter = new Counter(saleInterval);
        regenerateCounter = new Counter(regenerateInterval, true);
    }

    // Update is called once per frame
    void Update()
    {
        SaleMoney();
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
            if (regenerateCounter.Count())
            {
                GenerateMoneyObject();
                regenerateCounter.Initialize();
            }
            else
            {
                return;
            }
        }

        if (saleIntervalCounter.Count())
        {
            moneyAmount += onceSales;
            currentMoneyObject.transform.localScale
                = Vector3.one * (1 + moneyAmount * 0.005f);
            saleIntervalCounter.Initialize();
        }
    }

    public void LevelUp(PlayerStatus playerStatus)
    {
        for (int i = 0; i < requiredMaterialIndexes.Length; i++)
        {
            int index = requiredMaterialIndexes[i];
            if (playerStatus.MaterialCounts[index] < requiredMaterialCounts)
            {
                return;
            }
        }

        for (int i = 0; i < requiredMaterialIndexes.Length; i++)
        {
            playerStatus.ReduceMaterial(
                requiredMaterialIndexes[i], requiredMaterialCounts);
            requiredMaterialCounts += requiredMaterialIncrement;
        }
    }
}