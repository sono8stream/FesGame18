using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SceneLoaderやSoundPlayerなど、シーン共通のオブジェクトを呼び出し、自然消滅
/// </summary>
public class CommonLoader : MonoBehaviour
{
    public static string loadedName = "CommonObjects";

    [SerializeField]
    GameObject[] commonOrigins;

    void Awake()
    {
        if (GameObject.Find(loadedName))
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.name = loadedName;
            LoadCommons();
            DontDestroyOnLoad(gameObject);
            Destroy(this);
        }
    }

    void LoadCommons()
    {
        int objectCount = commonOrigins.Length;
        for(int i = 0; i < objectCount; i++)
        {
            GameObject g = Instantiate(commonOrigins[i]);
            g.transform.SetParent(transform);
        }
    }
}