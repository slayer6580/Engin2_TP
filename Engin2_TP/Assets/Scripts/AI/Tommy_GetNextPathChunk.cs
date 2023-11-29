using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/Get next path chunk")]
public class Tommy_GetNextPathChunk : Leaf
{

    [SerializeField]
    private TransformReference m_agentTransform = new TransformReference();
	public Vector2Reference m_chunkTargetPosition2D = new Vector2Reference(VarRefMode.DisableConstant);

	public override void OnEnter()
	{
		Tommy_Worker agentScript = m_agentTransform.Value.gameObject.GetComponent<Tommy_Worker>();
		m_chunkTargetPosition2D.Value = agentScript.GetChunkPosition();
	}

	public override NodeResult Execute()
	{
		//Debug.Log("On GetNextPathChunk execute");
		return NodeResult.success;
	}
}
