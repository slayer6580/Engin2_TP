using MBT;
using UnityEngine;

using System.Collections.Generic;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/Assign Worker")]
public class Tommy_AssignWorker : Leaf
{
	Tommy_TeamOrchestrator m_orchestrator;
	public List<Tommy_Worker> workerToAssignList = new List<Tommy_Worker>();

	public override void OnEnter()
	{
		m_orchestrator = Tommy_TeamOrchestrator._Instance;
		print("SERACH FOR WORKER");
		
		//Check for available workers
		foreach (Tommy_Worker worker in Tommy_TeamOrchestrator._Instance.WorkersList)
		{
			if (worker.assignedRessource == null)
			{
				workerToAssignList.Add(worker);		
			}
			if(workerToAssignList.Count >= m_orchestrator.m_ressourceToUse.Count)
			{
				break;
			}
		}

		int ressourceToAssign = m_orchestrator.m_ressourceToUse.Count;
	
		//TODO search for nearest worker
		for (int i = 0; i < ressourceToAssign; i++)
		{
			if (workerToAssignList.Count > 0)
			{
				if (i == 0)
				{
					workerToAssignList[0].AssignCamp(m_orchestrator.m_campToSpawn[0]);
					m_orchestrator.m_campToSpawn.RemoveAt(0);
				}
				workerToAssignList[0].AssignRessource(m_orchestrator.m_ressourceToUse[0]);
				m_orchestrator.m_ressourceToUse.RemoveAt(0);
				workerToAssignList.RemoveAt(0);
			}
			else 
			{ 
				//TODO check if it's a good idea to spawn a new worker
			}
				
		}		
		

		workerToAssignList.Clear();
	}

	public override NodeResult Execute()
	{
		//Debug.Log("On AddOneVisitedChunk execute");
		return NodeResult.success;
	}
}
