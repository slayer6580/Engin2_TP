using System.Collections.Generic;
using UnityEngine;

public class Collecting_Manager : MonoBehaviour
{
    public List<Collectible_Alex> KnownCollectibles { get; private set; } = new List<Collectible_Alex>();
    public List<Collectible_Alex> m_ressourceToUse = new List<Collectible_Alex>();
    public List<Collectible_Alex> m_alreadyUsedRessources = new List<Collectible_Alex>();
    public List<Vector2> m_campList = new List<Vector2>();
    public float WORKER_SPEED_BY_SECOND = 4.8076f;

   [HideInInspector] public bool m_predictionDistanceDone = false;
   [HideInInspector] public float m_predictionDistance;
    
    public static Collecting_Manager _Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (_Instance == null || _Instance == this)
        {
            _Instance = this;
            return;
        }
        Destroy(this);
    }

    public void TryAddCollectible(Collectible_Alex collectible)
    {
        if (KnownCollectibles.Contains(collectible))
        {
            return;
        }

        KnownCollectibles.Add(collectible);

        if (KnownCollectibles.Count >= 2 && !m_predictionDistanceDone)
        {
            PredictRessourceDistance();
        }

        Debug.Log("Collectible added");
    }

    // Fonction qui prédit une distance des deux premiers collectible trouvé
    private void PredictRessourceDistance()
    {      

        foreach (Collectible_Alex collectible in KnownCollectibles)
        {
            foreach (Collectible_Alex collectible2 in KnownCollectibles)
            {
                if (collectible == collectible2) 
                {
                    continue;
                }

                float distance = Vector2.Distance(collectible.transform.position, collectible2.transform.position);
               
                    m_predictionDistanceDone = true;
                    m_predictionDistance = distance;
                
            }
        }

        TeamOrchestrator_Alex._Instance.SpawnExplorerBasedOnPredictionDistance(m_predictionDistance);

    }  

}
