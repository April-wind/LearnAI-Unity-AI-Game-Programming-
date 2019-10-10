using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour {

    private Transform startPos, endPos;
    public Node startNode
    {
        get;
        set;
    }
    public Node goalNode
    {
        get;
        set;
    }
    /*用来存储返回自AStar FindPath方法的节点数组*/
    public ArrayList pathArray;

    GameObject objStartCube, objEndCube;
    private float elapsedTime = 0.0f;
    //Interval time between pathfinding
    public float intervalTime = 1.0f;
    // Use this for initialization
    void Start () {
        objStartCube = GameObject.FindGameObjectWithTag("Start");
        objEndCube = GameObject.FindGameObjectWithTag("End");

        pathArray = new ArrayList();
        FindPath();
    }
	
    // Update is called once per frame
    void Update () {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= intervalTime) 
        {
            elapsedTime = 0.0f;
            FindPath();
        }
    }

    void FindPath()
    {
        startPos = objStartCube.transform;
        endPos = objEndCube.transform;

        startNode = new Node(GridManager.instance.GetGridCellCenter(GridManager.instance.GetGridIndex(startPos.position)));
        goalNode = new Node(GridManager.instance.GetGridCellCenter(GridManager.instance.GetGridIndex(endPos.position)));

        pathArray = AStar.FindPath(startNode, goalNode);
    }
    /// <summary>
    /// 遍历pathArray，绘制源自pathArray的，用以连接节点的直线
    /// </summary>
    void OnDrawGizmos()
    {
        if (pathArray == null)
        {
            return;
        }

        if (pathArray.Count > 0)
        {
            int index = 1;
            foreach(Node node in pathArray) {
                if (index < pathArray.Count)
                {
                    Node nextNode = (Node)pathArray[index];
                    Debug.DrawLine(node.position, nextNode.position, Color.green);
                    index++;
                }
            }
        }
    }
}