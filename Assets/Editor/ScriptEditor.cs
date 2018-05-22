using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.IO;

public class ScriptEditor : EditorWindow
{
    const string SCRIPT_FOLDER_PATH = "Assets/Resources/ScenarioScript/";
    const string CHARA_FOLDER_PATH = "Assets/Resources/Portrait/";
    const string SCENERY_FOLDER_PATH = "Assets/Resources/Scenery/";
    const string BGM_FOLDER_PATH = "Assets/Resources/BGM/";
    const string SE_FOLDER_PATH = "Assets/Resources/SE/";

    static VariableEditor variableEditor;
    static string[] sceneNames;

    bool okDialog = false;

    TextAsset script = null;
    string scriptName;
    List<string> scriptLines = new List<string>();
    List<bool> scriptToggles = new List<bool>();
    Vector2 scriptScrollPosition = Vector2.zero;
    int varIndex = 0;

    CommandMessages messages = new CommandMessages();
    string inputMessage = "";
    int messageSpeed;
    string choiceName;

    int charaNo;
    int moveLength;
    Sprite charaSprite = null;
    Sprite scenerySprite = null;

    AudioClip bgm;
    AudioClip se;

    TextAsset changeScript = null;
    int sceneIndex;

    int changeVarIndex = 0;
    string[] operators = new string[5] { "=", "+=", "-=", "*=", "/=" };
    int operatorIndex = 0;
    int changeVal;
    int subVarIndex = 0;

    [MenuItem("Editor/ScriptEditor")]
    private static void OnCreate()
    {
        //最もよくわかっていない部分　この順番じゃないとダメっぽい
        ScriptEditor editor = GetWindow<ScriptEditor>();
        editor.position = new Rect(150, 150, 1160, 660);//サイズ変更
        variableEditor = GetWindow<VariableEditor>(typeof(ScriptEditor));
        editor.Focus();
        variableEditor.Initialize();

        InitializeSceneNames();
        //Debug.Log("OnCreate終了");
    }

    private void OnGUI()
    {
        using (new GUILayout.HorizontalScope())
        {
            TextAsset tempScript = EditorGUILayout.ObjectField("・スクリプト",
                    script, typeof(TextAsset), false, GUILayout.Width(300)) as TextAsset;
            EditorGUILayout.LabelField("・保存ファイル名");
            scriptName = EditorGUILayout.TextField(scriptName);
            if (tempScript != script)
            {
                script = tempScript;
                scriptName = script.name;
                LoadScript();
            }
        }//ファイル読み込み
        EditorGUILayout.Space();

        using (new GUILayout.HorizontalScope(GUILayout.Height(500)))
        {
            using (new GUILayout.VerticalScope(GUILayout.Width(600)))
            {
                scriptScrollPosition = EditorGUILayout.BeginScrollView(
                        scriptScrollPosition, GUI.skin.box);
                WriteScript();
                EditorGUILayout.EndScrollView();

                SwitchFocusedToggle();

                using (new GUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("選択行に空行追加", EditorStyles.miniButtonLeft))
                    {
                        InsertLine(" ");
                    }
                    if (GUILayout.Button("↓一行コピー", EditorStyles.miniButtonMid))
                    {
                        int index = scriptToggles.FindIndex(x => x);
                        if (index >= 0)
                        {
                            inputMessage = scriptLines[index];
                        }
                    }
                    if (GUILayout.Button("全選択 解除", EditorStyles.miniButtonMid))
                    {
                        scriptToggles = scriptToggles.Select(x => x = false).ToList();
                    }
                    if (GUILayout.Button("選択行 削除", EditorStyles.miniButtonRight))
                    {
                        RemoveSelectedLine();
                    }
                }
            }

            using (new GUILayout.VerticalScope(GUI.skin.box/*,GUILayout.Width(200)*/))
            {
                MessageGUI();
                ImageGUI();
                SoundGUI();
                SceneGUI();
                VariableGUI();
            }
        }

        using (new GUILayout.HorizontalScope(GUI.skin.box))
        {
            using (new GUILayout.VerticalScope())
            {
                GUILayout.Label("・メッセージ");
                //max length: 54*3=162

                inputMessage = EditorGUILayout.TextArea(inputMessage,
                    GUILayout.Width(600), GUILayout.Height(45));
                using (new GUILayout.HorizontalScope(GUILayout.Width(600)))
                {
                    string[] strs = Regex.Split(inputMessage, "\r\n|\r|\n");
                    if (GUILayout.Button("書き込み(入力待ち初期化)",
                        EditorStyles.miniButtonLeft))
                    {
                        foreach (string s in strs)
                        {
                            InsertLine(s);
                        }
                        InsertLine(@"[m\1\0]");
                    }
                    if (GUILayout.Button("書き込み(入力待ち)", EditorStyles.miniButtonMid))
                    {
                        foreach (string s in strs)
                        {
                            InsertLine(s);
                        }
                        InsertLine(@"[m\2\0]");
                    }
                    if (GUILayout.Button("書き込み", EditorStyles.miniButtonMid))
                    {
                        foreach (string s in strs)
                        {
                            InsertLine(s);
                        }
                    }
                    if (GUILayout.Button("ラベル", EditorStyles.miniButtonMid))
                    {
                        if (strs.Length > 0 && !string.IsNullOrEmpty(strs[0]))
                        {
                            InsertLine("ー" + strs[0]);
                        }
                    }
                    if (GUILayout.Button("コメント", EditorStyles.miniButtonMid))
                    {
                        foreach (string s in strs)
                        {
                            if (!string.IsNullOrEmpty(s))
                            {
                                InsertLine("//" + s);
                            }
                        }
                    }
                    if (GUILayout.Button("クリア", EditorStyles.miniButtonRight))
                    {
                        foreach (string s in strs)
                        {
                            inputMessage = "";
                        }
                    }
                }
            }

            using (new GUILayout.VerticalScope())
            {
                if (variableEditor != null && variableEditor.AllVariableNames != null)
                {
                    varIndex = EditorGUILayout.Popup(
                        "・変数表示", varIndex, variableEditor.AllVariableNames);

                    if (GUILayout.Button("変数表示"))
                    {
                        inputMessage += variableEditor.GetVariableNameByIndex(varIndex);
                    }
                }
            }
        }
        GUILayout.FlexibleSpace();

        SaveButton();
    }

    #region file management

    void SaveButton()
    {
        if (GUILayout.Button("Save") && !string.IsNullOrEmpty(scriptName))
        {
            string rawScript = "";
            if (scriptLines.Count > 0)
            {
                foreach (string s in scriptLines)
                {
                    rawScript += s + Environment.NewLine;
                }
                rawScript = rawScript.Substring(
                    0, rawScript.Length - Environment.NewLine.Length * 2);//最後の空行消す
                SaveScript(rawScript);
            }
        }
    }

    void SaveScript(string text)
    {
        string path = SCRIPT_FOLDER_PATH + string.Format("{0}.txt", scriptName);

        if (File.Exists(path) && !EditorUtility.DisplayDialog(
                "ファイル重複", "上書きしますか?", "OK", "キャンセル")) return;

        using (StreamWriter sw = new StreamWriter(path, false))
        {
            sw.Write(text);
            sw.Flush();
            sw.Close();
        }
        Debug.Log("Save Data!");
        AssetDatabase.Refresh();
        //AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        LoadScript();
    }

    void LoadScript()
    {
        string path = SCRIPT_FOLDER_PATH + string.Format("{0}.txt", scriptName);
        script = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
        if (!File.Exists(path))
        {
            script = null;
            return;
        }

        scriptLines = new List<string>();
        scriptLines.AddRange(Regex.Split(script.text, "\r\n|\r|\n"));
        scriptLines.Add("");
        int lineCount = scriptLines.Count;
        scriptToggles = new List<bool>();
        for (int i = 0; i < lineCount; i++)
        {
            scriptToggles.Add(false);
        }
        Debug.Log(scriptToggles.Count);
    }
    #endregion

    #region script box management
    void WriteScript()
    {
        if (script == null) return;

        //string[] strs = Regex.Split(scriptText, Environment.NewLine);
        int cnt = 0;
        //foreach (string s in strs)
        foreach (string s in scriptLines)
        {
            using (new GUILayout.HorizontalScope())
            {
                scriptToggles[cnt] = EditorGUILayout.Toggle(
                    scriptToggles[cnt], GUILayout.Width(15));
                GUI.SetNextControlName(cnt.ToString());//毎回セットする必要あり
                EditorGUILayout.SelectableLabel(messages.GetCommandMessage(s),
                    GUI.skin.textField, GUILayout.Height(16));
            }
            cnt++;
        }
    }

    void InsertLine(string text)
    {
        if (string.IsNullOrEmpty(text)) return;

        int length = scriptToggles.Count;
        for (int i = 0; i < length; i++)
        {
            if (!scriptToggles[i]) continue;

            scriptLines.Insert(i, text);
            scriptToggles.Insert(i, false);
            length++;
            i++;
            Debug.Log("Inserted");
        }

        if (!scriptToggles[scriptToggles.Count - 1] && length == scriptToggles.Count)
        {
            int index = scriptToggles.Count - 1;
            scriptLines.Insert(index, text);
            scriptToggles.Insert(index, false);
            scriptToggles[index + 1] = true;
        }
    }

    void RemoveSelectedLine()
    {
        int checkLength = scriptToggles.Count - 1;
        for (int i = 0; i < checkLength; i++)
        {
            if (!scriptToggles[i]) continue;

            scriptLines.RemoveAt(i);
            scriptToggles.RemoveAt(i);
            checkLength--;
            i--;
        }
    }

    void SwitchFocusedToggle()
    {
        int index;
        string indexText = GUI.GetNameOfFocusedControl();
        if (indexText.Equals("") || !int.TryParse(indexText, out index)
            || index >= scriptLines.Count) return;

        scriptToggles[index] = !scriptToggles[index];
        GUI.FocusControl("");
    }
    #endregion

    #region command management
    void MessageGUI()
    {
        using (new GUILayout.VerticalScope(GUI.skin.box))
        {
            using (new GUILayout.HorizontalScope())
            {
                messageSpeed = Mathf.Clamp(EditorGUILayout.IntField(messageSpeed), 0, 10);
                if (GUILayout.Button("速度変更"))
                {
                    InsertLine(string.Format(@"[m\4\{0}]", messageSpeed.ToString()));
                }
            }

            using (new GUILayout.HorizontalScope())
            {
                choiceName = EditorGUILayout.TextField(choiceName);
                if (GUILayout.Button("選択肢追加"))
                {
                    InsertLine(string.Format(@"[m\5\{0}]", choiceName));
                }
            }

            if (GUILayout.Button("選択待ち"))
            {
                InsertLine(@"[m\6\0]");
            }

            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("オート開始"))
                {
                    InsertLine(@"[m\7\100]");
                }
                if (GUILayout.Button("オート終了"))
                {
                    InsertLine(@"[m\8\0]");
                }
            }
        }
    }

    void ImageGUI()
    {
        using (new GUILayout.VerticalScope(GUI.skin.box))
        {
            using (new GUILayout.HorizontalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("・キャラ番号(0～3)");
                charaNo = Mathf.Clamp(EditorGUILayout.IntField(charaNo), 0, 3);
            }

            using (new GUILayout.HorizontalScope())
            {
                charaSprite = EditorGUILayout.ObjectField("・キャラ画像",
                        charaSprite, typeof(Sprite), false, GUILayout.Width(300)) as Sprite;
                if (charaSprite != null)
                {
                    string charaPath = CHARA_FOLDER_PATH + charaSprite.name;
                    if (!(File.Exists(charaPath + ".png")
                        || File.Exists(charaPath + ".jpg"))) charaSprite = null;
                }

                if (GUILayout.Button("キャラ画像変更") && charaSprite != null)
                {
                    InsertLine(string.Format(@"[i\1\{0}:{1}]", charaNo, charaSprite.name));
                }
            }

            if (GUILayout.Button("フキダシ追加"))
            {
                InsertLine(string.Format(@"[i\2\{0}]", charaNo));
            }

            if (GUILayout.Button("フキダシ非表示"))
            {
                InsertLine(@"[i\2\-1]");
            }

            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("・移動幅");
                moveLength = EditorGUILayout.IntField(moveLength);
                if (GUILayout.Button("キャラ移動"))
                {
                    InsertLine(string.Format(@"[i\3\{0}:{1}]", charaNo, moveLength));
                }
            }

            using (new GUILayout.HorizontalScope())
            {
                scenerySprite = EditorGUILayout.ObjectField("・背景画像",
                        scenerySprite, typeof(Sprite), false, GUILayout.Width(300)) as Sprite;
                if (scenerySprite != null)
                {
                    string sceneryPath
                        = SCENERY_FOLDER_PATH + scenerySprite.name;
                    if (!(File.Exists(sceneryPath + ".png")
                        || File.Exists(sceneryPath + ".jpg"))) scenerySprite = null;
                }

                if (GUILayout.Button("背景画像変更") && scenerySprite != null)
                {
                    InsertLine(string.Format(@"[i\4\{0}]", scenerySprite.name));
                }
            }
        }
    }

    void SoundGUI()
    {
        using (new GUILayout.VerticalScope(GUI.skin.box))
        {
            using (new GUILayout.HorizontalScope())
            {
                bgm = EditorGUILayout.ObjectField(
                    "・BGM", bgm, typeof(AudioClip), false) as AudioClip;
                if (bgm != null)
                {
                    string bgmPath
                        = BGM_FOLDER_PATH + bgm.name;
                    if (!(File.Exists(bgmPath + ".mp3")
                        || File.Exists(bgmPath + ".wav"))) bgm = null;
                }

                if (GUILayout.Button("BGM変更") && bgm != null)
                {
                    InsertLine(string.Format(@"[s\0\{0}]", bgm.name));
                }
            }

            if (GUILayout.Button("BGM停止"))
            {
                InsertLine(@"[s\1\0]");
            }

            if (GUILayout.Button("BGM再開"))
            {
                InsertLine(@"[s\2\0]");
            }

            using (new GUILayout.HorizontalScope())
            {
                se = EditorGUILayout.ObjectField(
                    "・SE", se, typeof(AudioClip), false) as AudioClip;
                if (se != null)
                {
                    string sePath
                        = SE_FOLDER_PATH + se.name;
                    if (!(File.Exists(sePath + ".mp3")
                        || File.Exists(sePath + ".wav"))) se = null;
                }

                if (GUILayout.Button("SE変更") && se != null)
                {
                    InsertLine(string.Format(@"[s\3\{0}]", se.name));
                }
            }
        }
    }

    void SceneGUI()
    {
        using (new GUILayout.VerticalScope(GUI.skin.box))
        {
            using (new GUILayout.HorizontalScope())
            {
                changeScript = EditorGUILayout.ObjectField(
                    "・スクリプト", changeScript, typeof(TextAsset), false) as TextAsset;
                if (changeScript != null)
                {
                    string scriptPath
                        = SCRIPT_FOLDER_PATH + changeScript.name;
                    if (!(File.Exists(scriptPath + ".txt"))) changeScript = null;
                }

                if (GUILayout.Button("スクリプト切替"))
                {
                    InsertLine(string.Format(@"[e\0\{0}]", changeScript.name));
                }
            }

            using (new GUILayout.HorizontalScope())
            {
                if (sceneNames != null)
                {
                    sceneIndex = EditorGUILayout.Popup(
                        "・シーン", sceneIndex, sceneNames);
                }
                if (GUILayout.Button("シーン切り替え"))
                {
                    InsertLine(string.Format(@"[e\1\{0}]", sceneIndex));
                }
            }
        }
    }

    void VariableGUI()
    {
        using (new GUILayout.HorizontalScope(GUI.skin.box))
        {
            if (variableEditor != null && variableEditor.AllVariableNames != null)
            {
                changeVarIndex = EditorGUILayout.Popup(
                    "・変数操作", changeVarIndex, variableEditor.AllVariableNames,
                    GUILayout.Width(250));

                operatorIndex = EditorGUILayout.Popup("", operatorIndex, operators,
                    GUILayout.Width(50));

                using (new GUILayout.VerticalScope())
                {
                    changeVal = EditorGUILayout.IntField(changeVal, GUILayout.Width(100));
                    subVarIndex = EditorGUILayout.Popup(
                        "", subVarIndex, variableEditor.AllVariableNames,
                        GUILayout.Width(100));
                }

                using (new GUILayout.VerticalScope())
                {
                    if (GUILayout.Button("変数変更", GUILayout.Width(100)))
                    {
                        InsertLine(string.Format(@"[v\0\{0}:{1}:{2}]",
                            variableEditor.GetVariableNameByIndex(changeVarIndex),
                            operators[operatorIndex], changeVal));
                    }
                    if (GUILayout.Button("変数変更", GUILayout.Width(100)))
                    {
                        InsertLine(string.Format(@"[v\0\{0}:{1}:{2}]",
                            variableEditor.GetVariableNameByIndex(changeVarIndex),
                            operators[operatorIndex],
                            variableEditor.GetVariableNameByIndex(subVarIndex)));
                    }
                }
            }
        }
    }
    #endregion

    static void InitializeSceneNames()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        sceneNames = new string[sceneCount];
        for (int i = 0; i < sceneCount; i++)
        {
            string name = SceneManager.GetSceneByBuildIndex(i).name;
            if (string.IsNullOrEmpty(name))
            {
                name = i.ToString();
            }
            sceneNames[i] = name;
        }
    }
}