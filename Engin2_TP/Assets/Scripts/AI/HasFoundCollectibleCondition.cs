using MBT;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/Has Found Collectible")]
public class HasFoundCollectibleCondition : Condition
{
    public override bool Check()
    {
        return TeamOrchestrator._Instance.KnownCollectibles.Count > 0;
    }
}
