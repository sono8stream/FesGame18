using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

/// <summary>
/// シナリオスクリプトから、コメントや空行を除いた有効な文字列を随時返す
/// 文字コードをutf-8にしてください(それ以外だとunityで認識されません)
/// 
/// ・テキスト形式
/// // ←コメント、無視
/// 空行、3文字以上の空文字 ←無視
/// [A:1:key] ←特殊コマンド呼び出し
/// ー終点 ←ラベル
/// →終点 ←ジャンプ
/// 
/// その他はそのまま返す
/// 
/// </summary>
//[Serializable]
public class TextLoader
{
    string[] validTexts;
    Counter textsCounter;

    public TextLoader(string initialScript)
    {
        validTexts = SplitScript(initialScript);
        textsCounter = new Counter(validTexts.Length);
        //DebugScript();
    }

    public string[] SplitScript(string text)//うまいアルゴリズムが書けない
    {
        text = text.Replace("//", "\r\n//");
        string[] allLines= Regex.Split(text, "\r\n|\r|\n");
        string[] validLines//まずコメント、空行無視
            = allLines.Where(x => !x.StartsWith("//") && !String.IsNullOrEmpty(x)).ToArray();
        validLines //特殊コマンド前後分割
            = validLines.Select(x => x.Replace("[", "\r[").Replace("]", "]\r")).ToArray();

        List<string> validList = new List<string>();
        foreach (string str in validLines)//特殊コマンド分割
        {
            validList.AddRange(Regex.Split(str, "\r\n|\r|\n"));
        }

        List<string> texts = new List<string>();
        int textLines = validList.Count;
        bool lastNormalMessage = false;//ただのメッセージかどうか、連結時の判定に利用
        for (int i = 0; i < textLines; i++)
        {
            string str = validList[i].Trim();//インデントや空行などの最初の空白は無視

            if (str.StartsWith("//") || String.IsNullOrEmpty(str)) continue;//コメント、空を無視

            char first = str[0];
            if (lastNormalMessage && first != '[' && first != 'ー' && first != '→')
            {
                texts[texts.Count - 1] += "\r\n" + str;//メッセージ連結
            }
            else
            {
                texts.Add(str);
                lastNormalMessage = first != '[' && first != 'ー'
                    && first != '→';//通常文字か判定
            }
        }

        return texts.ToArray();
    }

    /// <summary>
    /// スクリプトを一行ずつ読み込み
    /// </summary>
    /// <returns>終端:null</returns>
    public string ReadLine()
    {
        while (true)
        {
            if (textsCounter.OnLimit()) return null;

            string targetText = validTexts[textsCounter.Now];
            Debug.Log(targetText);
            textsCounter.Count();
            if (targetText[0] == 'ー') continue;//ラベル飛ばす, StartsWithだと適切に分岐しない

            if (targetText[0] == '→')
            {//ジャンプ
                JumpLabel(targetText.Substring(1));
                continue;
            }

            return targetText;
        }
    }

    public void JumpLabel(string labelName)
    {
        int index = Array.FindIndex(validTexts, x =>
        x[0] == 'ー' && x.Substring(1).Equals(labelName));
        if (index != -1) textsCounter.Now = index;
    }

    void DebugScript()
    {
        int scriptLength = validTexts.Length;
        for (int i = 0; i < scriptLength; i++)
        {
            Debug.Log(validTexts[i]);
        }
    }
}