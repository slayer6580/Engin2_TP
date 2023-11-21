using UnityEngine;

public class VisionRange : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var collectible = other.GetComponent<Collectible>();
        if (collectible == null )
        {
            return;
        }

        TeamOrchestrator._Instance.TryAddCollectible(collectible);
    }
}
