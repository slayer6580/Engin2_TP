using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionRange : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var collectible = other.GetComponent<Collectible>();
        if (collectible == null) 
        {
            return;
        }

        if (Workers.s_knownCollectibles.Contains(collectible))
        {
            return;
        }

        Workers.s_knownCollectibles.Add(collectible);
    }
}
