using MoonSharp.Interpreter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

[Preserve]
[MoonSharpUserData]
public class GameState

{
    private string playerName;
    private int buttonSelected;
    private HashSet<string> flags;
   

    [MoonSharpHidden]
    public GameState()
    {
        flags = new HashSet<string>();
    }
   
    public string PlayerName
    {
        get
        {
            return playerName;
        }
        [MoonSharpHidden]
        set
        {
            playerName = value;
        }
    }

    public int ButtonSelected
    {
        get { return buttonSelected; }
        [MoonSharpHidden]
        set { buttonSelected = value; }
    }

    public bool GetFlag(string flag)
    {
        return flags.Contains(flag);
    }

    public void SetFlag(string flag, bool set)
    {
        if (set)
        {
            flags.Add(flag);
        }
        else
        {
            flags.Remove(flag);
        }
    }
}
