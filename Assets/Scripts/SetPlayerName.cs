using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerName : MonoBehaviour
{
    [SerializeField]
    private string playerName;
    private void Start()
    {
        FindObjectOfType<LuaEnvironment>().LuaGameState.PlayerName = playerName;
    }
  
}
