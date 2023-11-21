using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/Can Place Camp")]
public class CanPlaceCamp : Condition
{
    public override bool Check()
    {
        return TeamOrchestrator._Instance.CanPlaceObject(new Vector2(transform.position.x, transform.position.y));
    }
}