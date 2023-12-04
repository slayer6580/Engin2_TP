using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;  //To restart scene


public class Tommy_TeamOrchestrator : MonoBehaviour
{
    /*
    const int CHUNK_SIZE = 5;
    const int SPECIAL_SCORE = 10;
    const int ACTION_TIME_ON_RESSOURCE = 1;
    private const float MIN_OBJECTS_DISTANCE = 2.0f;
    public List<Collectible> KnownCollectibles { get; private set; } = new List<Collectible>();
    public List<Camp> Camps { get; private set; } = new List<Camp>();
    public List<Tommy_Worker> WorkersList { get; private set; } = new List<Tommy_Worker>();

    [SerializeField]
    private TextMeshProUGUI m_scoreText;
    [SerializeField]
    private TextMeshProUGUI m_remainingTimeText;

    private float m_remainingTime;
    private int m_score = 0;


	
    public bool[,] m_chunkList;        //NEW
    private int m_totalChunk;
    public int m_visitedChunk;
    public int nbOfChunckInLine;        //NEW
    public float newTimeScale;        //NEW
    public float m_minimumDistanceBetweenRessources = 1000;    //NEW
    public int m_averageNumberOfRessourcesByDistance = 0;
    public int m_averageNumberOfRessourcesByPercentage = 0;
    public int m_mixedResultAverageRessources = 0;

    public float m_workerSpeedUnitBySecond = 0;
    public float m_workerSpeedByChunk;

    public List<Collectible> m_ressourceToUse = new List<Collectible>();
    public List<Vector2> m_campToSpawn = new List<Vector2>();
    public List<Collectible> m_alreadyUsedRessources = new List<Collectible>();
	public float WorkerSpeedByChunk
	{
		get
		{
            return m_workerSpeedByChunk;
		}

		set
		{
            m_workerSpeedUnitBySecond = CHUNK_SIZE / value; 
			m_workerSpeedByChunk = value; // Note the use of the implicit variable "value" here.
		}
	}

	public float percentageVisited = 0;
    private float m_timeForOneCollect;
	public Dictionary<Collectible, float> KnownCollectiblesTimers = new Dictionary<Collectible, float>();


	

	public static Tommy_TeamOrchestrator _Instance
    {
        get;
        private set;
    }
    private void CreateChunkList()      //NEW
    {
		float mapDimension = MapGenerator.MapDimension.Value;
		nbOfChunckInLine = (int)Mathf.Ceil(mapDimension / 5.0f);

		m_chunkList = new bool[nbOfChunckInLine, nbOfChunckInLine];
        m_totalChunk = nbOfChunckInLine * nbOfChunckInLine;
	}

    public void UpdateMinimumDistanceBetweenRessources()
    {
        float minDistance = 100;
        for(int i=0;i< KnownCollectibles.Count; i++)
        {
			for (int j = i+1; j < KnownCollectibles.Count; j++)
			{
                float currentDistance = Vector2.Distance(KnownCollectibles[i].transform.position, KnownCollectibles[j].transform.position);
                if(currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                }
			}
		}

        m_minimumDistanceBetweenRessources = minDistance;

	}

    public void CalculatePossibleAmountOfRessources()
    {

        float minDistanceAdjustment = 0;
        
		if (KnownCollectibles.Count < 12)
        {
            minDistanceAdjustment = -3 + (KnownCollectibles.Count /4);
        }
        else
        {
            minDistanceAdjustment = 0;
		}
     
        float lineAverage = MapGenerator.MapDimension.Value / (m_minimumDistanceBetweenRessources + minDistanceAdjustment);
        float divider = m_minimumDistanceBetweenRessources * 6;
		m_averageNumberOfRessourcesByDistance = (int)Mathf.Pow((lineAverage - (MapGenerator.MapDimension.Value / divider)), 2);
        
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

    private void Start()
    {
        Time.timeScale = newTimeScale;
		m_remainingTime = MapGenerator.SimulationDuration.Value;
        CreateChunkList(); //NEW
        m_minimumDistanceBetweenRessources = 100;   //NEW


	}

    private void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space))
        {
            print("TEST");
			SceneManager.LoadScene("SampleScene_Tommy");
		}
      
		//Average time for one collect
		if (KnownCollectibles.Count > 1)
        {
			m_timeForOneCollect = (m_minimumDistanceBetweenRessources / CHUNK_SIZE) * WorkerSpeedByChunk + ACTION_TIME_ON_RESSOURCE;
		}
       

		m_remainingTime -= Time.deltaTime;
        m_remainingTimeText.text = "Remaining time: " + m_remainingTime.ToString("#.00");

        
        //Average count of how many possible ressource there is in the map
		if (m_visitedChunk > 0)
		{
			percentageVisited = ((float)m_visitedChunk / (float)m_totalChunk) * 100;
			m_averageNumberOfRessourcesByPercentage = (int)(KnownCollectibles.Count * 100 / percentageVisited);
		}
        m_mixedResultAverageRessources = (m_averageNumberOfRessourcesByPercentage + m_averageNumberOfRessourcesByDistance) / 2;

        
      
	}

    public void TryAddCollectible(Collectible collectible)
    {
        if (KnownCollectibles.Contains(collectible))
        {
            return;
        }
        
		KnownCollectibles.Add(collectible);
        KnownCollectiblesTimers.Add(collectible, 0);
		UpdateMinimumDistanceBetweenRessources();   //NEW
		CalculatePossibleAmountOfRessources();      //NEW
													//Debug.Log("Collectible added");
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

    public List<Collectible> FindRessourcePackWithMiddlePoint()
    {
        Tommy_FindPackList knownRessourceManager = new Tommy_FindPackList();

		

		return knownRessourceManager.FindBestPackOfRessources(KnownCollectibles, m_ressourceToUse, m_alreadyUsedRessources, m_campToSpawn, m_workerSpeedUnitBySecond, m_minimumDistanceBetweenRessources, m_remainingTime);
		
    
	}

    public int CheckPossiblePointAtThatPosition(List<Collectible> ressourcePack, Vector2 positionToCheck)
    {
        int possiblePoints = 0;

        foreach(Collectible collectible in ressourcePack)
        {
			float distanceFromRessource = Vector2.Distance(positionToCheck, collectible.transform.position);
			float travelTime = distanceFromRessource / m_workerSpeedUnitBySecond;
			float timeToCollect = travelTime + 2;
			if (timeToCollect < 5)
			{
				timeToCollect = 5;
			}

			possiblePoints += Mathf.FloorToInt(m_remainingTime / timeToCollect);
		}


       // print("Possible points with " + ressourcePack.Count + " ressources. = " + possiblePoints);
		return possiblePoints;
    }


	public Vector2 GetCenterOfPack(List<Collectible> pack)
    {
		Vector3 center = Vector3.zero;

		foreach (Collectible collectible in pack)
		{
			center += collectible.transform.position;
		}
		center /= pack.Count;

        return new Vector2(center.x, center.y);
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
        File.AppendAllText(path, "Score of simulation with seed: " + MapGenerator.Seed +  ": " + m_score.ToString() + "\n");

#if UNITY_EDITOR
        UnityEditor.EditorUtility.RevealInFinder(path);
        UnityEditor.EditorUtility.OpenWithDefaultApp(path);
#endif
    }

    public bool CanPlaceObject(Vector2 coordinates)
    {
        foreach (var collectible in KnownCollectibles)
        {
            var collectibleLocation = new Vector2(collectible.transform.position.x, collectible.transform.position.y);
            if (Vector2.Distance(coordinates, collectibleLocation) < MIN_OBJECTS_DISTANCE)
            {
                return false;
            }
        }

        return true;
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
    */
}
