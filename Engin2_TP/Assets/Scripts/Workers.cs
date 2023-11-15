using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workers : MonoBehaviour
{
    public static List<Collectible> s_knownCollectibles { get; set; } = new List<Collectible>();
    public static List<Camp> s_Camps = new List<Camp>();

    [SerializeField]
    private float m_radius = 5.0f;

    [SerializeField]
    private Transform m_radiusDebugTransform;

    private void OnValidate()
    {
        m_radiusDebugTransform.localScale = new Vector3(m_radius, m_radius, m_radius); 
    }


    void Update()
    {
        
    }
}
