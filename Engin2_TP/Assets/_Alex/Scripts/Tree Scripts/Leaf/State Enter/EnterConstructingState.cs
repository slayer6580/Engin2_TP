using MBT;
using UnityEngine;

[MBTNode("Alex State Enter/Enter Constructing State")]
[AddComponentMenu("")]
public class EnterConstructingState : Leaf
{
    public GameObjectReference m_workerGO = new GameObjectReference();

    public override NodeResult Execute()
    {
        m_workerGO.Value.GetComponent<Worker_Alex>().m_workerState = EWorkerState.constructing;
        return NodeResult.success;

    }
}