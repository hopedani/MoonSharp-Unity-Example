using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    private LuaEnvironment lua;

    private void Start()
    {
        lua = FindObjectOfType<LuaEnvironment>();
    }
    public void ButtonClicked(int index)
    {
        Debug.Log("Button clicked: " + index);
        lua.LuaGameState.ButtonSelected = index + 1;
    }
}
