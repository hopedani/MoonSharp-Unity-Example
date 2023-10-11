using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    private LuaEnvironment lua;

    [SerializeField]
    private GameObject buttonParent;

   

    [SerializeField]
    private TMPro.TextMeshProUGUI textBtn1;// = new TMPro.TextMeshProUGUI();

    [SerializeField]
    private TMPro.TextMeshProUGUI textBtn2;// = new TMPro.TextMeshProUGUI();

    private void Start()
    {
        lua = FindObjectOfType<LuaEnvironment>();
        buttonParent.SetActive(false);
    }
    public void ButtonClicked(int index)
    {
        Debug.Log("Button clicked: " + index);
        lua.LuaGameState.ButtonSelected = index + 1;
        buttonParent.SetActive(false);
        lua.AdvanceScript();
    }

    public void ShowButtons(string txtBtn1, string txtBtn2)
    {
        textBtn1.text = txtBtn1;
        textBtn2.text = txtBtn2;
        buttonParent.gameObject.SetActive(true);
    }

    public bool AreButtonsVisible()
    {
        return buttonParent.gameObject.activeSelf;
    }
}
