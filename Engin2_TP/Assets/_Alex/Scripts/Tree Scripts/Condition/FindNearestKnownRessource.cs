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
		Collectible_Team chosenRessource = null;

		foreach (Collectible_Team ressource in Collecting_Manager._Instance.KnownCollectibles)
		{
			if(ressource.GetComponent<Collectible_Team>().m_endGameAssociatedWorkerList.Count >= 2)
			{
				continue;
			}

			float tempDistance = Vector2.Distance(ressource.transform.position, m_workerGO.Value.transform.position);
			// trouver le camp le plus proche
			if (tempDistance < minDistance)
			{
				m_workerGO.Value.gameObject.GetComponent<Worker_Team>().m_reservedCollectible = ressource;
				m_nearestRessource.Value = ressource.transform.position;
				minDistance = tempDistance;
				suitableRessourceExist = true;
				chosenRessource = ressource;
			}
		}

		if (suitableRessourceExist)
		{
			chosenRessource.GetComponent<Collectible_Team>().m_endGameAssociatedWorkerList.Add(m_workerGO.Value.GetComponent<Worker_Team>());
			return NodeResult.success;
		}

		return NodeResult.failure;
	}
}
