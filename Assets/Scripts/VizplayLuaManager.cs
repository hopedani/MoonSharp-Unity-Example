using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


    public class VizplayLuaManager
    {
        public static List<Type> GetMoonSharpTypes(Assembly a)
        {
            List<Type> result = new List<Type>();

            foreach (Type t in a.GetTypes())
            {
                if (t.GetCustomAttributes(typeof(MoonSharp.Interpreter.MoonSharpUserDataAttribute), false).Length > 0)
                {
                    result.Add(t);
                }
            }

            return result;
        }
    }

