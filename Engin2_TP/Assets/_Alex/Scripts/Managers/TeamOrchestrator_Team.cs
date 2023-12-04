using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class TeamOrchestrator_Team : MonoBehaviour
{
    const int SPECIAL_SCORE = 10;
    const int MAX_WORKERS = 35;

    public List<Worker_Team> WorkersList { get; private set; } = new List<Worker_Team>();

    [SerializeField] private TextMeshProUGUI m_scoreText;
    [SerializeField] private TextMeshProUGUI m_remainingTimeText;
    [SerializeField] private TextMeshProUGUI m_numberOfWorkersText;
    [SerializeField] private float m_timeScale;
    [SerializeField] private GameObject m_workersPrefab;

    private float m_remainingTime;
    private int m_score = 0;
    private bool m_workersAlreadySpawnBasedOnPrediction = false;

    private const int STARTING_WORKER = 5;

    public static TeamOrchestrator_Team _Instance
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
        m_numberOfWorkersText.text = "Number of workers: " + WorkersList.Count;

        CheckIfGameEnd();
    }


    public float ShareTimeLeft()
    {
        return m_remainingTime;
    }
	// Code a Max
	private void CheckIfGameEnd()
    {
        if (MapGenerator.SimulationDuration.Value < Time.timeSinceLevelLoad)
        {
            OnGameEnded();
        }
    }

    // Code a Max
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

       // Debug.Log("New score = " + m_score);
        m_scoreText.text = "Score: " + m_score.ToString();
    }

    // Code a Max
    public void OnGameEnded()
    {
        PrintTextFile();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    // Code a Max
    private void PrintTextFile()
    {
        string path = Application.persistentDataPath + "/Results.txt";
        File.AppendAllText(path, "Score of simulation with seed: " + MapGenerator.Seed + ": " + m_score.ToString() + "\n");

#if UNITY_EDITOR
        UnityEditor.EditorUtility.RevealInFinder(path);
        UnityEditor.EditorUtility.OpenWithDefaultApp(path);
#endif
    }

    // Code a Max
    public void OnCampPlaced()
    {
        m_score -= MapGenerator.CampCost.Value;
    }

    // Code a Max
    public void OnWorkerCreated()
    {
        //TODO élèves. À vous de trouver quand utiliser cette méthode et l'utiliser.
        m_score -= MapGenerator.WORKER_COST;
    }

    // Fonction de départ qui spawn mes 5 workers principal.
    private void SpawnStartingWorkers()
    {
        for (int i = 0; i < STARTING_WORKER; i++)
        {
            Transform worker = Instantiate(m_workersPrefab, new Vector2(0, 0), transform.rotation).transform;
            worker.parent = transform;
        }
    }

    // Fonction qui spawn des explorator selon une prédiction de collectible
    public void SpawnExplorerBasedOnPredictionDistance(float distancePredicted)
    {
        if (m_workersAlreadySpawnBasedOnPrediction)
        {
            return;
        }
        m_workersAlreadySpawnBasedOnPrediction = true;

        //TODO améliorer la formule pour résultat plus efficace (avec temps restant aussi)
        int mapDimension = MapGenerator.MapDimension.Value;
        float mapDimensionScale = mapDimension / 600; //600 = max

        int timeLeft = MapGenerator.SimulationDuration.GetValue();
        float simulationDurationScale = timeLeft / 1000; //1000 = max

        int numberOfRessourcePossibleInZoneLenght = mapDimension / (int)distancePredicted;
        int numberOfRessourcePossible = (int)Mathf.Pow(numberOfRessourcePossibleInZoneLenght, 2);

        float nbsOfWorkers = ((MAX_WORKERS * mapDimensionScale) + (MAX_WORKERS * simulationDurationScale)) / 2;

        //Pas la facon la plus optimal donc feel free de changer
        nbsOfWorkers = Mathf.Clamp(nbsOfWorkers, 0, numberOfRessourcePossible);

        int nbOfNewExplorator = (int)Mathf.Round(Mathf.Clamp(nbsOfWorkers, 0, MAX_WORKERS));
        //Debug.LogWarning(numberOfRessourcePossible);

        // Spawn le nombre d'explorateur selon la formule du haut
        Exploring_Manager._Instance.m_nbOfExploringWorkers += nbOfNewExplorator;

        // TODO spawn un lot de 4 workers et moins a la fois sur une petite durée
        for (int i = 0; i < nbOfNewExplorator; i++)
        {
            GameObject newWorker = Instantiate(m_workersPrefab, new Vector2(0, 0), transform.rotation);
            OnWorkerCreated();
            newWorker.transform.SetParent(transform);
        }
    }

    // Fonction qui spawn des collector manquant a la fin de l'exploration
    public void SpawnCollectingWorker(int numberToSpawn)
    {

        for (int i = 0; i < numberToSpawn; i++)
        {
            // spawner un collecteur
            GameObject newWorker = Instantiate(m_workersPrefab, new Vector2(0, 0), transform.rotation);
            OnWorkerCreated();
            newWorker.transform.SetParent(transform);

           
        }

    }

}
