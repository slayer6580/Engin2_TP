using System.Collections.Generic;
using UnityEngine;

public class Collecting_Manager : MonoBehaviour
{
    public List<Collectible_Alex> KnownCollectibles { get; private set; } = new List<Collectible_Alex>();

    
    public static Collecting_Manager _Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (_Instance == null || _Instance == this)
        {
            _Instance = this;
            return;
        }
        Destroy(this);
    }

    public void TryAddCollectible(Collectible_Alex collectible)
    {
        if (KnownCollectibles.Contains(collectible))
        {
            return;
        }

        KnownCollectibles.Add(collectible);
        Debug.Log("Collectible added");
    }

   

}
