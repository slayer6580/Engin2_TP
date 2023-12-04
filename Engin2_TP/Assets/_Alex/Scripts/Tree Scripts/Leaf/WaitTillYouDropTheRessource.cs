using MBT;
using UnityEngine;

[MBTNode("Alex Leaf/Wait till you drop the ressource")]
[AddComponentMenu("")]
public class WaitTillYouDropRessource : Leaf
{
    public GameObjectReference m_workerGO = new GameObjectReference();
    private Worker_Team m_worker;
    private bool m_workerHaveNoRessource = false;

    public override void OnEnter()
    {
        m_worker = m_workerGO.Value.GetComponent<Worker_Team>();
    }

    public override NodeResult Execute()
    {
        m_workerHaveNoRessource = (m_worker.m_collectibleInInventory == ECollectibleType.None);

        if (m_workerHaveNoRessource)
        {
            return NodeResult.success;           
        }
        else
        {
            return NodeResult.running;
        }
    }
}