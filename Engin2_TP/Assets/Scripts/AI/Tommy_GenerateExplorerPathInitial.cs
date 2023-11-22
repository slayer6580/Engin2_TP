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
		Worker_Explorer_Tommy agentScript = m_agentTransform.Value.gameObject.GetComponent<Worker_Explorer_Tommy>();
        agentScript.FindInitialChunk();

	}

    public override NodeResult Execute()
    {
		Worker_Explorer_Tommy agentScript = m_agentTransform.Value.gameObject.GetComponent<Worker_Explorer_Tommy>();
		if (agentScript.m_workerPath.Count == 0)
        {
            return NodeResult.failure;
        }



		// Debug.Log("On Tommy_GenerateExplorerPathInitial execute");
		return NodeResult.success;
    }
}
