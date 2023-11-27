using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Alex Condition/Can Place Camp")]
public class CanPlaceCamp_Alex : Condition
{
    public override bool Check()
    {
        return Constructing_Manager._Instance.CanPlaceObject(new Vector2(transform.position.x, transform.position.y));
    }
}