using MoonSharp.Interpreter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;


[Preserve]
[MoonSharpUserData]
public class GameObjectLuaProxy
{
    public static void Destroy(GameObject g)
    {
        GameObject.Destroy(g);
    }

    //public static ThisGameObjectLuaProxy Find(string gameobject_name)
    //{
    //    return new ThisGameObjectLuaProxy(GameObject.Find(gameobject_name));
    //}

    public static void Instantiate(GameObject prefab)
    {
        GameObject.Instantiate(prefab);
    }

    public static void DontDestoryOnLoad(GameObject g)
    {
        GameObject.DontDestroyOnLoad(g);
    }
}

