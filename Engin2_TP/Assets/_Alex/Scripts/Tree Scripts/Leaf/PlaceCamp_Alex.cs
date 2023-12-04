using MBT;
using UnityEngine;

[MBTNode("Alex Leaf/Place Camp")]
[AddComponentMenu("")]
public class PlaceCamp_Alex : Leaf
{
    [SerializeField] private GameObject m_campPrefab;
    public GameObjectReference m_workerGO = new GameObjectReference();

    public override NodeResult Execute()
    {
        Transform parentManager = Constructing_Manager._Instance.transform;
        GameObject camp = Instantiate(m_campPrefab, transform.position, Quaternion.identity);
        camp.transform.SetParent(parentManager);
        TeamOrchestrator_Team._Instance.OnCampPlaced();
        return NodeResult.success;
    }
}
