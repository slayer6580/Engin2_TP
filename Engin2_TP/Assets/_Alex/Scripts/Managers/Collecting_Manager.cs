using System.Collections.Generic;
using UnityEngine;

public class Collecting_Manager : MonoBehaviour
{
    public List<Collectible_Team> KnownCollectibles { get; private set; } = new List<Collectible_Team>();
    public List<Collectible_Team> m_ressourceToUse = new List<Collectible_Team>();
    public List<Collectible_Team> m_alreadyUsedRessources = new List<Collectible_Team>();
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

    public void TryAddCollectible(Collectible_Team collectible)
    {
        if (KnownCollectibles.Contains(collectible))
        {
            return;
        }

        KnownCollectibles.Add(collectible);

        if (KnownCollectibles.Count == 2)
        {
            PredictRessourceDistance();
        }

        Debug.Log("Collectible added");
    }

    public void RemoveCollectible (Collectible_Team collectible)
    {
        KnownCollectibles.Remove(collectible);
        
    }

    private void PredictRessourceDistance()
    {
        Collectible_Team firstCollectible = KnownCollectibles[0];
        Collectible_Team secondCollectible = KnownCollectibles[1];

        float distance = Vector2.Distance(firstCollectible.transform.position, secondCollectible.transform.position);
        m_predictionDistanceDone = true;
        m_predictionDistance = distance;

        TeamOrchestrator_Team._Instance.SpawnExplorerBasedOnPredictionDistance(m_predictionDistance);
    }  

}
