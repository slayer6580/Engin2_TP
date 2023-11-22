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
		Worker_Explorer_Tommy agentScript = m_agentTransform.Value.gameObject.GetComponent<Worker_Explorer_Tommy>();
        agentScript.FindNextChunk();

	}

    public override NodeResult Execute()
    {
		Worker_Explorer_Tommy agentScript = m_agentTransform.Value.gameObject.GetComponent<Worker_Explorer_Tommy>();
		if (agentScript.IsPathListFull)
        {
            return NodeResult.failure;
        }
           

        
       // Debug.Log("On Tommy_GenerateExplorerPath execute");
        return NodeResult.success;
    }
}
