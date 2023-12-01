using Unity.VisualScripting;
using UnityEngine;

public class Collectible_Alex : MonoBehaviour
{
    private const float COOLDOWN = 5.0f;
    private float m_currentCooldown = 0.0f;
    [HideInInspector] public Worker_Alex m_designedWorker = null;
    public bool m_hasBeenPickedInTheLastFiveSeconds = false;
    public Vector2 m_associatedCamp = Vector2.positiveInfinity;
    
    public ECollectibleType Extract()
    {
        if (m_currentCooldown < 0.0f)
        {
           // Debug.Log("Collectible extracted. Last extraction was: " + (COOLDOWN - m_currentCooldown).ToString() + " seconds ago");
            m_currentCooldown = COOLDOWN;
            m_hasBeenPickedInTheLastFiveSeconds = true;
            return ECollectibleType.Regular;
        }

        //We have been extracted twice under 5 seconds
        Collecting_Manager._Instance.KnownCollectibles.Remove(this);
        m_designedWorker.m_reservedCollectible = null;
        Destroy(gameObject);
        return ECollectibleType.Special;
    }

    private void FixedUpdate()
    {
        m_currentCooldown -= Time.fixedDeltaTime;

        if (m_currentCooldown <= 0 && m_hasBeenPickedInTheLastFiveSeconds)
        {
            m_hasBeenPickedInTheLastFiveSeconds = false;
        }
    }

    public void SetWorkerToCollectible(Worker_Alex worker)
    {
        if (m_designedWorker == null)
        {
            m_designedWorker = worker;
        } 
    }
}

public enum ECollectibleType_Alex
{
    Regular,
    Special,
    None
}