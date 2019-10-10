using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar  {
    public static PriorityQueue closedList, openList;
	
    /// <summary>
    /// 计算两个节点之间的估值
    /// </summary>
    /// <param name="curNode"></param>
    /// <param name="goalNode"></param>
    /// <returns></returns>
    private static float HeuristicEstimateCost(Node curNode,Node goalNode)
    {
        Vector3 vecCost = curNode.position - goalNode.position;
        return vecCost.magnitude;//向量的模
    }

    public static ArrayList FindPath(Node start,Node goal)
    {
        openList = new PriorityQueue();
        openList.Push(start);
        start.nodeTotalCost = 0.0f;
        start.estimatedCost = HeuristicEstimateCost(start, goal);

        closedList = new PriorityQueue();
        Node node = null;

        //下面对开放列表和闭合列表执行初始化操作。首先是起始节点，可将其置于开放列表中，并于随后对开放列表加以处理。
        while (openList.Length != 0)
        {
            node = openList.First();

            //check if the current node is the goal node
            if (node.position == goal.position)
            {
                return CalculatePath(node);
            }

            //Create an ArrayList to store the neighboring nodes
            ArrayList neighbours = new ArrayList();

            GridManager.instance.GetNeighbours(node, neighbours);

            for(int i = 0; i < neighbours.Count; i++)
            {
                Node neighbourNode = (Node)neighbours[i];

                if (!closedList.Contains(neighbourNode))
                {
                    float cost = HeuristicEstimateCost(node, neighbourNode);

                    float totalCost = node.nodeTotalCost + cost;
                    float neighbourNodeEstCost = HeuristicEstimateCost(neighbourNode, goal);

                    neighbourNode.nodeTotalCost = totalCost;
                    neighbourNode.parent = node;
                    neighbourNode.estimatedCost = totalCost + neighbourNodeEstCost;

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Push(neighbourNode);
                    }
                }
            }

            //经过上面的if该node的相邻节点都已被遍历过了，将该节点移至关闭列表
            closedList.Push(node);
            openList.Remove(node);   
        }
        //判断openlist中位于首位的node是不是goal
        if (node.position != goal.position)
        {
            Debug.LogError("Goal Not Found");
            return null;
        }
        return CalculatePath(node);
    }
    /// <summary>
    /// 跟踪各个节点的父节点对象，并创建数组列表
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private static ArrayList CalculatePath(Node node)
    {
        ArrayList list = new ArrayList();
        while (node != null)
        {
            list.Add(node);
            node = node.parent;
        }
        list.Reverse();
        return list;
    }
}
