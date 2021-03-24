using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IAction
{
    void Activate();
    bool CheckInput();
}
