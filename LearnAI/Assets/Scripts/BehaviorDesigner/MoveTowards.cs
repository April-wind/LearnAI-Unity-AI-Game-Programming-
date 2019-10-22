using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

using UnityEngine;

public class MoveTowards : Action
{
    public float speed = 0;
    /*行为树中的变量*/
    public SharedTransform target;

    public override TaskStatus OnUpdate()
    {
        if (Vector3.SqrMagnitude(transform.position - target.Value.position) < 0.1f)
        {
            return TaskStatus.Success;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.Value.position, speed * Time.deltaTime);
        return TaskStatus.Running;
    }
}
