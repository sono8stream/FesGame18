using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPieces : MonoBehaviour
{

    const float fadeTime = 3;

    [SerializeField]
    List<Transform> childList;
    [SerializeField]
    List<Vector3> childDefualtPos;
    public bool onEndExplosion;
    private bool isExplode;
    private float tmpTime;

    // Use this for initialization
    void Start()
    {
        foreach (Transform child in transform)
        {
            childList.Add(child);
            childDefualtPos.Add(child.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isExplode)
        {
            tmpTime += Time.deltaTime;
            Debug.Log(tmpTime);
            if (tmpTime > fadeTime)
            {
                foreach (Transform child in childList)
                {
                    child.gameObject.SetActive(false);
                }
                onEndExplosion = true;
                isExplode = false;
            }
        }
    }

    public void Explosion()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        foreach (Transform child in childList)
        {
            child.SetParent(null);
            child.gameObject.SetActive(true);
        }
        isExplode = true;
        tmpTime = 0;
    }

    public void ResetP()
    {
        for (int i = 0; i < childList.Count; i++)
        {
            childList[i].position = childDefualtPos[i];
            childList[i].rotation = Quaternion.identity;
            childList[i].SetParent(this.transform);
            childList[i].gameObject.SetActive(false);
        }
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
        isExplode = false;
    }
}