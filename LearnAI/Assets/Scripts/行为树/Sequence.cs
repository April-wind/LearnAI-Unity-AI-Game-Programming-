using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Nodes
{
    private List<Nodes> m_nodes = new List<Nodes>();

    public Sequence(List<Nodes> nodes)
    {
        m_nodes = nodes;
    }

    public override NodeStates Evaluate()
    {
        bool anyChildRunning = false;

        //所有都为success为success
        foreach (var VARIABLE in m_nodes)
        {
            switch (VARIABLE.Evaluate())
            {
                case NodeStates.FAILURE:
                    m_NodeState = NodeStates.FAILURE;
                    return m_NodeState;
                case NodeStates.SUCCESS:
                    continue;
                case NodeStates.RUNNINT:
                    anyChildRunning = true;
                    continue;
                default:
                    m_NodeState = NodeStates.SUCCESS;
                    return m_NodeState;
            }
        }

        m_NodeState = anyChildRunning ? NodeStates.RUNNINT : NodeStates.SUCCESS;
        return m_NodeState;
    }
}
