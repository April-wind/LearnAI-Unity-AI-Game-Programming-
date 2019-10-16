using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour
{
    private NavMeshAgent[] navAgents;

    public Transform targetMarker;
    // Start is called before the first frame update
    void Start()
    {
        navAgents = FindObjectsOfType(typeof(NavMeshAgent)) as NavMeshAgent[];
    }

    /// <summary>
    /// 更新新目标
    /// </summary>
    /// <param name="targetPosition"></param>
    void UpdateTargets(Vector3 targetPosition)
    {
        foreach (var VARIABLE in navAgents)
        {
            VARIABLE.destination = targetPosition; 
        }
    }
    // Update is called once per frame
    void Update()
    {
        int button = 0;

        if (Input.GetMouseButtonDown(button))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
            {
                Vector3 targetPosition = hitInfo.point;
                UpdateTargets(targetPosition);
                targetMarker.position = targetPosition + new Vector3(0, 5, 0);
            }
        }
    }
}
