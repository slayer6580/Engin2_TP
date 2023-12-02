using MBT;
using UnityEngine;


[AddComponentMenu("")]
[MBTNode(name = "Alex Condition/Get Nearest Camp From That Ressource")]
public class Tommy_GetNearestCampFromThatRessource : Leaf
{
    public GameObjectReference m_workerGO = new GameObjectReference();
    public Collectible_Alex workerCollectible;

    public override NodeResult Execute()
	{
		workerCollectible = m_workerGO.Value.gameObject.GetComponent<Worker_Alex>().m_reservedCollectible;

		float minDistance = float.PositiveInfinity;
		bool suitableCampExist = false;


		foreach (Vector2 camp in Collecting_Manager._Instance.m_campList)
		{
			float tempDistance = Vector2.Distance(camp, workerCollectible.transform.position);
			// trouver le camp le plus proche
			if (tempDistance < minDistance)
			{
				minDistance = tempDistance;
				//Associé la position du camp au worker ET à la ressource
				m_workerGO.Value.gameObject.GetComponent<Worker_Alex>().m_campPosition = camp;
				workerCollectible.GetComponent<Collectible_Alex>().m_associatedCamp = camp;
				suitableCampExist = true;
			}
		}

		if (suitableCampExist)
		{
			return NodeResult.success;
		}

		return NodeResult.failure;
	}

    

  
}
