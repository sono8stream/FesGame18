using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FieldLooper : MonoBehaviour
{
    [SerializeField]
    bool vertical;
    [SerializeField]
    Vector2 loopPos;

    // Use this for initialization
    void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Vector2 destinationPos = loopPos;
        Transform targetTransform = collider.transform;
        if(vertical)
        {
            destinationPos.y = targetTransform.position.y;
        }
        else
        {
            destinationPos.x = targetTransform.position.y;
        }
        targetTransform.position = destinationPos;
    }
}