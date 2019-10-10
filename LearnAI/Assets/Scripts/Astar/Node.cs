using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Node : IComparable {

    /*起始节点和当前节点之间的移动估值*/
    public float nodeTotalCost;
    /*当前节点和目标节点之间的移动估值*/
    public float estimatedCost;
    /*障碍物标记*/
    public bool bObstacle;
    /*父节点*/
    public Node parent;
    /*障碍物位置*/
    public Vector3 position;

    public Node()
    {
        this.estimatedCost = 0.0f;
        this.nodeTotalCost = 1.0f;
        this.bObstacle = false;
        this.parent = null;
    }

    public Node(Vector3 pos)
    {
        this.estimatedCost = 0.0f;
        this.nodeTotalCost = 1.0f;
        this.bObstacle = false;
        this.parent = null;
        this.position = pos;
    }

    /// <summary>
    /// 可以将节点设置为障碍物对象
    /// </summary>
    public void MarkAsObstacle()
    {
        this.bObstacle = true;
    }

    public int CompareTo(object obj)
    {
        Node node = (Node)obj;
        //负值表示obj对象在排序顺序中位于此之前
        if (this.estimatedCost < node.estimatedCost) return -1;
        if (this.estimatedCost > node.estimatedCost) return 1;
        return 0;
    }
}