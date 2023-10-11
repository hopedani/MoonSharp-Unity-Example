using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceInput : MonoBehaviour
{
    private LuaEnvironment lua;
    private void Start()
    {
        lua = FindObjectOfType<LuaEnvironment>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            lua.AdvanceScript();
        }
    }
}
