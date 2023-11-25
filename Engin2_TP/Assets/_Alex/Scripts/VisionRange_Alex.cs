using UnityEngine;

public class VisionRange_Alex : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var collectible = other.GetComponent<Collectible>();
        if (collectible == null )
        {
            return;
        }

        TeamOrchestrator_Alex._Instance.TryAddCollectible(collectible);
    }
}
