using MBT;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/Is Assign To A Ressource")]
public class Tommy_IsAssignToARessource : Condition
{
	public TransformReference worker = new TransformReference();
	public override bool Check()
    {
		return worker.Value.gameObject.GetComponent<Tommy_Worker>().assignedRessource != null;
	}
}
