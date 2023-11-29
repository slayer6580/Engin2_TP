using UnityEngine;

public class Worker_Alex : MonoBehaviour
{
    private const float EXTRACTION_DURATION = 1.0f;
    private const float DEPOSIT_DURATION = 1.0f;

    [SerializeField] private float m_radius = 5.0f;
    [SerializeField] private Transform m_radiusDebugTransform;
  
    private bool m_isInDepot = false;
    private bool m_isInExtraction = false;
    private float m_currentActionDuration = 0.0f;
    private Collectible_Alex m_currentExtractingCollectible;
    private Color32 m_noRessourceColor = new Color32(255, 255, 0, 255); // yellow
    private Color32 m_ressourceColor = new Color32(0, 0, 255, 255);  // bleu
    [HideInInspector] public bool m_extraExplorator = false;
    private Vector2 m_campPositionPlacement = Vector2.zero;

    private bool m_isCollectingAndEmptyHands => m_collectibleInInventory == ECollectibleType.None && m_workerState == EWorkerState.collecting;

    [HideInInspector] public Collectible_Alex m_reservedCollectible = null;
    [HideInInspector] public ECollectibleType m_collectibleInInventory = ECollectibleType.None;

    // Pour exploration initiale
     public EWorkerState m_workerState = EWorkerState.none;
    [HideInInspector] public EDirection m_workerDirection = EDirection.left;
  

    private void OnValidate()
    {
        m_radiusDebugTransform.localScale = new Vector3(m_radius, m_radius, m_radius);
    }

    private void Start()
    {
        TeamOrchestrator_Alex._Instance.WorkersList.Add(this);
        SetWorkerState();      
    }

    private void FixedUpdate()
    {
        if (m_isInDepot || m_isInExtraction)
        {
            m_currentActionDuration -= Time.fixedDeltaTime;
            if (m_currentActionDuration < 0.0f)
            {
                if (m_isInDepot)
                {
                    DepositResource();
                }
                else
                {
                    GainCollectible();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Collectible_Alex collectible = collision.GetComponent<Collectible_Alex>();
        if (collectible != null && m_isCollectingAndEmptyHands)
        {
            m_currentExtractingCollectible = collectible;
            m_currentActionDuration = EXTRACTION_DURATION;
            m_isInExtraction = true;
            //Start countdown to collect it
        }

        Camp_Alex camp = collision.GetComponent<Camp_Alex>();
        if (camp != null && m_collectibleInInventory != ECollectibleType.None)
        {
            m_currentActionDuration = DEPOSIT_DURATION;
            m_isInDepot = true;
            //Start countdown to deposit my current collectible (if it exists)
        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Collectible_Alex collectible = collision.GetComponent<Collectible_Alex>();
        if (collectible != null && m_collectibleInInventory == ECollectibleType.None)
        {
            if (m_currentExtractingCollectible == collectible)
            {
                m_currentExtractingCollectible = null;
            }
            m_currentActionDuration = EXTRACTION_DURATION;
            m_isInExtraction = false;
        }

        Camp_Alex camp = collision.GetComponent<Camp_Alex>();
        if (camp != null && m_collectibleInInventory != ECollectibleType.None)
        {
            m_isInDepot = false;
            m_currentActionDuration = DEPOSIT_DURATION;
        }
    }

    public void SetNextCampPosition(Vector2 campPosition)
    {
        m_campPositionPlacement = campPosition;
    }

    public Vector2 GetCampPosition()
    {
        return m_campPositionPlacement;
    }
    private void GainCollectible()
    {
        m_collectibleInInventory = m_currentExtractingCollectible.Extract();

        if (m_collectibleInInventory == ECollectibleType.Special)
        {
            m_reservedCollectible = null;
        }

        m_isInExtraction = false;
        m_currentExtractingCollectible = null;
        GetComponent<SpriteRenderer>().color = m_ressourceColor;
    }

    private void DepositResource()
    {
        TeamOrchestrator_Alex._Instance.GainResource(m_collectibleInInventory);
        m_collectibleInInventory = ECollectibleType.None;
        m_isInDepot = false;
        GetComponent<SpriteRenderer>().color = m_noRessourceColor;
    }

    private void SetWorkerState()
    {
        if (Exploring_Manager._Instance.m_exploringWorkers.Count < Exploring_Manager._Instance.m_nbOfExploringWorkers)
        {
            Exploring_Manager._Instance.SetWorkerForExploring(this);
        }
        else
        {
            m_workerState = EWorkerState.collecting;
        }
    }
}

public enum EWorkerState
{
    // State de mes workers
    exploring,
    collecting,
    constructing,
    endPhase,
    none
}

public enum EDirection
{
    // Pour exploration initiale
    left,
    right,
    up,
    down,


}