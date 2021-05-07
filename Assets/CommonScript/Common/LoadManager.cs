﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour {

    public const string objectName = "SceneLoader";

    public Image FadeImage { get; private set; }

    [SerializeField]
    int fadeFrames;

    float fadeSpeed;
    Waiter fadeWaiter;

    // Use this for initialization
    void Awake()
    {
        gameObject.name = objectName;
        DontDestroyOnLoad(transform.parent.gameObject);
        fadeSpeed = 1.0f / fadeFrames;
        fadeWaiter = new Waiter(fadeFrames);
        FadeImage = GetComponent<Image>();
        FadeImage.enabled = false;
    }

    // Update is called once per frame
    void Update() { }

    public static LoadManager Find()
    {
        return GameObject.Find(objectName).GetComponent<LoadManager>();
    }

    public void LoadScene(int index, float loadSec = 0)
    {
        StartCoroutine(LoadSceneCoroutine(index, loadSec));
    }

    IEnumerator LoadSceneCoroutine(int index,float loadSec)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(index);
        async.allowSceneActivation = false;    // シーン遷移を待つ
        FadeImage.enabled = true;
        fadeWaiter.Initialize();
        while (!FadeIn() || async.progress < 0.9f)
        {
            //Debug.Log(async.progress);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(loadSec);
        async.allowSceneActivation = true;    // シーン遷移許可
        yield return new WaitForSeconds(0.2f);
        Debug.Log("Load");
        fadeWaiter.Initialize();
        while (!FadeOut())
        {
            yield return new WaitForEndOfFrame();
        }
        FadeImage.enabled = false;
    }

    bool FadeIn()
    {
        if (fadeWaiter.Wait()) return true;

        FadeImage.color += Color.black * fadeSpeed;
        return false;
    }

    bool FadeOut()
    {
        if (fadeWaiter.Wait()) return true;

        FadeImage.color -= Color.black * fadeSpeed;
        return false;
    }
}
