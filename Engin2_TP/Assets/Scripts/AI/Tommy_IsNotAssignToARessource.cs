using MBT;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/Is NOT Assign To A Ressource")]
public class Tommy_IsNotAssignToARessource : Condition
{
	public TransformReference worker = new TransformReference();
	public override bool Check()
    {
		return worker.Value.gameObject.GetComponent<Tommy_Worker>().assignedRessource == null;
	}
}
