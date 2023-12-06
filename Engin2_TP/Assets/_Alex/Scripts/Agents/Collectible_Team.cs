using System.Collections.Generic;
using UnityEngine;

public class Collectible_Team : MonoBehaviour
{
    private const float COOLDOWN = 5.0f;
    private float m_currentCooldown = 0.0f;
    [HideInInspector] public Worker_Team m_designedWorker = null;
    [HideInInspector] public bool m_hasBeenPickedInTheLastFiveSeconds = false;
    [HideInInspector] public Vector2 m_associatedCamp = Vector2.positiveInfinity;
    [HideInInspector] public List<Worker_Team> m_endGameAssociatedWorkerList = new List<Worker_Team>();

	public ECollectibleType Extract(EWorkerState state)
    {
        if (m_currentCooldown < 0.0f)
        {
            m_currentCooldown = COOLDOWN;
            m_hasBeenPickedInTheLastFiveSeconds = true;
            return ECollectibleType.Regular;
        }

        if (state != EWorkerState.endPhase)
        {
            return ECollectibleType.None;
        }

        Collecting_Manager._Instance.KnownCollectibles.Remove(this);
        
        Destroy(gameObject);
        return ECollectibleType.Special;
    }

    private void FixedUpdate()
    {
        m_currentCooldown -= Time.fixedDeltaTime;

        if (m_currentCooldown <= 0.9f && m_hasBeenPickedInTheLastFiveSeconds)
        {
            m_hasBeenPickedInTheLastFiveSeconds = false;
        }
    }

    public void SetWorkerToCollectible(Worker_Team worker)
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