using UnityEngine;

public class VisionRange_Alex : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Collectible_Alex collectible = other.GetComponent<Collectible_Alex>();
        if (collectible == null )
        {
            return;
        }

        Collecting_Manager._Instance.TryAddCollectible(collectible);
        
    }
}
