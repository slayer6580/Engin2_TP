using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Alex Condition/Is Ressource Near For Two Collect")]

public class IsRessourceNearForTwoCollect : Condition
{
	public GameObjectReference m_workerGO = new GameObjectReference();

    public override bool Check()
    {
        Vector2 campPosition = m_workerGO.Value.GetComponent<Worker_Team>().m_campPosition;
        Vector2 ressourcePosition = m_workerGO.Value.GetComponent<Worker_Team>().m_reservedCollectible.transform.position;

        float distance = Vector2.Distance(campPosition, ressourcePosition);

        float tripTime = distance / Collecting_Manager._Instance.WORKER_SPEED_BY_SECOND;

        if (((tripTime * 2) + 2) < 5)
        {
            return true;
        }

        return false;

    }
}
