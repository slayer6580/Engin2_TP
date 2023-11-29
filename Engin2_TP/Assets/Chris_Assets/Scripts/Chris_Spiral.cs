using MBT;
using UnityEngine;

[MBTNode("Chris/Spiral")]
[AddComponentMenu("")]

public class Chris_Spiral : Leaf
{

    public override void OnEnter()
    {
        //var pos = Random.insideUnitCircle * m_movementRange.Value;
        //m_targetPosition2D.Value = pos + new Vector2(m_agentTransform.Value.position.x, m_agentTransform.Value.position.y);
    }

    public override NodeResult Execute()
    {
        Debug.Log("On GeneratedPointAroundSelf execute");
        return NodeResult.success;
    }
}
