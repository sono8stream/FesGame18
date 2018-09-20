using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField]
    List<GameObject> itemObjects;
    [SerializeField]
    List<float> generateRatios;
    [SerializeField]
    Rect generateRect;
    [SerializeField]
    int maxGenerateCount;
    [SerializeField]
    float zPosition;
    [SerializeField]
    float generateIntervalSec;//フレーム単位系

    TimeCounter timer;
    float ratioTotal;
    [SerializeField]
    List<GameObject> generatedObjects;

    // Use this for initialization
    void Start()
    {
        if(itemObjects.Count< generateRatios.Count)
        {
            generateRatios.Take(itemObjects.Count);
        }
        FormatRatio();
        timer = new TimeCounter(generateIntervalSec);
        timer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.OnLimit())
        {
            timer.Start();
            GenerateItem();
        }
    }

    void GenerateItem()
    {
        if (generatedObjects.Count >= maxGenerateCount)
        {
            //既に消えたアイテムをリストから削除
            generatedObjects.RemoveAll(x => x == null);
            if (generatedObjects.Count >= maxGenerateCount) return;
        }

        int index = RandomItemIndex();
        Vector3 pos = RandomItemPos();
        GameObject gameObject = Instantiate(itemObjects[index]);
        gameObject.transform.position = pos;
        generatedObjects.Add(gameObject);
        //Debug.Log(pos);
    }

    void FormatRatio()
    {
        for (int i = 1; i < generateRatios.Count; i++)
        {
            generateRatios[i] += generateRatios[i - 1];
        }
        ratioTotal = generateRatios[generateRatios.Count - 1];
    }

    int RandomItemIndex()
    {
        float val = UnityEngine.Random.Range(0, ratioTotal);
        int index = generateRatios.IndexOf(
            generateRatios.First(x => val < x));
        return index;
    }

    Vector3 RandomItemPos()
    {
        Vector3 minPos = generateRect.min;
        int i = 0;
        while (i<1000)
        {
            i++;
            Vector3 targetPos = minPos
                + new Vector3(UnityEngine.Random.Range(0, generateRect.width),
                              UnityEngine.Random.Range(0, generateRect.height));
            if (Physics2D.Raycast(targetPos, Vector3.forward * zPosition))
            {
                continue;
            }
            else
            {
                return targetPos + Vector3.forward * zPosition;
            }
        }
        return minPos + Vector3.forward * zPosition;
    }
}
