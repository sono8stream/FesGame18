using System;
using System.Collections.Generic;

public class CommandProcessor
{
    protected List<Func<bool>> commandList;
    protected int commandNo;
    protected string keyText;
    protected char trigger;
    public char Trigger { get { return trigger; } }

    public CommandProcessor()
    {
        commandNo = -1;
    }

    /// <summary>
    /// rawText format is usually [typeCode\no\key]
    /// for example [a\2\Hello]
    /// </summary>
    /// <param name="rawText">read line string</param>
    public virtual void ProcessBegin(string rawText)
    {
        string[] textSet = rawText.Split('\\');
        if (textSet.Length != 3)
        {
            commandNo = -1;
            return;
        }

        commandNo = int.Parse(textSet[1]);
        keyText = textSet[2].Substring(0, textSet[2].Length - 1);
    }

    /// <summary>
    /// process the command which suits "rawText" in commandList
    /// <returns>true: process completed</returns>
    public virtual bool Process()
    {
        if (commandNo < 0 || commandNo >= commandList.Count) return true;
        
        return commandList[commandNo]();
    }
}
