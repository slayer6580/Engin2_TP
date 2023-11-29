using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wandering_Manager : MonoBehaviour
{

    public static Wandering_Manager _Instance
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

    public Vector2 FindAnRandomExploredZone(Vector2 workerPos)
    {
        int nbOfZOne = Exploring_Manager._Instance.m_zonesPositions.Count;
        int randomZone = Random.Range(0, nbOfZOne - 1);

        while (Exploring_Manager._Instance.m_zonesIsDetected[randomZone] == true)
        {
            randomZone = Random.Range(0, nbOfZOne - 1);
        }
      
        return Exploring_Manager._Instance.m_zonesPositions[randomZone];
    }

}
