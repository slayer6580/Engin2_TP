using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Alex Condition/Look for available ressource?")]

public class LookIfRessourceAvailable : Condition
{
    public override bool Check()
    {
        bool emptyCollectible = false;
        bool workerWithNoCollectible = false;

        foreach (Collectible_Alex collectible in Collecting_Manager._Instance.KnownCollectibles)
        {
            if (collectible.m_designedWorker == null) 
            {
                emptyCollectible = true;
                break;
            }
        }

        foreach (Worker_Alex worker in TeamOrchestrator_Alex._Instance.WorkersList) 
        { 
            if (worker.m_reservedCollectible == null && worker.m_workerState != EWorkerState.exploring)
            {
                workerWithNoCollectible = true;
                break;

            }
        }

        return emptyCollectible && workerWithNoCollectible;
    }
}
