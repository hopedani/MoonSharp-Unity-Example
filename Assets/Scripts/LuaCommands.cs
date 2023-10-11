using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuaCommands : MonoBehaviour
{
    private static LuaCommands _instance;
    public static LuaCommands Instance { get { return _instance; } }

    [SerializeField]
    private TMPro.TextMeshProUGUI textBox = new TMPro.TextMeshProUGUI();

    [SerializeField]
    private TMPro.TextMeshProUGUI textBtn1 = new TMPro.TextMeshProUGUI();

    [SerializeField]
    private TMPro.TextMeshProUGUI textBtn2 = new TMPro.TextMeshProUGUI();

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public static void SetText(string txt)
    {
        _instance.textBox.text = txt;
    }

    public static void ShowButtons(string txtBtn1, string txtBtn2)
    {
        _instance.textBtn1.text = txtBtn1;
        _instance.textBtn2.text = txtBtn2;
    }
}
