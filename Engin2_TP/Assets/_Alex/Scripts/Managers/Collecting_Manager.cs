using System.Collections.Generic;
using UnityEngine;

public class Collecting_Manager : MonoBehaviour
{
    public List<Collectible_Alex> KnownCollectibles { get; private set; } = new List<Collectible_Alex>();
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

        if (KnownCollectibles.Count >= 2)
        {
            PredictRessourceDistance();
        }

        Debug.Log("Collectible added");
    }

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

                if (!m_predictionDistanceDone)
                {
                    m_predictionDistanceDone = true;
                    m_predictionDistance = distance;
                }
                else
                {
                    if (distance < m_predictionDistance)
                    {
                        m_predictionDistance = distance;
                    }
                }

            }
        }

        TeamOrchestrator_Alex._Instance.SpawnExplorerBasedOnPredictionDistance(m_predictionDistance);

    }

   

}
