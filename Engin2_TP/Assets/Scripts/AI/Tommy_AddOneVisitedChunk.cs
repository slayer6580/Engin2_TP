using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/Add One Visited Chunk")]
public class Tommy_AddOneVisitedChunk : Leaf
{


	public override void OnEnter()
	{
		Tommy_TeamOrchestrator._Instance.m_visitedChunk++;
	}

	public override NodeResult Execute()
	{
		//Debug.Log("On AddOneVisitedChunk execute");
		return NodeResult.success;
	}
}
