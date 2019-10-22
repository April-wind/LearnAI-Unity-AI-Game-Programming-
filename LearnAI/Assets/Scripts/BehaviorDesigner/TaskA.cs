using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class TaskA : Action
{
    public TaskB referencedTask;

    public override void OnAwake()
    {
        Debug.Log(referencedTask.someFloat);
    }
}
