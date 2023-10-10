using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using System;
using System.Linq;
using MoonSharp.Interpreter.Loaders;

//DH-Using this script to learn how to use Lua with C#
public class LuaTutorial : MonoBehaviour
{
    public bool runFunction = false;

   
    // Update is called once per frame
    void Update()
    {
        if (runFunction)
        {
            runFunction = false;
            Debug.Log("MoonSharp Factorial = " + MoonSharpFactorial2());
            LuaTypesTest();
            Debug.Log("Callback Factorial = " + CallbackTest());
            Debug.Log("Enumerable Test = " + EnumerableTest());
            Debug.Log("TableTest1 = " + TableTest1());
            Debug.Log("TableTest2 = " + TableTest2());
            Debug.Log("TableTestReverseSafter = " + TableTestReverseWithTable());
            Debug.Log("CallMyClass1 = " + CallMyClass1());
            Debug.Log("Event called = " + Events());
        }
    }

    double MoonSharpFactorial()
    {
        string scriptCode = @"
            -- defines a factorial function
            function fact (n)
                if ( n == 0) then
                    return 1
                else
                    return n*fact(n-1)
                end
            end

            return fact(myNumber)";

        Script script = new Script();

        script.Globals["myNumber"] = 7;

        DynValue res = script.DoString(scriptCode);
        return res.Number;
    }

    //Call a Lua function from C#
    double MoonSharpFactorial2()
    {
        string scriptCode = @"
            -- defines a factorial function
            function fact (n)
                if ( n == 0) then
                    return 1
                else
                    return n*fact(n-1)
                end
            end";

        //create a "global" Script obj, script sticks around for reuse
        Script script = new Script();

        //compile the script
        script.DoString(scriptCode);

        //grab the "fact" function and store it in a "DynValue" variable
        DynValue luaFactFunction = script.Globals.Get("fact");

        //grab the result of the "fact" function and the parameter for the "fact" function
        DynValue res = script.Call(luaFactFunction, DynValue.NewNumber(4));

        //return the result of the "fact" function
        return res.Number;
    }


    void LuaTypesTest()
    {
        DynValue v1 = DynValue.NewNumber(1);

        DynValue v2 = DynValue.NewString("hey there!");

        DynValue v3 = DynValue.FromObject(new Script(), "word");

        Debug.Log(v1.Type + " " + v2.Type + " " + v3.Type);

        Debug.Log(v1.Number + " " + v2.String + " " + v2.Number);

        //Tuples! not just for 2 values anymore :)
        DynValue ret = Script.RunString("return false, 'hey hey hey', 2*7, 'another', true, 6*7");

        Debug.Log(ret.Type);

        for(int i = 0; i < ret.Tuple.Length; i++)
        {
            Debug.Log(ret.Tuple[i].Type + " = " + ret.Tuple[i]);
        }
    }

    int Mul(int a, int b)
    {
        return a * b;
    }
    
    double CallbackTest()
    {
        string scriptCode = @"
            --defines a factorial function
            function fact (n)
                if(n == 0) then
                    return 1
                else
                    return Mul(n, fact(n-1));
                end
            end";

        Script script = new Script();

        //this line sets the "Global" variable to the "Mul" function/delegate... cast is to please the C# compiler (Func takes 2 "ins" and 1 "out" hence the 3 "int"s)
        script.Globals["Mul"] = (Func<int, int, int>)Mul;

        script.DoString(scriptCode);

        DynValue res = script.Call(script.Globals["fact"], 4);

        return res.Number;
    }

    IEnumerable<int> GetNumbers()
    {
        for(int i = 0; i <= 10; i++)
        {
            yield return i;
        }
    }

    double EnumerableTest()
    {
        string scriptCode = @"
            total = 0;

            for i in getNumbers() do
                total = total + i;
            end

            return total;
        ";

        Script script = new Script();

        script.Globals["getNumbers"] = (Func<IEnumerable<int>>)GetNumbers;

        DynValue res = script.DoString(scriptCode);

        return res.Number;
    }

    List<int> GetNumberList()
    {
        List<int> lst = new List<int>();

        for(int i = 1; i <= 10; i++)
        {
            lst.Add(i);
        }

        return lst;
    }

    double TableTest1()
    {
        string scriptCode = @"
            total = 0;

            tbl = getNumbers();

            for _, i in ipairs(tbl) do
                total = total + i;
            end

            return total;
        ";

        Script script = new Script();

        script.Globals["getNumbers"] = (Func<List<int>>)GetNumberList;

        DynValue res = script.DoString(scriptCode);

        return res.Number;
    }

    Table GetNumberTable(Script script)
    {
        Table tbl = new Table(script);

        //apparently Lua Tables are 1- indexed (not 0- indexed like C#)
        for(int i = 1; i <=10; i++)
        {
            tbl[i] = i;
        }

        return tbl;
    }

    double TableTest2()
    {
        string scriptCode = @"
            total = 0;
            
            --the line below will actually use the GetNumberTable function from above (after linking that C# function to the getNumbers global name)
            tbl = getNumbers()

            for _, i in ipairs(tbl) do
                total = total + i;
            end

            return total;

        ";

        Script script = new Script();

        script.Globals["getNumbers"] = (Func<Script, Table>)(GetNumberTable);

        DynValue res = script.DoString(scriptCode);

        return res.Number;
    }

    //DH - THIS EXAMPLE DOES NOT WORK... result is always zero
    //double TableTestReverseSafer()
    //{
    //    string scriptCode = @"
    //        return dosum { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }
    //    ";

    //    Script script = new Script();

    //    script.Globals["dosum"] = (Func<List<object>, int>)(l => l.OfType<int>().Sum()); //this is a "Linq" operation

    //    DynValue res = script.DoString(scriptCode);

    //    return res.Number;
    //}

    static double Sum(Table t)
    {
        var nums = from v in t.Values
                   where v.Type == DataType.Number
                   select v.Number;

        return nums.Sum();
    }

    private static double TableTestReverseWithTable()
    {
        string scriptCode = @"    
        return dosum { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }
    ";

        Script script = new Script();

        script.Globals["dosum"] = (Func<Table, double>)Sum;

        DynValue res = script.DoString(scriptCode);

        return res.Number;
    }

    double CallMyClass1()
    {
        string scriptCode = @"
            return obj.calcHypotenuse(3,4);
        ";

        UserData.RegisterType<LuaObjectTest>();

        Script script = new Script();

        DynValue obj = UserData.Create(new LuaObjectTest());

        script.Globals.Set("obj", obj);

        DynValue res = script.DoString(scriptCode);

        return res.Number;
    }

    class MyClass
    {
        public event EventHandler SomethingHappened;

        public void RaiseTheEvent()
        {
            if(SomethingHappened != null)
            {
                SomethingHappened(this, EventArgs.Empty);
            }
        }
    }

    string Events()
    {
        string scriptCode = @"
            function handler(o, a)
                print('handled!', o, a);
            end

            myobj.somethingHappened.add(handler);
            myobj.raiseTheEvent();
            myobj.somethingHappened.remove(handler);
            myobj.raiseTheEvent();
            return 'handled!';
        ";

        UserData.RegisterType<EventArgs>();
        UserData.RegisterType<MyClass>();

        Script script = new Script();
        script.Globals["myobj"] = new MyClass();
        DynValue res = script.DoString(scriptCode);
        return res.String;
    }

    //set script loader for unity
    void SetScriptLoaderForUnity()
    {
        //you must add "using MoonSharp.Interpreter.Loaders;" to the ref's to do this
        Script.DefaultOptions.ScriptLoader = new UnityAssetsScriptLoader();
    }

    //Custom script loader
    private class MyCustomScriptLoader : ScriptLoaderBase
    {
        public override object LoadFile(string file, Table globalContext)
        {
            return string.Format("print ([[A request to load '{0}' has been made]]", file);
        }

        public override bool ScriptFileExists(string name)
        {
            return true;
        }
    }

    //Running custom script loader
    void CustomScriptLoader()
    {
        Script script = new Script();
       

        script.Options.ScriptLoader = new MyCustomScriptLoader()
        {
            ModulePaths = new string[] { "?_module.lua" }
        };

        script.DoString(@"
                require 'somemodule'
                f = loadfile 'someothermodule.lua'
                f()
        ");
    }
}
