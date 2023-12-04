using MBT;
using UnityEngine;

[MBTNode("Alex State Enter/Enter Collecting State")]
[AddComponentMenu("")]
public class EnterCollectingState : Leaf
{
    public GameObjectReference m_workerGO = new GameObjectReference();

    public override NodeResult Execute()
    {
        m_workerGO.Value.GetComponent<Worker_Team>().m_workerState = EWorkerState.collecting;
        return NodeResult.success;

    }
}