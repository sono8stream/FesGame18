using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    public int Count { get { return objects.Count; } }

    Stack<GameObject> objects;
    int capacity;

    public ObjectPool(int capacity)
    {
        objects = new Stack<GameObject>();
        this.capacity = capacity;
    }

    public void Push(GameObject obj)
    {
        if (obj == null) return;

        obj.SetActive(false);
        objects.Push(obj);
    }

    public GameObject Pop()
    {
        if (Count == 0) return null;
        
        GameObject g = objects.Pop();
        g.SetActive(true);
        return g;
    }
}
