using MBT;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;


[AddComponentMenu("")]
[MBTNode(name = "Alex Condition/Find Nearest Known Ressource")]
public class FindNearestKnownRessource : Leaf
{
	public GameObjectReference m_workerGO = new GameObjectReference();

	public Vector2Reference m_nearestRessource = new Vector2Reference();

	public override NodeResult Execute()
	{

		float minDistance = float.PositiveInfinity;
		bool suitableRessourceExist = false;


		foreach (Collectible_Team ressource in Collecting_Manager._Instance.KnownCollectibles)
		{
			float tempDistance = Vector2.Distance(ressource.transform.position, m_workerGO.Value.transform.position);
			// trouver le camp le plus proche
			if (tempDistance < minDistance)
			{
				m_workerGO.Value.gameObject.GetComponent<Worker_Team>().m_reservedCollectible = ressource;
				m_nearestRessource.Value = ressource.transform.position;
				minDistance = tempDistance;
				suitableRessourceExist = true;
			}
		}

		if (suitableRessourceExist)
		{
			return NodeResult.success;
		}

		return NodeResult.failure;
	}
}
