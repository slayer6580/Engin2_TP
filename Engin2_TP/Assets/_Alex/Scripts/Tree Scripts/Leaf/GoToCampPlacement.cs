using MBT;
using UnityEngine;

[MBTNode("Alex Leaf/Go To Camp Placement")]
[AddComponentMenu("")]
public class GoToCampPlacement : Leaf
{
    public float m_speed = 0.1f;
    public float minDistance = 0f;
    public GameObjectReference m_workerGO = new GameObjectReference();

    public override NodeResult Execute()
    {
        Vector2 target = m_workerGO.Value.GetComponent<Worker_Alex>().GetCampPosition();
        Vector2 obj = m_workerGO.Value.transform.position;
        // Move as long as distance is greater than min. distance
        float dist = Vector2.Distance(target, obj);
        if (target != obj)
        {
            // Move towards target
            obj = Vector2.MoveTowards(
                obj,
                target,
                (m_speed > dist) ? dist : m_speed
            );
            return NodeResult.running;


        }
        else
        {
            return NodeResult.success;
        }
    }
}