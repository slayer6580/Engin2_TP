using UnityEngine;

public class Camp : MonoBehaviour
{
    void Start()
    {
        TeamOrchestrator._Instance.Camps.Add(this);
    }

    private void OnDestroy()
    {
        TeamOrchestrator._Instance.Camps.Remove(this);
    }
}
