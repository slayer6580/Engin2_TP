using MBT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[MBTNode("Engin2/Generate Point Around Self")]
[AddComponentMenu("")]
public class GeneratePointAroundSelfBehavior : Leaf
{
    public FloatReference m_movementRange = new FloatReference(VarRefMode.DisableConstant);
    public Vector2Reference m_targetPosition2D = new Vector2Reference(VarRefMode.DisableConstant);
    public TransformReference m_agentTransform = new TransformReference();

    public override void OnEnter()
    {
        var pos = Random.insideUnitCircle * m_movementRange.Value;
        m_targetPosition2D.Value = pos + new Vector2(m_agentTransform.Value.position.x, m_agentTransform.Value.position.y);
    }

    public override NodeResult Execute()
    {
        Debug.Log("On GeneratedPointAroundSelf execute");
        return NodeResult.success;
    }
}
