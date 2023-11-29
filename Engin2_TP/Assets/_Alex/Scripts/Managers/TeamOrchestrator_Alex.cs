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
    [SerializeField] private int nbOfWorkerToSpawn;
    [SerializeField] private int m_nbOfNewWorker;

    private float m_remainingTime;
    private int m_score = 0;
    private bool m_workerAlreadySpawnBasedOnPrediction = false;

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
        for (int i = 0; i < nbOfWorkerToSpawn; i++)
        {
            Transform worker = Instantiate(m_workersPrefab, new Vector2(0, 0), transform.rotation).transform;
            worker.parent = transform;
        }
    }

    public void SpawnWorkerBasedOnPredictionDistance(float distancePredicted)
    {
        if (m_workerAlreadySpawnBasedOnPrediction)
        {
            return;
        }

        m_workerAlreadySpawnBasedOnPrediction = true;

        //TODO logique pour savoir combien de worker je doit faire selon map et temps restant
        // 5 == Logique temporaire pour test

        Exploring_Manager._Instance.m_nbOfExploringWorkers += m_nbOfNewWorker;
        for (int i = 0; i < m_nbOfNewWorker; i++)
        {
            GameObject newWorker = Instantiate(m_workersPrefab, new Vector2(0, 0), transform.rotation);
            OnWorkerCreated();
            newWorker.transform.SetParent(transform);
        }


    }

    public void SpawnWorkerForCollecting()
    {
        GameObject newWorker = Instantiate(m_workersPrefab, new Vector2(0, 0), transform.rotation);
        OnWorkerCreated();
        newWorker.transform.SetParent(transform);
    }

}
