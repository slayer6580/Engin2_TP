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
        Instantiate(m_campPrefab, transform.position, Quaternion.identity);
        TeamOrchestrator_Alex._Instance.OnCampPlaced();
        return NodeResult.success;
    }
}
