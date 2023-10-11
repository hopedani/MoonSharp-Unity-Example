using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceInput : MonoBehaviour
{
    private LuaEnvironment lua;
    private ButtonHandler buttons;

    private void Start()
    {
        buttons = GetComponent<ButtonHandler>();
        lua = FindObjectOfType<LuaEnvironment>();
    }
    // Update is called once per frame
    void Update()
    {
        if (buttons.AreButtonsVisible() == false && Input.GetKeyDown(KeyCode.Space))
        {
            lua.AdvanceScript();
        }
    }
}
