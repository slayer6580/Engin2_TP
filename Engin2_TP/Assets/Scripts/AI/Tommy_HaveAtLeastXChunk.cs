using MBT;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/Have at least x chunk")]
public class Tommy_HaveAtLeastXChunk : Condition
{
	[SerializeField]
	private TransformReference m_agentTransform = new TransformReference();
	[SerializeField]
	private int m_minChunkToHave;
	public override bool Check()
    {
		Tommy_Worker agentScript = m_agentTransform.Value.gameObject.GetComponent<Tommy_Worker>();
		return agentScript.m_workerPath.Count >= m_minChunkToHave;
    }
}
