using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/Get nearest camp")]
public class GetNearestCamp : Leaf
{
	public GameObjectReference m_workerGO = new GameObjectReference();
	public Vector2Reference m_nearestCampPos = new Vector2Reference();


	public override NodeResult Execute()
	{

		float minDistance = float.PositiveInfinity;
		bool suitableCampExist = false;

		foreach (Vector2 camp in Collecting_Manager._Instance.m_campList)
		{
			float tempDistance = Vector2.Distance(camp, m_workerGO.Value.transform.position);
			if (tempDistance < minDistance)
			{
				m_nearestCampPos.Value = camp;
				minDistance = tempDistance;
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
