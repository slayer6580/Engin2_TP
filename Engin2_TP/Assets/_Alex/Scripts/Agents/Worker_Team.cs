using UnityEngine;

public class Worker_Team : MonoBehaviour
{
    private const float EXTRACTION_DURATION = 1.0f;
    private const float DEPOSIT_DURATION = 1.0f;

    [SerializeField] private float m_radius = 5.0f;
    [SerializeField] private Transform m_radiusDebugTransform;

    private bool m_isInDepot = false;
    private bool m_isInExtraction = false;
    private float m_currentActionDuration = 0.0f;
    private Collectible_Team m_currentExtractingCollectible;

    private Color32 m_noRessourceColor = new Color32(255, 255, 0, 255); // yellow
    private Color32 m_ressourceColor = new Color32(0, 0, 255, 255);  // bleu
    private Color32 m_specialRessourceColor = new Color32(255, 0, 0, 255);  // red

    public Collecting_Manager collecting_manager;
    [HideInInspector] public bool m_extraExplorator = false;

    private bool m_isCollectingAndEmptyHands => m_collectibleInInventory == ECollectibleType.None && m_workerState == EWorkerState.collecting ;

    [HideInInspector] public Collectible_Team m_reservedCollectible = null;
    /*[HideInInspector]*/ public ECollectibleType m_collectibleInInventory = ECollectibleType.None;     //ReadOnly

    // Pour exploration initiale
    public EWorkerState m_workerState = EWorkerState.none;
    [HideInInspector] public EDirection m_workerDirection = EDirection.left;

    [SerializeField]
    public bool hasCollectibleReserverd;        //ReadOnly

    public Vector2 m_campPosition = Vector2.positiveInfinity;

	private void OnValidate()
    {
        m_radiusDebugTransform.localScale = new Vector3(m_radius, m_radius, m_radius);
    }

    private void Start()
    {
        TeamOrchestrator_Team._Instance.WorkersList.Add(this);
        collecting_manager = Collecting_Manager._Instance;
        SetWorkerState();
    }

    private void FixedUpdate()
    {
		hasCollectibleReserverd = m_reservedCollectible != null;

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
            //Start countdown to collect it
        }

        if (m_workerState == EWorkerState.endPhase)
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
            //Start countdown to deposit my current collectible (if it exists)
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

    //Code a max
    private void GainCollectible()
    {
       
        m_collectibleInInventory = m_currentExtractingCollectible.Extract(m_workerState);
        
        // si jamais peut pas extraire
        if (m_collectibleInInventory == ECollectibleType.None)
        {
            return;
        }

        m_isInExtraction = false;
        m_currentExtractingCollectible = null;
        GetComponent<SpriteRenderer>().color = m_ressourceColor;

		if (m_collectibleInInventory == ECollectibleType.Special)
		{
			
			GetComponent<SpriteRenderer>().color = m_specialRessourceColor;
            collecting_manager.RemoveCollectible(m_reservedCollectible); // le fait deja dans collectible mais somehow ca marche pas!
            m_reservedCollectible = null; // to be sure
        }
	}

    //Code a max
    private void DepositResource()
    {
        TeamOrchestrator_Team._Instance.GainResource(m_collectibleInInventory);
        m_collectibleInInventory = ECollectibleType.None;
        m_isInDepot = false;
        GetComponent<SpriteRenderer>().color = m_noRessourceColor;
    }

    // Fonction qui va décider du role de mes workers
    private void SetWorkerState()
    {
        Exploring_Manager._Instance.SetWorkerForExploring(this);
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