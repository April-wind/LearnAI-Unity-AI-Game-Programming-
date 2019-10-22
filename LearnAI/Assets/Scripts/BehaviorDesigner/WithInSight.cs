using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;

public class WithInSight : Conditional
{
    /*可视范围*/
    public float fieldofViewAngle;
    /*目标物体标签*/
    public string targetTag;
    /*目标物*/    
    public SharedTransform target;

    public Transform[] possibleTarget;
    // Start is called before the first frame update
    public override void OnAwake()
    {
        var targets = GameObject.FindGameObjectsWithTag(targetTag);
        possibleTarget = new Transform[targets.Length];
        for (int i = 0; i < targets.Length; i++)
        {
            possibleTarget[i] = targets[i].transform;
        }
    }

    public override TaskStatus OnUpdate()
    {
        for (int i = 0; i < possibleTarget.Length; ++i)
        {
            if (withInSight(possibleTarget[i], fieldofViewAngle))
            {
                //为行为树中的变量target赋值
                target.Value = possibleTarget[i];

                return TaskStatus.Success;
            }
        }
        return TaskStatus.Failure;
    }

    public bool withInSight(Transform targetTransform, float fieldOfViewAngle)
    {
        Vector3 direction = targetTransform.position - transform.position;

        //判断目标是否在视野范围内
        return Vector3.Angle(direction, transform.forward) < fieldOfViewAngle;
    }
}
