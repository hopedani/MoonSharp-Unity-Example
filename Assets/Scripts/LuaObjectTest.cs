using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using UnityEngine.Scripting;
using System;

[Preserve]
[MoonSharpUserData]
public class LuaObjectTest
{
    public void SetActive(GameObject g, bool active)
    {
        g.SetActive(active);
    }

    public double calcHypotenuse(double a, double b)
    {
        return Math.Sqrt(a * a + b * b);
    }
}
