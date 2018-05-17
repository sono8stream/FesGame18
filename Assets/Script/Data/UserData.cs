using System;
using System.Collections.Generic;

[Serializable]
public class UserData
{
    public Dictionary<string, int> variableDict;//消さない

    public static UserData instance = new UserData();

    private UserData()
    {
        //自動ロード処理
        UserData userData = SaveManager.Load();
        if (userData != null)
        {
            instance = userData;
            return;
        }

        variableDict = SaveManager.LoadVariableDict();
        if (variableDict == null) variableDict = new Dictionary<string, int>();

    }
}
