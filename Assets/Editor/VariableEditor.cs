using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class VariableEditor : EditorWindow
{
    const string VARIABLE_PATH = "Assets/Resources/Variable/variableList.txt";
    const int tempVarCount = 10;

    TextAsset variablesAsset;
    List<string> variables;
    List<string> variableNamesTemp;
    string[] variableNames;
    string[] allVariableNames;
    Vector2 variableScrollPosition;
    string variableName = "";
    int value;

    public string[] VariableNames { get { return variableNames; } }
    public string[] AllVariableNames { get { return allVariableNames; } }

    static void OnCreate()
    {
        GetWindow<VariableEditor>(typeof(ScriptEditor));
    }

    private void OnGUI()
    {
        using (new GUILayout.HorizontalScope())
        {
            using (new GUILayout.VerticalScope())
            {
                variableScrollPosition = EditorGUILayout.BeginScrollView(
                        variableScrollPosition, GUI.skin.box);
                WriteVariable();
                EditorGUILayout.EndScrollView();
                using (new GUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("削除"))
                    {
                        RemoveSelectedVariable();
                    }
                }
            }

            using(new GUILayout.VerticalScope())
            {
                using (new GUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("・変数名");
                    variableName = EditorGUILayout.TextField(variableName);
                }
                using (new GUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("・初期値");
                    value = EditorGUILayout.IntField(value);
                }
                if (GUILayout.Button("追加")&& !variableName.Equals(""))
                {
                    if (variableNamesTemp.FindIndex(x => x.Equals(variableName)) == -1)
                    {//重複防ぐ
                        variables.Add(string.Format("{0}:{1}", variableName, value));
                        variableNamesTemp.Add(variableName);
                    }
                }
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Save"))
                {
                    SaveVariable();
                }
            }
        }
    }

    public void Initialize()
    {
        LoadVariables();
        Debug.Log("Loaded");
    }

    void LoadVariables()
    {
        variablesAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(VARIABLE_PATH);
        variables = new List<string>();
        variables.AddRange(Regex.Split(variablesAsset.text, "\r\n|\r|\n"));
        variables.RemoveAll(x => string.IsNullOrEmpty(x));
        variableNames = variables.Select(x => x.Split(':')[0]).ToArray();
        variableNamesTemp = variables.Select(x => x.Split(':')[0]).ToList();//コピー

        List<string> allVarTemp = new List<string>();
        for (int i = 0; i < 10; i++)
        {
            allVarTemp.Add(string.Format("一次変数{0}",i));
        }
        allVarTemp.AddRange(variableNames);
        allVariableNames = allVarTemp.ToArray();
    }

    void WriteVariable()
    {
        if (variables == null) return;
        
        int cnt = 0;
        foreach (string s in variables)
        {
            GUI.SetNextControlName("v" + cnt.ToString());//毎回セットする必要あり
            EditorGUILayout.SelectableLabel(s, GUI.skin.textField, GUILayout.Height(16));
            cnt++;
        }
    }
    
    void SaveVariable()
    {
        string text = "";
        if (variables.Count > 0)
        {
            foreach (string s in variables)
            {
                text += s + Environment.NewLine;
            }
            text.Substring(0, text.Length - Environment.NewLine.Length);
        }

        using (StreamWriter sw = new StreamWriter(VARIABLE_PATH, false))
        {
            sw.Write(text);
            sw.Flush();
            sw.Close();
        }
        Debug.Log("Save Variable!");
        AssetDatabase.Refresh();
        LoadVariables();
    }

    void RemoveSelectedVariable()
    {
        if (variables.Count == 0) return;

        int index;
        string indexText = GUI.GetNameOfFocusedControl();
        if (indexText.Equals("") || !int.TryParse(indexText.Substring(1), out index)
            || index >= variables.Count) return;

        variables.RemoveAt(index);
        if (variables.Count == 0) return;

        string focus = index > 0 ? (index - 1).ToString() : "";
        GUI.FocusControl(focus);//表示更新のため、フォーカスを変える
    }

    public string GetVariableNameByIndex(int index)
    {
        if (allVariableNames == null
            || index < 0 || allVariableNames.Length < index) return "";
        
        if (index < tempVarCount)
        {
            return string.Format("@{0}@", index);
        }
        else
        {
            return string.Format("_{0}_", allVariableNames[index]);
        }
    }
}