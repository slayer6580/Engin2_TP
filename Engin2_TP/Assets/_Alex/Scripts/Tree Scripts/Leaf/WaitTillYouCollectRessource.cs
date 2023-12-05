using MBT;
using UnityEngine;

[MBTNode("Alex Leaf/Wait till you collect the ressource")]
[AddComponentMenu("")]
public class WaitTillYouCollectRessource : Leaf
{
    public GameObjectReference m_workerGO = new GameObjectReference();
    private Worker_Team m_worker;
    private bool m_workerHaveRessource = false;

    public override void OnEnter()
    {
        m_worker = m_workerGO.Value.GetComponent<Worker_Team>();
    }

    public override NodeResult Execute()
    {
        m_workerHaveRessource = m_worker.m_collectibleInInventory != ECollectibleType.None;

        if(m_worker.m_reservedCollectible == null)
        {
			return NodeResult.failure;
		}
        if (!m_workerHaveRessource)
        {          
            return NodeResult.running;
        }
        else
        {
            return NodeResult.success;
        }
    }
}