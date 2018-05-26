using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    int[] requiredMaterialCounts;
    [SerializeField]
    int[] requiredMaterialIncrements;
    [SerializeField]
    int regenerateInterval = 200;

    Money currentMoney;
    Counter saleIntervalCounter;
    Counter regenerateCounter;

	public Player owner;

    // Use this for initialization
    void Start()
    {
        saleIntervalCounter = new Counter(saleInterval);
        regenerateCounter = new Counter(regenerateInterval, true);

        int indexesLength = requiredMaterialIndexes.Length;
        int countsLength = requiredMaterialCounts.Length;
        if (indexesLength < countsLength)
        {
            requiredMaterialCounts
                = requiredMaterialCounts.Take(indexesLength).ToArray();
        }
        else
        {
            requiredMaterialIndexes
                = requiredMaterialIndexes.Take(countsLength).ToArray();
        }
    }

    // Update is called once per frame
    void Update()
    {
        SaleMoney();
    }

    void GenerateMoneyObject()
    {
        currentMoney = Instantiate(moneyObjectOrigin).GetComponent<Money>();

        currentMoney.transform.SetParent(transform);
        currentMoney.transform.localPosition = Vector3.zero;

    }

    void SaleMoney()
    {
        if (currentMoney == null)
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
            currentMoney.value += onceSales;
            currentMoney.transform.localScale
                = Vector3.one * (1 + currentMoney.value * 0.001f);
            saleIntervalCounter.Initialize();
        }
    }

    public void LevelUp(PlayerStatus playerStatus)
    {
        for (int i = 0; i < requiredMaterialIndexes.Length; i++)
        {
            int index = requiredMaterialIndexes[i];
            if (playerStatus.MaterialCounts[index] < requiredMaterialCounts[index])
            {
                return;
            }
        }

        for (int i = 0; i < requiredMaterialIndexes.Length; i++)
        {
            playerStatus.ReduceMaterial(
                requiredMaterialIndexes[i], requiredMaterialCounts[i]);

            if (requiredMaterialIncrements.Length <= i) continue;
            requiredMaterialCounts[i] += requiredMaterialIncrements[i];
        }
    }
}