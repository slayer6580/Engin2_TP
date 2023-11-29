using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Constructing_Manager : MonoBehaviour
{
    public List<Camp_Alex> Camps { get; private set; } = new List<Camp_Alex>();


    private const float MIN_OBJECTS_DISTANCE = 2.0f;

    public static Constructing_Manager _Instance
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

    public bool CanPlaceObject(Vector2 coordinates)
    {
        foreach (var collectible in Collecting_Manager._Instance.KnownCollectibles)
        {
            var collectibleLocation = new Vector2(collectible.transform.position.x, collectible.transform.position.y);
            if (Vector2.Distance(coordinates, collectibleLocation) < MIN_OBJECTS_DISTANCE)
            {
                return false;
            }
        }

        foreach (var camp in Camps)
        {
            var collectibleLocation = new Vector2(camp.transform.position.x, camp.transform.position.y);
            if (Vector2.Distance(coordinates, collectibleLocation) < MIN_OBJECTS_DISTANCE)
            {
                return false;
            }
        }

        return true;
    }


    public void SetCamp(Collectible_Alex reservedCollectible)
    {

        if (!Collecting_Manager._Instance.m_predictionDistanceDone)
        {
            return;
        }

        Vector2 reservedCollectiblePosition = reservedCollectible.transform.position;


        foreach (Collectible_Alex collectible in Collecting_Manager._Instance.KnownCollectibles)
        {

            if (collectible == reservedCollectible) 
            {
                continue;
            }

            if (collectible.m_reservedCamp == true)
            {
                continue;
            }

            Vector2 colPos = collectible.transform.position;
            float distance = Vector2.Distance(colPos, reservedCollectiblePosition);            

            if (distance <= (Collecting_Manager._Instance.m_predictionDistance + 1))
            {
                Vector2 campPosition = (colPos + reservedCollectiblePosition) / 2;
                reservedCollectible.m_reservedCamp = true;
                collectible.m_reservedCamp = true;
                reservedCollectible.m_reservedCampPosition = campPosition;
                collectible.m_reservedCampPosition = campPosition;
                SetWorkerToConstructCamp(campPosition);
                return;
            }         
        }

    }

    private void SetWorkerToConstructCamp(Vector2 campPosition)
    {
        float distance = 1000;
        Worker_Alex designedWorker = null;
        foreach (Worker_Alex worker in TeamOrchestrator_Alex._Instance.WorkersList) 
        {
            if (worker.m_workerState == EWorkerState.collecting) 
            { 
                Vector2 workerPos = worker.transform.position;
                if (Vector2.Distance(workerPos, campPosition) <  distance)
                { 
                    designedWorker = worker;
                }
            }
        }
        if (designedWorker)
        {
            designedWorker.m_workerState = EWorkerState.constructing;
            designedWorker.SetNextCampPosition(campPosition);
        }
     
    }
}
