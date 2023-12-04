using MBT;
using UnityEngine;

[MBTNode("Alex Leaf/Go to reserved ressource")]
[AddComponentMenu("")]
public class GoToReservedRessource : Leaf
{
    public TransformReference transformToMove;
    public float speed = 0.1f;
    public float minDistance = 0f;
    public GameObjectReference m_workerGO = new GameObjectReference();
    private Vector2 m_target;


    public override void OnEnter()
    {
        Collectible_Team collectible = m_workerGO.Value.GetComponent<Worker_Team>().m_reservedCollectible;
        m_target = collectible.transform.position;
    }


    public override NodeResult Execute()
    {        
        Transform obj = transformToMove.Value;

        // Move as long as distance is greater than min. distance
        float dist = Vector2.Distance(m_target, obj.position);
        if (dist > minDistance)
        {
            // Move towards target
            obj.position = Vector2.MoveTowards(
                obj.position,
                m_target,
                (speed > dist) ? dist : speed
            );
            return NodeResult.running;
        }
        else
        {
            return NodeResult.success;
        }
    }
}