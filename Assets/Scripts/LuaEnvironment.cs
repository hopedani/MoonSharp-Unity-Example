using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using System.IO;
using System;
using MoonSharp.Interpreter.Loaders;
using System.Reflection;

public class LuaEnvironment : MonoBehaviour
{
    [SerializeField]
    private string loadFile;

    Script enviro;
    private MoonSharp.Interpreter.Coroutine activeCoroutine;
    private GameState luaGameState;

    public GameState LuaGameState
    {
        get { return luaGameState; }
    }

    private void Awake()
    {
        luaGameState = new GameState();
    }

    private void Start()
    {
        StartCoroutine(Initailize());
    }

    // Start is called before the first frame update
    private IEnumerator Initailize()
    {
        Script.DefaultOptions.DebugPrint = (s) => Debug.Log(s);
       

        enviro = new Script();
 
        UserData.RegisterType<GameState>(); //this was suppose to be "UserData.RegisterAssembly()" but that throws an error
        enviro.Globals["SetText"] = (Action<string>)LuaCommands.SetText;  //defines a type of function (one without a return type)
        enviro.Globals["State"] = UserData.Create(luaGameState);

        yield return 1;

        ////test print something
        //string testScript = "print('Hello World')";
        //DynValue testPrint = enviro.DoString(testScript);
        //Debug.Log(testPrint.Type);
        LoadFile(loadFile);
        AdvanceScript();

    }

    private void LoadFile(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        DynValue ret = DynValue.Nil;

        try
        {
            using (BufferedStream stream = new BufferedStream(new FileStream(filePath, FileMode.Open, FileAccess.Read)))
            {
                ret = enviro.DoStream(stream);
            }
        }
        catch (SyntaxErrorException ex)
        {
            Debug.Log("Lua Error: " + ex.DecoratedMessage);
        }

        if (ret.Type == DataType.Function)
        {
            activeCoroutine = enviro.CreateCoroutine(ret).Coroutine;
        }
        else
        {
            activeCoroutine = null;
        }
    }

    public void AdvanceScript()
    {
        if (activeCoroutine != null)
        {
            try
            {
                activeCoroutine.Resume();
                if (activeCoroutine.State == CoroutineState.Dead)
                {
                    activeCoroutine = null;
                    Debug.Log("Dialogue Complete!");
                }
            }
            catch(ScriptRuntimeException ex)
            {
                Debug.LogError("AdvanceScript Issue - " + ex.DecoratedMessage);
            }
            
        }
        else
        {
            Debug.Log("No active Dialogue");
        }
    }

}
