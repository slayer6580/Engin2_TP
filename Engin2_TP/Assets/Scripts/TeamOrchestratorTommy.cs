using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class TeamOrchestratorTommy : MonoBehaviour
{
    const int SPECIAL_SCORE = 10;
    private const float MIN_OBJECTS_DISTANCE = 2.0f;
    public List<Collectible> KnownCollectibles { get; private set; } = new List<Collectible>();
    public List<Camp> Camps { get; private set; } = new List<Camp>();
    public List<Worker_Explorer_Tommy> WorkersList { get; private set; } = new List<Worker_Explorer_Tommy>();

    [SerializeField]
    private TextMeshProUGUI m_scoreText;
    [SerializeField]
    private TextMeshProUGUI m_remainingTimeText;

    private float m_remainingTime;
    private int m_score = 0;


	
    public bool[,] m_chunkList;        //NEW
    public int nbOfChunckInLine;        //NEW
    public float newTimeScale;        //NEW


	public static TeamOrchestratorTommy _Instance
    {
        get;
        private set;
    }
    private void CreateChunkList()      //NEW
    {
		float mapDimension = MapGenerator.MapDimension.Value;
		nbOfChunckInLine = (int)Mathf.Floor(mapDimension / 5.0f);

		m_chunkList = new bool[nbOfChunckInLine, nbOfChunckInLine];
		
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
       
    }

    private void Update()
    {
        m_remainingTime -= Time.deltaTime;
        m_remainingTimeText.text = "Remaining time: " + m_remainingTime.ToString("#.00");
    }

    public void TryAddCollectible(Collectible collectible)
    {
        if (KnownCollectibles.Contains(collectible))
        {
            return;
        }

        KnownCollectibles.Add(collectible);
        Debug.Log("Collectible added");
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
}
