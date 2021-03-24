using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Color : IAction
{
    enum Colors 
    {
        BLUE,
        RED,
        ORANGE,
        YELLOW,
    }

    private Dictionary<Colors, KeyCode> ColorToKey = new Dictionary<Colors, KeyCode>() 
    {   {Colors.BLUE, KeyCode.Q}, 
        {Colors.RED, KeyCode.W}, 
        {Colors.ORANGE, KeyCode.E}, 
        {Colors.YELLOW, KeyCode.R}, };

    private Colors currentColor;

    private System.Random random = new System.Random();

    public void Activate()
    {
        int numOfColors = Enum.GetNames(typeof(Colors)).Length;

        currentColor = (Colors)random.Next(0, numOfColors);

        GameManager.Instance.UpdateActionText(currentColor.ToString());

        if(GameManager.Instance.DebugMode)
        {
            Debug.Log($"Color: {currentColor.ToString()}");
        }
    }

    public bool CheckInput()
    {
        if(Input.GetKeyDown(ColorToKey[currentColor]))
        {
            return true;
        }
        
        return false;
    }
}
