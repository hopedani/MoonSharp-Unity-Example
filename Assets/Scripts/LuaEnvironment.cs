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
    private Stack<MoonSharp.Interpreter.Coroutine> coroutineStack;
    
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
        coroutineStack = new Stack<MoonSharp.Interpreter.Coroutine>();

        //**!!! IMPORTANT! - the param given to the "Script" constructor will prevent the script from being able to exit the program and read files from disk!!!
        enviro = new Script(CoreModules.Preset_SoftSandbox);  
 
        UserData.RegisterType<GameState>(); //this was suppose to be "UserData.RegisterAssembly()" but that throws an error
        enviro.Globals["SetText"] = (Action<string>)LuaCommands.SetText;  //defines a type of function (one without a return type)
        enviro.Globals["ShowButtons"] = (Action<string,string>)LuaCommands.ShowButtons;
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
            coroutineStack.Push(enviro.CreateCoroutine(ret).Coroutine);
        }
    
    }

    public void AdvanceScript()
    {
        if (coroutineStack.Count > 0)
        {
            try
            {
                MoonSharp.Interpreter.Coroutine active = coroutineStack.Peek();
                DynValue ret = active.Resume();
                
                if (active.State == CoroutineState.Dead)
                {
                    coroutineStack.Pop();
                    Debug.Log("Coroutine Dead!");
                }

                if (ret.Type == DataType.Function)
                {
                    coroutineStack.Push(enviro.CreateCoroutine(ret).Coroutine);
                }
            }
            catch(ScriptRuntimeException ex)
            {
                Debug.LogError("AdvanceScript Issue - " + ex.DecoratedMessage);
                coroutineStack.Clear();
            }
            
        }
        else
        {
            Debug.Log("No active Dialogue");
        }
    }

}
