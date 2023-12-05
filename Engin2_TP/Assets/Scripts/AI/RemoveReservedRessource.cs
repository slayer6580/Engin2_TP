using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/Remove Reserved Ressource")]
public class RemoveReservedRessource : Leaf
{
	public GameObjectReference m_workerGO = new GameObjectReference();

	public override NodeResult Execute()
	{
		m_workerGO.Value.gameObject.GetComponent<Worker_Team>().m_reservedCollectible = null;

		return NodeResult.success;
	}
}
