using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : IAction
{
    public void Activate()
    {
        GameManager.Instance.UpdateActionText("JUMP");

        if(GameManager.Instance.DebugMode)
        {
            Debug.Log("Character jumping.");
        }
    }

    public bool CheckInput()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            return true;
        }

        return false;
    }
}
