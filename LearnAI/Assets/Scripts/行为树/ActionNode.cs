 using System.Collections;
using System.Collections.Generic;
 using System.Xml.Schema;
 using UnityEngine;

 /// <summary>
 /// 通用叶节点
 /// </summary>
public class ActionNode : Nodes
{
    public delegate NodeStates ActionNodeDelegate();

    private ActionNodeDelegate m_action;

    public ActionNode(ActionNodeDelegate action)
    {
        m_action = action;
    }

    public override NodeStates Evaluate()
    {
        //默认为失败
        switch (m_action())
        {
            case NodeStates.SUCCESS:
                m_NodeState = NodeStates.SUCCESS;
                return m_NodeState;
            case NodeStates.FAILURE:
                m_NodeState = NodeStates.FAILURE;
                return m_NodeState;
            case NodeStates.RUNNINT:
                m_NodeState = NodeStates.RUNNINT;
                return m_NodeState;
            default:
                m_NodeState = NodeStates.FAILURE;
                return m_NodeState;
        }
    }
}
