using System.Collections.Generic;
using UnityEngine;

public class Exploring_Manager : MonoBehaviour
{
    public int m_nbOfExploringWorkers;
    public int m_zoneLenght = 0;

    [SerializeField] private GameObject m_zone;

    private int m_mapDimension = 0;
    private int m_workerInExploration = 0;
    private float m_pourcentageOfTotalTimeExploration;

    private List<float> m_xPositions = new List<float>();
    private List<float> m_yPositions = new List<float>();

    [HideInInspector] public List<Vector2> m_zonesPositions = new List<Vector2>();
    [HideInInspector] public List<bool> m_zonesIsDetected = new List<bool>();
    [HideInInspector] public List<Worker_Alex> m_exploringWorkers = new List<Worker_Alex>();
    [HideInInspector] public bool m_explorationIsDone = false;

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
            CheckIfExploratorAreDoneExploring();
        }
    }

    void Start()
    {
      
        GetMapDimensionAndZoneLength();
        EvaluateWhenStopExploring();
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

    /// <summary> Cette fonction sert a placer les positions des zones dans une liste </summary>
    private void SetZonePositionsForList()
    {
        int mapDimension = m_mapDimension;

        // Round to 5
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

    /// <summary> Cette fonction initialise les listes de positions et la liste de bool </summary>
    private void ListAllZones(int i, int j)
    {
        Vector2 zonePosition = new Vector2(m_xPositions[j], m_yPositions[i]);
        m_zonesPositions.Add(zonePosition);
        m_zonesIsDetected.Add(false);
    }

    /// <summary> Cette fonction sert a assigner des workers comme explorateur </summary>
    public void SetWorkerForExploring(Worker_Alex worker)
    {

        if (m_workerInExploration >= m_nbOfExploringWorkers)
        {
            worker.m_workerState = EWorkerState.collecting;
            return;
        }
        else if (m_explorationIsDone) 
        {
            worker.m_workerState = EWorkerState.collecting;
            return;
        }

        if (m_workerInExploration > 4)
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
        m_exploringWorkers.Add(worker);
        m_workerInExploration++;
    }

    /// <summary> Cette fonction sert a placer les zones détectés </summary>
    public void SpawnDetectedZone(Vector2 position)
    {
        GameObject zone = Instantiate(m_zone, position, transform.rotation);
        zone.transform.SetParent(transform);
    }

    private void EvaluateWhenStopExploring()
    {      
        int minDuration = MapGenerator.SimulationDuration.GetMin();
        int maxDuration = MapGenerator.SimulationDuration.GetMax();
        int mapDuration = MapGenerator.SimulationDuration.Value;

        // retourne le pourcentage sur le temps d'une partie en comparaison a son min et max
        m_pourcentageOfTotalTimeExploration = Mathf.InverseLerp(minDuration, maxDuration, mapDuration) * 100;

        float totalTime = MapGenerator.SimulationDuration.Value;
        float explorationTime = (totalTime / 100) * m_pourcentageOfTotalTimeExploration;

        Invoke("AllWorkerStopExploring", explorationTime);
    }

    private void AllWorkerStopExploring()
    {
        m_explorationIsDone = true;

        foreach (Worker_Alex worker in TeamOrchestrator_Alex._Instance.WorkersList)
        {
            if (worker.m_workerState == EWorkerState.exploring)
            {
                worker.m_workerState = EWorkerState.collecting;
                m_exploringWorkers.Remove(worker);
            }
        }

        int numberOfCollectorToSpawn = Collecting_Manager._Instance.KnownCollectibles.Count - TeamOrchestrator_Alex._Instance.WorkersList.Count;
        for (int i = 0; i < numberOfCollectorToSpawn; i++)
        {
            TeamOrchestrator_Alex._Instance.SpawnCollectingWorker();
        }

    }

    public void TryRemoveWorkerFromExploring(Worker_Alex worker)
    {
        if (m_exploringWorkers.Contains(worker)) 
        {
            m_exploringWorkers.Remove(worker);
        }
    }

    public float GetPourcentageOfMapExpored()
    {
        float pourcentage = 100;
        int numberOfZoneExplored = 0;

        foreach (bool zone in m_zonesIsDetected)
        {
            if (zone == true)
            {
                numberOfZoneExplored++;
            }
        }

        pourcentage /= m_zonesIsDetected.Count;
        pourcentage *= numberOfZoneExplored;
        return pourcentage;
    }

    private void CheckIfExploratorAreDoneExploring()
    {
      
        foreach (Worker_Alex worker in TeamOrchestrator_Alex._Instance.WorkersList)
        {
            if (worker.m_workerState == EWorkerState.exploring)
            {
                return;
            }
        }

        AllWorkerStopExploring();
    }
}
