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
}
