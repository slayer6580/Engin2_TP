using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class TeamOrchestrator_Team : MonoBehaviour
{
    const int SPECIAL_SCORE = 10;
    const int MAX_SPAWNABLE_WORKERS = 35;

    public List<Worker_Team> WorkersList { get; private set; } = new List<Worker_Team>();

    [SerializeField] private TextMeshProUGUI m_scoreText;
    [SerializeField] private TextMeshProUGUI m_remainingTimeText;
    [SerializeField] private TextMeshProUGUI m_numberOfWorkersText;
    [SerializeField] private float m_timeScale;
    [SerializeField] private GameObject m_workersPrefab;


    [HideInInspector] public float m_remainingTime;
    [HideInInspector] public bool m_newWorkerIsNecessary = true;
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
        float mapDimensionScale = (float)mapDimension / 600; //600 = max

        int timeLeft = MapGenerator.SimulationDuration.GetValue();
        float simulationDurationScale = (float)timeLeft / 1000; //1000 = max

        int numberOfRessourcePossibleInZoneLenght = mapDimension / (int)distancePredicted;
        int numberOfRessourcePossible = (int)Mathf.Pow(numberOfRessourcePossibleInZoneLenght, 2);

        int nbsOfWorkers;

		int tripTime = (int)Mathf.Floor(((distancePredicted / Collecting_Manager._Instance.WORKER_SPEED_BY_SECOND) * 2) + 2);
		if (tripTime < 5) tripTime = 5;


		float timeForAWorkerToPayForHimself = tripTime *20;
		float timeForAWorkerToPayForPartOfCamp = (tripTime * MapGenerator.CampCost.GetValue())/4;   //Assuming there will be 4 worker on 1 camp
		

		if (m_remainingTime > (timeForAWorkerToPayForPartOfCamp + timeForAWorkerToPayForHimself)) // new
        {
            nbsOfWorkers = numberOfRessourcePossible;
            m_newWorkerIsNecessary = true;

			if (nbsOfWorkers > MAX_SPAWNABLE_WORKERS)
            {
                nbsOfWorkers = MAX_SPAWNABLE_WORKERS;
            }
        }
        else
        {
            SetClosestWorkerToRessource();
			m_newWorkerIsNecessary = false;
			nbsOfWorkers = 0;
		}

       
        // Spawn le nombre d'explorateur selon la formule du haut
        Exploring_Manager._Instance.m_nbOfExploringWorkers += nbsOfWorkers;

		for (int i = 0; i < nbsOfWorkers; i++)
        {
            GameObject newWorker = Instantiate(m_workersPrefab, new Vector2(0, 0), transform.rotation);
            OnWorkerCreated();
            newWorker.transform.SetParent(transform);
        }
    }

    public void SetWorkerToCollecting(Worker_Team worker)
    {
        worker.m_workerState = EWorkerState.collecting;
    }

    public void SetClosestWorkerToRessource()
    {
        foreach(Collectible_Team ressource in Collecting_Manager._Instance.KnownCollectibles)
        {

			if(ressource.m_designedWorker == null)
            {
				float distance = float.PositiveInfinity;
                Worker_Team closestWorker = null;
                foreach(Worker_Team worker in WorkersList)
                {
                    if(worker.m_workerState == EWorkerState.exploring)
                    {
						float tempDistance = Vector2.Distance(ressource.transform.position, worker.transform.position);
						
						if (tempDistance < distance)
						{
							closestWorker = worker;
							distance = tempDistance;
						}
					}				
				}

                if(closestWorker != null)
                {
                    closestWorker.m_reservedCollectible = ressource;
                    closestWorker.m_campPosition = Vector2.zero;
                    ressource.m_designedWorker = closestWorker;
                    ressource.m_associatedCamp = Vector2.zero;
					SetWorkerToCollecting(closestWorker);
				}
			}
		}
    }
}
