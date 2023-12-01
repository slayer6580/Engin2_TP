using MBT;
using System.Collections.Generic;
using UnityEngine;


[AddComponentMenu("")]
[MBTNode(name = "Engin2/Has Found Enough Ressource To Use")]

public class Tommy_HasFoundEnoughRessourceToUse : Condition
{
	public TransformReference worker = new TransformReference();
	public override bool Check()
    {
		if(Collecting_Manager._Instance.m_alreadyUsedRessources.Count > 39)
		{
			return true;
		}
		return false;
	}
}
