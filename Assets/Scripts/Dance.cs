using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dance : IAction
{
    public void Activate()
    {
        GameManager.Instance.UpdateActionText("DANCE");

        if(GameManager.Instance.DebugMode)
        {
            Debug.Log("Character dancing.");
        }
    }

    public bool CheckInput()
    {
        if(Input.GetKey(KeyCode.LeftArrow) &&
           Input.GetKey(KeyCode.RightArrow))
        {
            return true;
        }
        
        return false;
    }
}
