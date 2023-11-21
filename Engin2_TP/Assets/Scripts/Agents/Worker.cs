using Unity.VisualScripting;
using UnityEngine;

public class Worker : MonoBehaviour
{
    private const float EXTRACTION_DURATION = 1.0f;
    private const float DEPOSIT_DURATION = 1.0f;

    [SerializeField]
    private float m_radius = 5.0f;
    [SerializeField]
    private Transform m_radiusDebugTransform;
    [SerializeField]
    private ECollectibleType m_collectibleInInventory = ECollectibleType.None;
    [SerializeField]
    private Collectible m_currentExtractingCollectible;

    private bool m_isInDepot = false;
    private bool m_isInExtraction = false;
    private float m_currentActionDuration = 0.0f;

    private void OnValidate()
    {
        m_radiusDebugTransform.localScale = new Vector3(m_radius, m_radius, m_radius);
    }

    private void Start()
    {
        TeamOrchestrator._Instance.WorkersList.Add(this);
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
        var collectible = collision.GetComponent<Collectible>();
        if (collectible != null && m_collectibleInInventory == ECollectibleType.None)
        {
            m_currentExtractingCollectible = collectible;
            m_currentActionDuration = EXTRACTION_DURATION;
            m_isInExtraction = true;
            //Start countdown to collect it
        }

        var camp = collision.GetComponent<Camp>();
        if (camp != null && m_collectibleInInventory != ECollectibleType.None)
        {
            m_currentActionDuration = DEPOSIT_DURATION;
            m_isInDepot = true;
            //Start countdown to deposit my current collectible (if it exists)
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var collectible = collision.GetComponent<Collectible>();
        if (collectible != null && m_collectibleInInventory == ECollectibleType.None)
        {
            if (m_currentExtractingCollectible == collectible)
            {
                m_currentExtractingCollectible = null;
            }
            m_currentActionDuration = EXTRACTION_DURATION;
            m_isInExtraction = false;
        }

        var camp = collision.GetComponent<Camp>();
        if (camp != null && m_collectibleInInventory != ECollectibleType.None)
        {
            m_isInDepot = false;
            m_currentActionDuration = DEPOSIT_DURATION;
        }
    }

    private void GainCollectible()
    {
        m_collectibleInInventory = m_currentExtractingCollectible.Extract();
        m_isInExtraction = false;
        m_currentExtractingCollectible = null;
    }

    private void DepositResource()
    {
        TeamOrchestrator._Instance.GainResource(m_collectibleInInventory);
        m_collectibleInInventory = ECollectibleType.None;
        m_isInDepot = false;
    }
}