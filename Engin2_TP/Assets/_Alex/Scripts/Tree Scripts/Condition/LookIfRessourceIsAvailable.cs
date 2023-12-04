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

        foreach (Collectible_Team collectible in Collecting_Manager._Instance.KnownCollectibles)
        {
            if (collectible.m_designedWorker == null) 
            {
                emptyCollectible = true;
                break;
            }
        }

        foreach (Worker_Team worker in TeamOrchestrator_Team._Instance.WorkersList) 
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
