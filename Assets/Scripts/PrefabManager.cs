using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using System.IO;
using System;


public class PrefabManager : MonoBehaviour
{
    private static PrefabManager _instance;
    Script script;
    [SerializeField]
    private string luaScriptLocation;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        //hook up Lua's print to Unity Debug
        Script.DefaultOptions.DebugPrint = (s) => Debug.Log(s);

        //create script object and set Sand boxing rule
        script = new Script(CoreModules.Preset_SoftSandbox);

        //register C# function with Lua, so Lua script can make calls to it
        script.Globals["MakePrefab"] = (Action<string>)PrefabManager.MakePrefab;
        script.Globals["MakePrefabWithLocation"] = (Action<string, int, int, int>)PrefabManager.MakePrefabWithLocation;

        yield return 1;

        LoadFile(luaScriptLocation);
    }

    public static void MakePrefab(string prefabName)
    {
        Debug.Log("we are inside MakePrefab");
        GameObject obj = _instance.GetPrefabFromResources(prefabName);
        GameObject.Instantiate(obj, _instance.transform);
    }

    public static void MakePrefabWithLocation(string prefabName, int x, int y, int z)
    {
        GameObject obj = _instance.GetPrefabFromResources(prefabName);
        GameObject.Instantiate(obj, new Vector3(x, y, z), new Quaternion(0, 0, 0, 0),_instance.transform);
    }

    GameObject GetPrefabFromResources(string prefabName)
    {
        return (GameObject)Resources.Load(prefabName);
    }

    private void LoadFile(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        DynValue ret = DynValue.Nil;

        try
        {
             ret = script.DoFile(filePath);
        }
        catch (SyntaxErrorException ex)
        {
            Debug.Log("Lua Error: " + ex.DecoratedMessage);
        }

        //try
        //{
        //    using (BufferedStream stream = new BufferedStream(new FileStream(filePath, FileMode.Open, FileAccess.Read)))
        //    {
        //        ret = script.DoStream(stream);
        //    }
        //}
        //catch (SyntaxErrorException ex)
        //{
        //    Debug.Log("Lua Error: " + ex.DecoratedMessage);
        //}

        if(ret.Type == DataType.Function)
        {
            Debug.Log("Object should have been created");
        }
        else
        {
            Debug.Log("No Function found in lua script");
        }
    }
}
