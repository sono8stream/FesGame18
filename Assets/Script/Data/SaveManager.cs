using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static bool Save(UserData target)
    {
        string prefKey = Application.dataPath + "/savedata.dat";
        MemoryStream memoryStream = new MemoryStream();
#if UNITY_IPHONE || UNITY_IOS
		System.Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
#endif
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(memoryStream, target);

        string tmp = System.Convert.ToBase64String(memoryStream.ToArray());
        try
        {
            PlayerPrefs.SetString(prefKey, tmp);
        }
        catch (PlayerPrefsException)
        {
            return false;
        }
        return true;
    }

    public static UserData Load()
    {
        string prefKey = Application.dataPath + "/savedata.dat";
        if (!PlayerPrefs.HasKey(prefKey))
        {
            return null;
        }
#if UNITY_IPHONE || UNITY_IOS
		System.Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
#endif
        BinaryFormatter bf = new BinaryFormatter();
        string serializedData = PlayerPrefs.GetString(prefKey);

        MemoryStream dataStream
            = new MemoryStream(System.Convert.FromBase64String(serializedData));
        UserData data = (UserData)bf.Deserialize(dataStream);

        return data;
    }

    public static Dictionary<string, int> LoadVariableDict()//指定形式の変数リストを読み込み
    {
        Dictionary<string, int> dict = new Dictionary<string, int>();

        string path = "Variable/variableList";
        TextAsset listFile = Resources.Load<TextAsset>(path);
        if (listFile == null) return null;

        string[] variableTree = Regex.Split(listFile.text, "\r\n|\r|\n");
        int variableCount = variableTree.Length;
        for (int i = 0; i < variableCount; i++)
        {
            string[] str = variableTree[i].Split(':');
            int value;
            if (!(str.Length == 2 && int.TryParse(str[1], out value))) continue;

            dict.Add(str[0], value);
        }

        return dict;
    }
}
