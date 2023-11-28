using MBT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[MBTNode("Engin2/Generate Explorer Path")]
[AddComponentMenu("")]
public class Tommy_GenerateExplorerPath : Leaf
{
    public TransformReference m_agentTransform = new TransformReference();

    public override void OnEnter()
    {
		Tommy_Worker agentScript = m_agentTransform.Value.gameObject.GetComponent<Tommy_Worker>();
        agentScript.FindNextChunk();

	}

    public override NodeResult Execute()
    {
		Tommy_Worker agentScript = m_agentTransform.Value.gameObject.GetComponent<Tommy_Worker>();
		if (agentScript.IsPathListFull)
        {
            return NodeResult.failure;
        }
           

        
       // Debug.Log("On Tommy_GenerateExplorerPath execute");
        return NodeResult.success;
    }
}
