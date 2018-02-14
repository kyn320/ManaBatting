using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaitForAction : CustomYieldInstruction
{
    public bool isFinish = false;

    public override bool keepWaiting
    {
        get
        {
            return !isFinish;
        }
    }

    public void Finish() {
        isFinish = true;
    }
}
