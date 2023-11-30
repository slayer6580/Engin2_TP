using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class TeamOrchestrator_Alex : MonoBehaviour
{
    const int SPECIAL_SCORE = 10;

    public List<Worker_Alex> WorkersList { get; private set; } = new List<Worker_Alex>();

    [SerializeField] private TextMeshProUGUI m_scoreText;
    [SerializeField] private TextMeshProUGUI m_remainingTimeText;
    [SerializeField] private float m_timeScale;
    [SerializeField] private GameObject m_workersPrefab;

    private float m_remainingTime;
    private int m_score = 0;
    private bool m_workersAlreadySpawnBasedOnPrediction = false;

    private const int STARTING_WORKER = 5;

    public static TeamOrchestrator_Alex _Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        SpawnStartingWorkers();

        if (_Instance == null || _Instance == this)
        {
            _Instance = this;
            return;
        }
        Destroy(this);
    }

    private void Start()
    {
        m_remainingTime = MapGenerator.SimulationDuration.Value;
    }

    private void Update()
    {
        Time.timeScale = m_timeScale;

        m_remainingTime -= Time.deltaTime;
        m_remainingTimeText.text = "Remaining time: " + m_remainingTime.ToString("#.00");

        CheckIfGameEnd();
    }

    private void CheckIfGameEnd()
    {
        if (MapGenerator.SimulationDuration.Value < Time.timeSinceLevelLoad)
        {
            OnGameEnded();
        }
    }

    public void GainResource(ECollectibleType collectibleType)
    {
        if (collectibleType == ECollectibleType.Regular)
        {
            m_score++;
        }
        if (collectibleType == ECollectibleType.Special)
        {
            m_score += SPECIAL_SCORE;
        }

        Debug.Log("New score = " + m_score);
        m_scoreText.text = "Score: " + m_score.ToString();
    }

    public void OnGameEnded()
    {
        PrintTextFile();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    private void PrintTextFile()
    {
        string path = Application.persistentDataPath + "/Results.txt";
        File.AppendAllText(path, "Score of simulation with seed: " + MapGenerator.Seed + ": " + m_score.ToString() + "\n");

#if UNITY_EDITOR
        UnityEditor.EditorUtility.RevealInFinder(path);
        UnityEditor.EditorUtility.OpenWithDefaultApp(path);
#endif
    }

    public void OnCampPlaced()
    {
        m_score -= MapGenerator.CampCost.Value;
    }

    public void OnWorkerCreated()
    {
        //TODO élèves. À vous de trouver quand utiliser cette méthode et l'utiliser.
        m_score -= MapGenerator.WORKER_COST;
    }

    private void SpawnStartingWorkers()
    {
        for (int i = 0; i < STARTING_WORKER; i++)
        {
            Transform worker = Instantiate(m_workersPrefab, new Vector2(0, 0), transform.rotation).transform;
            worker.parent = transform;
        }
    }

    public void SpawnExplorerBasedOnPredictionDistance(float distancePredicted)
    {
        if (m_workersAlreadySpawnBasedOnPrediction)
        {
            return;
        }
        m_workersAlreadySpawnBasedOnPrediction = true;

        int mapDimension = MapGenerator.MapDimension.Value;
        int numberOfRessourcePossibleInZoneLenght = mapDimension / (int)distancePredicted;
        int nbOfNewExplorator = (numberOfRessourcePossibleInZoneLenght * 2) - STARTING_WORKER;

        Exploring_Manager._Instance.m_nbOfExploringWorkers += nbOfNewExplorator;
        for (int i = 0; i < nbOfNewExplorator; i++)
        {
            GameObject newWorker = Instantiate(m_workersPrefab, new Vector2(0, 0), transform.rotation);
            OnWorkerCreated();
            newWorker.transform.SetParent(transform);
        }
    }

    public void SpawnCollectingWorker(int numberToSpawn)
    {
        List<Collectible_Alex> m_unreservedCollectible = new List<Collectible_Alex>();

        foreach (Collectible_Alex collectible in Collecting_Manager._Instance.KnownCollectibles)
        {
            if (collectible.m_designedWorker == null)
            {
                m_unreservedCollectible.Add(collectible);
            }
        }

        for (int i = 0; i < numberToSpawn; i++)
        {
            GameObject newWorker = Instantiate(m_workersPrefab, new Vector2(0, 0), transform.rotation);
            OnWorkerCreated();
            newWorker.transform.SetParent(transform);
            newWorker.GetComponent<Worker_Alex>().m_reservedCollectible = m_unreservedCollectible[i];
            int index = Collecting_Manager._Instance.KnownCollectibles.IndexOf(m_unreservedCollectible[i]);
            Collecting_Manager._Instance.KnownCollectibles[index].m_designedWorker = newWorker.GetComponent<Worker_Alex>();


        }

    }

}
