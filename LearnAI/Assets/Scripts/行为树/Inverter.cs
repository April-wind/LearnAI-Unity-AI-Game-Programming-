using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : Nodes
{
    private Nodes m_node;

    public Nodes node
    {
        get { return m_node; }
    }

    public Inverter(Nodes nodes)
    {
        m_node = nodes;
    }
    public override NodeStates Evaluate()
    { 
        switch (m_node.Evaluate())
        {
            case NodeStates.FAILURE:
                m_NodeState = NodeStates.SUCCESS;
                return m_NodeState;
            case NodeStates.SUCCESS:
                m_NodeState = NodeStates.FAILURE;
                return m_NodeState;
            case NodeStates.RUNNINT:
                m_NodeState = NodeStates.RUNNINT;
                return m_NodeState;
        }
        //默认为success
        m_NodeState = NodeStates.SUCCESS;
        return m_NodeState;
    }
}
