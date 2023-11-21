using UnityEngine;

public class Collectible : MonoBehaviour
{
    private const float COOLDOWN = 5.0f;
    private float m_currentCooldown = 0.0f;

    public ECollectibleType Extract()
    {
        if (m_currentCooldown < 0.0f)
        {
            Debug.Log("Collectible extracted. Last extraction was: " + (COOLDOWN - m_currentCooldown).ToString() + " seconds ago");
            m_currentCooldown = COOLDOWN;
            return ECollectibleType.Regular;
        }

        //We have been extracted twice under 5 seconds
        TeamOrchestrator._Instance.KnownCollectibles.Remove(this);
        Destroy(gameObject);
        return ECollectibleType.Special;
    }

    private void FixedUpdate()
    {
        m_currentCooldown -= Time.fixedDeltaTime;
    }
}

public enum ECollectibleType
{
    Regular,
    Special,
    None
}