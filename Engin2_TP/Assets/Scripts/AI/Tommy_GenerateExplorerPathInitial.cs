using MBT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[MBTNode("Engin2/Generate Explorer Initial Path")]
[AddComponentMenu("")]
public class Tommy_GenerateExplorerPathInitial : Leaf
{
    public TransformReference m_agentTransform = new TransformReference();

    public override void OnEnter()
    {
		Tommy_Worker agentScript = m_agentTransform.Value.gameObject.GetComponent<Tommy_Worker>();
        agentScript.FindInitialChunk();

	}

    public override NodeResult Execute()
    {
		Tommy_Worker agentScript = m_agentTransform.Value.gameObject.GetComponent<Tommy_Worker>();
		if (agentScript.m_workerPath.Count == 0)
        {
            return NodeResult.failure;
        }



		// Debug.Log("On Tommy_GenerateExplorerPathInitial execute");
		return NodeResult.success;
    }
}
