using MBT;
using System.Collections.Generic;
using UnityEngine;


[AddComponentMenu("")]
[MBTNode(name = "Engin2/Has One Or More Ressource To Use")]

public class Tommy_HasOneOrMoreRessourceToUse : Condition
{
	public TransformReference worker = new TransformReference();
	public override bool Check()
    {
		if(Collecting_Manager._Instance.m_ressourceToUse.Count > 0)
		{
			return true;
		}
		return false;
	}
}
