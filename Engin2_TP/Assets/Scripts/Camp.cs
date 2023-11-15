using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp : MonoBehaviour
{
    void Start()
    {
        Workers.s_Camps.Add(this);
    }

    private void OnDestroy()
    {
        Workers.s_Camps.Remove(this);
    }

}
