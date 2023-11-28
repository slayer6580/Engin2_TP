using MBT;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/Can Go Collect")]
public class Tommy_CanGoCollect : Condition
{
	[SerializeField]
	private TransformReference m_agentTransform = new TransformReference();
	public Vector2Reference targetPosition;


	private Collectible assignedRessource;

	public override bool Check()
    {


		Transform obj = m_agentTransform.Value;
		Vector2 target = targetPosition.Value;
		assignedRessource = m_agentTransform.Value.gameObject.GetComponent<Tommy_Worker>().assignedRessource;

		float mySpeed = Tommy_TeamOrchestrator._Instance.m_workerSpeedUnitBySecond;
		float dist = Vector2.Distance(target, obj.position);
		float timeToReachTarget = dist / mySpeed;



		if (Time.time - Tommy_TeamOrchestrator._Instance.KnownCollectiblesTimers[assignedRessource] + timeToReachTarget > 4.1f)
		{
			return true;
		}

		return false;

	}
}
