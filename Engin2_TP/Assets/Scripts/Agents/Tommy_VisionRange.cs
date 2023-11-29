using UnityEngine;

public class Tommy_VisionRange : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var collectible = other.GetComponent<Collectible>();
        if (collectible == null )
        {
            return;
        }

        Tommy_TeamOrchestrator._Instance.TryAddCollectible(collectible);
    }
}
