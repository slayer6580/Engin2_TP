using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Alex Leaf/Set ressource to closest worker")]

public class RessourceAvailable : Leaf
{

    public override NodeResult Execute()
    {
        Collectible_Alex closestCollectible = null;

        float minDistance = 1000; // test

        foreach (Worker_Alex worker in TeamOrchestrator_Alex._Instance.WorkersList)
        {
            if (worker.m_reservedCollectible == null && worker.m_workerState == EWorkerState.collecting)
            {
                foreach (Collectible_Alex collectible in Collecting_Manager._Instance.KnownCollectibles)
                {
                    float distanceBetweenRessourceAndWorker = Vector2.Distance(worker.transform.position, collectible.transform.position);

                    if (collectible.m_designedWorker == null && distanceBetweenRessourceAndWorker < minDistance)
                    {
                        minDistance = distanceBetweenRessourceAndWorker;
                        closestCollectible = collectible;
                                        
                    }
                }

                if (closestCollectible != null)
                {
                    closestCollectible.m_designedWorker = worker;
                    worker.m_reservedCollectible = closestCollectible;
                    worker.m_workerState = EWorkerState.collecting;
                    return NodeResult.success;
                }

            }

       
        }

        
            return NodeResult.failure;
          
    }
}
