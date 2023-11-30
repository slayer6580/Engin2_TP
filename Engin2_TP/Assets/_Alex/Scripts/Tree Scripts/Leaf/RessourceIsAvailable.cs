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
            // si un worker n'a pas de ressource
            if (worker.m_reservedCollectible == null && worker.m_workerState == EWorkerState.collecting)
            {
                // regarder dans mes ressources
                foreach (Collectible_Alex collectible in Collecting_Manager._Instance.KnownCollectibles)
                {
                    float distanceBetweenRessourceAndWorker = Vector2.Distance(worker.transform.position, collectible.transform.position);

                    // prendre la ressource la plus proche du worker
                    if (collectible.m_designedWorker == null && distanceBetweenRessourceAndWorker < minDistance)
                    {
                        minDistance = distanceBetweenRessourceAndWorker;
                        closestCollectible = collectible;
                                        
                    }
                }

                // si une ressource disponible la plus proche es trouvé, donner cette ressource au worker et réservé des deux bords
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
