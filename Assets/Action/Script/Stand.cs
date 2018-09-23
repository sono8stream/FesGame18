using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stand : MonoBehaviour
{
    public int[] requiredMaterialIndexes;
    public int[] requiredMaterialCounts;

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
    int[] requiredMaterialIncrements;
    [SerializeField]
    int regenerateInterval = 200;
    [SerializeField]
    AudioClip generateMoneySE;

    Money currentMoney;
    Counter saleIntervalCounter;
    Counter regenerateCounter;
    int tempSales;
    Material defaultMoneyMaterial;

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
        ResetLevel();
        defaultMoneyMaterial
            = moneyObjectOrigin.GetComponent<SpriteRenderer>().sharedMaterial;
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
        currentMoney.transform.localPosition = Vector3.up * 4;
        currentMoney.colorID = (int)owner.teamColor;
        currentMoney.value = 100;
        currentMoney.GetComponent<SpriteRenderer>().material
            = GetComponent<SpriteRenderer>().material;
        SoundPlayer.Find().PlaySE(generateMoneySE);
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
            currentMoney.value += tempSales;
            currentMoney.transform.localScale
                = Vector3.one * (1 + currentMoney.value * 0.0001f);
            saleIntervalCounter.Initialize();
        }
    }

    public bool LevelUp(PlayerStatus playerStatus)
    {
        for (int i = 0; i < requiredMaterialIndexes.Length; i++)
        {
            int index = requiredMaterialIndexes[i];
            Debug.Log(index);
            if (playerStatus.MaterialCounts[index] < requiredMaterialCounts[i])
            {
                return false;
            }
        }

        for (int i = 0; i < requiredMaterialIndexes.Length; i++)
        {
            playerStatus.ReduceMaterial(
                requiredMaterialIndexes[i], requiredMaterialCounts[i]);

            if (requiredMaterialIncrements.Length <= i) continue;
            requiredMaterialCounts[i] += requiredMaterialIncrements[i];
        }
        tempSales += salesIncrement;
        return true;
    }

    public void ResetLevel()
    {
        tempSales = onceSales;
        Destroy(currentMoney.gameObject);
    }

    public void ReleaseMoney()
    {
        if (!currentMoney) return;
        /*currentMoney.GetComponent<SpriteRenderer>().material
            = Resources.GetBuiltinResource<Material>("Sprites-Default.mat");*/
        currentMoney.colorID = -1;
        currentMoney.gameObject.AddComponent<Rigidbody2D>();
        currentMoney.transform.SetParent(null);
        currentMoney.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 1000);
        currentMoney.GetComponent<SpriteRenderer>().material = defaultMoneyMaterial;
        Debug.Log("Released");
    }

    public void ResetGeneration()
    {
        if (regenerateCounter == null)
        {
            regenerateCounter = new Counter(regenerateInterval);
        }
        else
        {
            regenerateCounter.Initialize();
        }
    }
}