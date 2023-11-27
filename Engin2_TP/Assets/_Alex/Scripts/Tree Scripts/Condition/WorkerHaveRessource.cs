using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Alex Condition/Worker have a ressource?")]

public class WorkerHaveRessource : Condition
{
    public GameObjectReference m_workerGO = new GameObjectReference();
    public override bool Check()
    {
        return m_workerGO.Value.GetComponent<Worker_Alex>().m_collectibleInInventory != ECollectibleType.None;
    }
}
