using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Nodes
{
    public delegate NodeStates NodeReturn();

    /*当前节点*/
    protected NodeStates m_NodeState;

    public NodeStates nodeState
    {
        get { return m_NodeState; }
    }
    
    public Nodes(){}
    /// <summary>
    /// 用于确定节点状态
    /// </summary>
    /// <returns></returns>
    public abstract NodeStates Evaluate();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum NodeStates
{
    FAILURE,
    SUCCESS,
    RUNNINT
}
