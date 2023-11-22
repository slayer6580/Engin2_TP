using MBT;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/Is Path List Incomplete")]
public class Tommy_IsPathListIncomplete : Condition
{
	[SerializeField]
	private BoolReference m_isPathListFull = new BoolReference();
	public override bool Check()
    {
		return !m_isPathListFull.Value;
	 
    }
}
