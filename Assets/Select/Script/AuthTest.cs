using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthTest : MonoBehaviour {

    WWWForm form;

    // Use this for initialization
    void Start()
    {
        form = new WWWForm();
        form.AddField("code",
            "4/QwDwytDg_L8EDrsWtREU5PprCi5fI--gwivuGM8HBBEs80yowNaxFYl926k0YpSHyrCWgMV4XdKl5VjZHpEKOr4#");
        form.AddField("client_id",
            "193121282174-imoafa5fu33seiqdumtb7tpnjtvbnc2d.apps.googleusercontent.com");
        form.AddField("client_secret",
            "ds8We15i74Cst352BNTJ5NvR");
        form.AddField("redirect_url", "http://localhost");
        form.AddField("grant_type", "authorization_code");
        form.AddField("access_type", "offline");
        form.headers.Add("Content-Type", "application/x-www-form-urlencoded");
        Dictionary<string, string> headers = form.headers;
        byte[] rawData = form.data;
        var www = new WWW("https://www.googleapis.com/oauth2/v4/token",
            rawData, headers);
        Debug.Log(www.text);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
