using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuaCommands : MonoBehaviour
{
    private static LuaCommands _instance;
    public static LuaCommands Instance { get { return _instance; } }

    [SerializeField]
    private TMPro.TextMeshProUGUI textBox;

    private ButtonHandler buttons;

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

    private void Start()
    {
       buttons = FindObjectOfType<ButtonHandler>();
    }

    public static void SetText(string txt)
    {
        _instance.textBox.text = txt;
    }

    public static void ShowButtons(string txtBtn1, string txtBtn2)
    {
        _instance.buttons.ShowButtons(txtBtn1, txtBtn2);
    }


}
