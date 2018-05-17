using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour {

    public const string objectName = "SceneLoader";

    [SerializeField]
    int fadeFrames;
    float fadeSpeed;
    bool fadeIn, fadeOut;
    int sceneIndex;
    Waiter fadeWaiter;
    Image image;

    public Image FadeImage{ get { return image; } }

    // Use this for initialization
    void Awake()
    {
        gameObject.name = objectName;
        sceneIndex = -1;
        fadeSpeed = 1.0f / fadeFrames;
        fadeWaiter = new Waiter(fadeFrames);
        image = GetComponent<Image>();
        image.enabled = false;
    }

    // Update is called once per frame
    void Update() { }

    public static LoadManager Find()
    {
        return GameObject.Find(objectName).GetComponent<LoadManager>();
    }

    public void LoadScene(int index)
    {
        if (index < 0 || SceneManager.sceneCount < index)
        {
            Debug.Log(string.Format("{0} is out of range!", index));
            return;
        }
        Debug.Log("ChangeScene");
        StartCoroutine(LoadSceneCoroutine(index));
    }

    IEnumerator LoadSceneCoroutine(int index)
    {
        if (sceneIndex == index)
        {
            sceneIndex = -1;
        }
        AsyncOperation async = SceneManager.LoadSceneAsync(index);
        async.allowSceneActivation = false;    // シーン遷移を待つ
        image.enabled = true;
        while (!FadeIn() || async.progress < 0.9f)
        {
            Debug.Log(async.progress);
            yield return new WaitForEndOfFrame();
        }
        async.allowSceneActivation = true;    // シーン遷移許可
        yield return new WaitForSeconds(0.2f);

        while (!FadeOut())
        {
            yield return new WaitForEndOfFrame();
        }
        image.enabled = false;
    }

    bool FadeIn()
    {
        if (fadeWaiter.Wait())
        {
            fadeWaiter.Initialize();
            return true;
        }

        image.color += Color.black * fadeSpeed;
        return false;
    }

    bool FadeOut()
    {
        if (fadeWaiter.Wait())
        {
            fadeWaiter.Initialize();
            return true;
        }

        image.color -= Color.black * fadeSpeed;
        return false;
    }
}