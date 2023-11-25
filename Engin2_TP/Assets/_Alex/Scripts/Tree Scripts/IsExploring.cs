using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Alex/Worker is exploring")]
public class IsExploring : Condition
{
    public GameObjectReference m_workerGO = new GameObjectReference();
    public override bool Check()
    {
        return m_workerGO.Value.GetComponent<Worker_Alex>().m_workerState == Worker_Alex.EWorkerState.exploring;
    }
}
