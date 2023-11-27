using UnityEngine;

public class Camp_Alex : MonoBehaviour
{
    void Start()
    {
        Constructing_Manager._Instance.Camps.Add(this);
    }

    private void OnDestroy()
    {
        Constructing_Manager._Instance.Camps.Remove(this);
    }
}
