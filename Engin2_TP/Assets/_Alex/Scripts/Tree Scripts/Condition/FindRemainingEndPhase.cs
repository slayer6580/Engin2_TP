using MBT;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

[AddComponentMenu("")]
[MBTNode(name = "François Condition/Find Remaining EndPhase")]
public class FindRemainingEndPhase : Condition
{
    public GameObjectReference m_workerGO = new GameObjectReference();
    private Vector2 m_workerPos;

    public override void OnEnter()
    {
        m_workerPos = m_workerGO.Value.transform.position;
    }

    public override bool Check()
    {
        float minDistance = float.PositiveInfinity;
        bool suitableRessourceExist = false;
        Collectible_Team chosenRessource = null;

        // regarde si la distance d'un camp est plus petit qu'un autre camps
        foreach (Collectible_Team ressource in Collecting_Manager._Instance.KnownCollectibles)
        {
                // trouver le camp le plus proche
                float tempMinDistance = Vector2.Distance(ressource.transform.position, m_workerPos);

                if (tempMinDistance < minDistance)
                {
                    minDistance = tempMinDistance;
                    chosenRessource = ressource;
                    suitableRessourceExist = true;
                }     
        }

        if (suitableRessourceExist)
        {
            m_workerGO.Value.gameObject.GetComponent<Worker_Team>().m_reservedCollectible = chosenRessource;
            return true;
        }
        return false;

    }
}
