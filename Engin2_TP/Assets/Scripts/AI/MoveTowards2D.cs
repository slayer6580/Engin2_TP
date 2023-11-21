using MBT;
using UnityEngine;

[MBTNode("Engin2/Move Towards 2D")]
[AddComponentMenu("")]
public class MoveTowards2D : Leaf
{
    public Vector2Reference targetPosition;
    public TransformReference transformToMove;
    public float speed = 0.1f;
    public float minDistance = 0f;

    public override NodeResult Execute()
    {
        Vector2 target = targetPosition.Value;
        Transform obj = transformToMove.Value;
        // Move as long as distance is greater than min. distance
        float dist = Vector2.Distance(target, obj.position);
        if (dist > minDistance)
        {
            // Move towards target
            obj.position = Vector2.MoveTowards(
                obj.position,
                target,
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