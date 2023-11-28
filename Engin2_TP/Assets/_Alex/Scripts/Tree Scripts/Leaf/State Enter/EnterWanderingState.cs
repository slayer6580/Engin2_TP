using MBT;
using UnityEngine;

[MBTNode("Alex State Enter/Enter Wandering State")]
[AddComponentMenu("")]
public class EnterWanderingState : Leaf
{
    public GameObjectReference m_workerGO = new GameObjectReference();

    public override NodeResult Execute()
    {
        m_workerGO.Value.GetComponent<Worker_Alex>().m_workerState = EWorkerState.wandering;
        return NodeResult.success;
        
    }
}