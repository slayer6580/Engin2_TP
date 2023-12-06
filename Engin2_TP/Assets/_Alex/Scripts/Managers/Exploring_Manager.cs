using System.Collections.Generic;
using UnityEngine;

public class Exploring_Manager : MonoBehaviour
{

    [SerializeField] private GameObject m_zone;

    private int m_mapDimension = 0;
    private int m_workerInExploration = 0;

    private List<float> m_xPositions = new List<float>();
    private List<float> m_yPositions = new List<float>();

    [HideInInspector] public int m_nbOfExploringWorkers = 5;
    [HideInInspector] public int m_zoneLenght = 0;
    [HideInInspector] public List<Vector2> m_zonesPositions = new List<Vector2>();
    [HideInInspector] public List<bool> m_zonesIsDetected = new List<bool>();
    [HideInInspector] public bool m_explorationIsDone = false;

<<<<<<< HEAD
=======
    private const int MAX_WORKER = 40;
    //private const int RESSOURCE_TO_FOUND_TO_STOP_EXPLORING = 60;    //Bigger than max nb of worker


>>>>>>> origin/main
    public static Exploring_Manager _Instance
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

    private void Update()
    {
        if (!m_explorationIsDone)
        {
            CheckIfExploratorsAreDoneExploring();
            StopExploringWhenEnoughRessourceFound();
		}
    }

    void Start()
    {
        GetMapDimensionAndZoneLength();
        SetZonePositionsForList();

        for (int i = 0; i < m_zoneLenght; i++)
        {
            for (int j = 0; j < m_zoneLenght; j++)
            {
                ListAllZones(i, j);
            }
        }
    }


    private void GetMapDimensionAndZoneLength()
    {
        m_mapDimension = MapGenerator.MapDimension.Value;
        Debug.Log("Map dimension: " + m_mapDimension.ToString());

        m_zoneLenght = (m_mapDimension / 5) + 1;
        Debug.Log("Length of Zones: " + m_zoneLenght.ToString());
    }

    private void SetZonePositionsForList()
    {
        int mapDimension = m_mapDimension;

        while (mapDimension % 5 != 0)
        {
            mapDimension--;
        }

        float halfMapDimension = (float)mapDimension / 2;

        for (int i = 0; i < m_zoneLenght; i++)
        {
            float x = -halfMapDimension + (i * 5);
            m_xPositions.Add(x);

            float y = halfMapDimension - (i * 5);
            m_yPositions.Add(y);
        }
    }

    private void ListAllZones(int i, int j)
    {
        Vector2 zonePosition = new Vector2(m_xPositions[j], m_yPositions[i]);
        m_zonesPositions.Add(zonePosition);
        m_zonesIsDetected.Add(false);
    }


    public void SetWorkerForExploring(Worker_Team worker)
    {

        if (m_workerInExploration > m_nbOfExploringWorkers || m_explorationIsDone)
        {
            worker.m_workerState = EWorkerState.collecting;
            return;
        }
     
        if (m_workerInExploration > 3)
        {
            worker.m_extraExplorator = true;
        }

        int moduloForDirection = m_workerInExploration % 4;

        switch (moduloForDirection)
        {
            case 0:
                worker.m_workerDirection = EDirection.left;
                break;
            case 1:
                worker.m_workerDirection = EDirection.right;
                break;
            case 2:
                worker.m_workerDirection = EDirection.up;
                break;
            case 3:
                worker.m_workerDirection = EDirection.down;
                break;
            default:
                break;
        }

        worker.m_workerState = EWorkerState.exploring;
        m_workerInExploration++;
    }


    public void SpawnDetectedZone(Vector2 position)
    {
        GameObject zone = Instantiate(m_zone, position, transform.rotation);
        zone.transform.SetParent(transform);
    }

<<<<<<< HEAD
=======
    // calcule un temps d'arret de l'exploration
    private void EvaluateWhenStopExploring()
    {
        int minDuration = MapGenerator.SimulationDuration.GetMin(); // 10
        int maxDuration = MapGenerator.SimulationDuration.GetMax(); // 1000
        int mapDuration = MapGenerator.SimulationDuration.Value;

        // retourne le pourcentage sur le temps d'une partie en comparaison a son min et max
        m_pourcentageOfTotalTimeExploration = Mathf.InverseLerp(minDuration, maxDuration, mapDuration) * 100;

        float totalTime = MapGenerator.SimulationDuration.Value;
        float explorationTime = (totalTime / 100) * m_pourcentageOfTotalTimeExploration;

        // dire a mes explorateur de collecté apres un certain temps calculer plus haut
        Invoke("WorkersStopExploringAndSpawnCollectors", explorationTime);
    }

    // Arreter l'exploration et spawner les collecteur manquant
>>>>>>> origin/main
    private void WorkersStopExploringAndSpawnCollectors()
    {
        if (m_explorationIsDone)
        {
            return;
        }

        m_explorationIsDone = true;

        foreach (Worker_Team worker in TeamOrchestrator_Team._Instance.WorkersList)
        {
            if (worker.m_workerState == EWorkerState.exploring)
            {
                worker.m_workerState = EWorkerState.collecting;
            }
        }

<<<<<<< HEAD
=======
		int knownCollectibleCount = Collecting_Manager._Instance.KnownCollectibles.Count;
        int workerCount = TeamOrchestrator_Team._Instance.WorkersList.Count;
		int numberOfCollectorToSpawn = knownCollectibleCount - workerCount;

		if (workerCount >= MAX_WORKER)
        {
            return;
        }
>>>>>>> origin/main
    }  

    private void CheckIfExploratorsAreDoneExploring()
    {

        foreach (Worker_Team worker in TeamOrchestrator_Team._Instance.WorkersList)
        {
            if (worker.m_workerState == EWorkerState.exploring)
            {
                return;
            }
        }

        WorkersStopExploringAndSpawnCollectors();
    }

	private void StopExploringWhenEnoughRessourceFound()
	{
        if (Collecting_Manager._Instance.KnownCollectibles.Count >= (int)(TeamOrchestrator_Team._Instance.WorkersList.Count * 1.5f))
		{
			WorkersStopExploringAndSpawnCollectors();
		}
	}
}
