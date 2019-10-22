using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Nodes
{
   /*该选择器的子节点*/
   protected List<Nodes> m_nodes = new List<Nodes>();

   public Selector(List<Nodes> nodes)
   {
      m_nodes = nodes;
   }

   public override NodeStates Evaluate()
   {
      //只要有一个Success即为success
      foreach (var VARIABLE in m_nodes)
      {
         switch (VARIABLE.Evaluate())
         {
            case NodeStates.FAILURE:
               continue;
            case NodeStates.SUCCESS:
               m_NodeState = NodeStates.SUCCESS;
               return m_NodeState;
            case NodeStates.RUNNINT:
               m_NodeState = NodeStates.RUNNINT;
               return m_NodeState;
            default:
               continue;
         }
      }
      m_NodeState = NodeStates.FAILURE;
      return m_NodeState;
   }
}
