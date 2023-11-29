using MBT;
using UnityEngine;

[MBTNode("Alex Leaf/Set camp")]
[AddComponentMenu("")]
public class SetCamp : Leaf
{
    public GameObjectReference m_workerGO = new GameObjectReference();

    public override void OnEnter()
    {
        Constructing_Manager._Instance.SetCamp(m_workerGO.Value.GetComponent<Worker_Alex>().m_reservedCollectible);
    }


    public override NodeResult Execute()
    {
        return NodeResult.success;
    }
}