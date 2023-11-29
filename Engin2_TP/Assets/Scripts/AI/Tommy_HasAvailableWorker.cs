using MBT;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/Has Available Worker")]
public class Tommy_HasAvailableWorker : Condition
{

	public override bool Check()
    {
		bool atLeastOne = false;
		foreach(Tommy_Worker worker in Tommy_TeamOrchestrator._Instance.WorkersList)
		{
			if(worker.assignedRessource == null)
			{
				atLeastOne = true;
			}
		}
		return atLeastOne;


	}
}
