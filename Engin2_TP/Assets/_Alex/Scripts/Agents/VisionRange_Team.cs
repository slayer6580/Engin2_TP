using UnityEngine;

public class VisionRange_Team : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Collectible_Team collectible = other.GetComponent<Collectible_Team>();
        if (collectible == null )
        {
            return;
        }

        Collecting_Manager._Instance.TryAddCollectible(collectible);
        
    }
}
