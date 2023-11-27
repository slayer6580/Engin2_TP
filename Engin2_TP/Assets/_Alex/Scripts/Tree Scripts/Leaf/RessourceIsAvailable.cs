using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Alex Leaf/Set ressource to closest worker")]

public class RessourceAvailable : Leaf
{

    public override NodeResult Execute()
    {
        foreach (Collectible_Alex collectible in Collecting_Manager._Instance.KnownCollectibles)
        {
            if (collectible.m_designedWorker == null)
            {
                float minDistance = 1000; // test
                Worker_Alex closestWorker = null;
                foreach (Worker_Alex worker in TeamOrchestrator_Alex._Instance.WorkersList)
                {
                    if (worker.m_reservedCollectible == null &&
                        (worker.m_workerState == EWorkerState.wandering || worker.m_workerState == EWorkerState.collecting))
                    {
                        if (Vector2.Distance(worker.transform.position, collectible.transform.position) < minDistance)
                        {
                            minDistance = Vector2.Distance(worker.transform.position, collectible.transform.position);
                            closestWorker = worker;
                        }
                    }
                }

                collectible.m_designedWorker = closestWorker;
                closestWorker.m_reservedCollectible = collectible;
                closestWorker.m_workerState = EWorkerState.collecting;
                return NodeResult.success;
            }
        }

        return NodeResult.success;
    }
}
