using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Alex State Condition/Worker is collecting?")]
public class IsCollecting : Condition
{
    public GameObjectReference m_workerGO = new GameObjectReference();
    public override bool Check()
    {
        return m_workerGO.Value.GetComponent<Worker_Team>().m_workerState == EWorkerState.collecting;
    }
}
