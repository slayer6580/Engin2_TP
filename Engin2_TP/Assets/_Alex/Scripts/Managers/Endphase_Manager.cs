using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Endphase_Manager : MonoBehaviour
{
    public static Endphase_Manager _Instance
    {
        get;
        private set;
    }

    // Start is called before the first frame update
    private void Awake()
    {
        if (_Instance == null || _Instance == this)
        {
            _Instance = this;
            return;
        }
        Destroy(this);
    }
}
