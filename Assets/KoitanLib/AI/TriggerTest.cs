using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTest : MonoBehaviour
{

    List<GameObject> conList = new List<GameObject>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < conList.Count; i++)
        {
            if (conList[i] == null)
            {
                conList.RemoveAt(i);
                i--;
                continue;
            }
            Debug.DrawLine(transform.position, conList[i].transform.position, Color.red);
        }

    }

    private void OnDrawGizmos()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!conList.Contains(collision.gameObject))
        {
            conList.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (conList.Contains(collision.gameObject))
        {
            conList.Remove(collision.gameObject);
        }
    }
}
