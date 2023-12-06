using UnityEngine;

public class Worker_Team : MonoBehaviour
{
    private const float EXTRACTION_DURATION = 1.0f;
    private const float DEPOSIT_DURATION = 1.0f;
    private const float RADIUS = 5.0f;

    [SerializeField] private Transform m_radiusDebugTransform;

    private bool m_isInDepot = false;
    private bool m_isInExtraction = false;
    private float m_currentActionDuration = 0.0f;
    private Collectible_Team m_currentExtractingCollectible;

    private Color32 m_yellowColor = new Color32(255, 255, 0, 255); 
    private Color32 m_blueColor = new Color32(0, 0, 255, 255);  
    private Color32 m_redColor = new Color32(255, 0, 0, 255);  

    [HideInInspector] public Collecting_Manager collecting_manager;
    [HideInInspector] public bool m_extraExplorator = false;
    [HideInInspector] public EWorkerState m_workerState = EWorkerState.none;
    [HideInInspector] public Collectible_Team m_reservedCollectible = null;    
    [HideInInspector] public ECollectibleType m_collectibleInInventory = ECollectibleType.None;    
    [HideInInspector] public EDirection m_workerDirection = EDirection.left;
    [HideInInspector] public Vector2 m_campPosition = Vector2.positiveInfinity;

    private bool m_isCollectingAndEmptyHands => m_collectibleInInventory == ECollectibleType.None && m_workerState != EWorkerState.exploring;


    private void OnValidate()
    {
        m_radiusDebugTransform.localScale = new Vector3(RADIUS, RADIUS, RADIUS);
    }

    private void Start()
    {
        TeamOrchestrator_Team._Instance.WorkersList.Add(this);
        collecting_manager = Collecting_Manager._Instance;
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

        Collectible_Team collectible = collision.GetComponent<Collectible_Team>();
        if (collectible != null && m_isCollectingAndEmptyHands) 
        {
            m_currentExtractingCollectible = collectible;
            m_currentActionDuration = EXTRACTION_DURATION;
            m_isInExtraction = true;
        }
      
        Camp_Team camp = collision.GetComponent<Camp_Team>();
        if (camp != null && m_collectibleInInventory != ECollectibleType.None)
        {
            m_currentActionDuration = DEPOSIT_DURATION;
            m_isInDepot = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Collectible_Team collectible = collision.GetComponent<Collectible_Team>();
        if (collectible != null && m_collectibleInInventory == ECollectibleType.None) 
        {
            if (m_currentExtractingCollectible == collectible)
            {
                m_currentExtractingCollectible = null;
            }
            m_currentActionDuration = EXTRACTION_DURATION;
            m_isInExtraction = false;
        }

        Camp_Team camp = collision.GetComponent<Camp_Team>();
        if (camp != null && m_collectibleInInventory != ECollectibleType.None)
        {
            m_isInDepot = false;
            m_currentActionDuration = DEPOSIT_DURATION;
        }
    }

    private void GainCollectible()
    {
        
        if (m_currentExtractingCollectible == null)
        {
            return;
        }
        

        m_collectibleInInventory = m_currentExtractingCollectible.Extract(m_workerState);
        
        if (m_collectibleInInventory == ECollectibleType.None)
        {
            return;
        }

        m_isInExtraction = false;
        m_currentExtractingCollectible = null;
        GetComponent<SpriteRenderer>().color = m_blueColor;

		if (m_collectibleInInventory == ECollectibleType.Special)
		{			
			GetComponent<SpriteRenderer>().color = m_redColor;
            collecting_manager.RemoveCollectible(m_reservedCollectible);
            m_reservedCollectible = null; 
        }

    }

    private void DepositResource()
    {
        TeamOrchestrator_Team._Instance.GainResource(m_collectibleInInventory);
        m_collectibleInInventory = ECollectibleType.None;
        m_isInDepot = false;
        GetComponent<SpriteRenderer>().color = m_yellowColor;
    }

    private void SetWorkerState()
    {
        Exploring_Manager._Instance.SetWorkerForExploring(this);
    }
}

public enum EWorkerState
{
    exploring,
    collecting,
    constructing,
    endPhase,
    none
}

public enum EDirection
{
    left,
    right,
    up,
    down,
}