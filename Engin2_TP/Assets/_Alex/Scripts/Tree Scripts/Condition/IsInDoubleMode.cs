using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Alex Condition/Is in double mode")]

public class IsInDoubleMode : Condition
{
	public GameObjectReference m_workerGO = new GameObjectReference();

    public override bool Check()
    {
   

       

        if (m_workerGO.Value.gameObject.GetComponent<Worker_Team>().m_doubleMode)
        {
            return true;
        }

        return false;

    }
}
