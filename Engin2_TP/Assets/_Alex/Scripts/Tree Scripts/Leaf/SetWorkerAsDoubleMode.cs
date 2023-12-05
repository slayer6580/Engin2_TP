using MBT;
using UnityEngine;

[MBTNode("Alex Leaf/Set Worker As Double Mode")]
[AddComponentMenu("")]
public class SetWorkerAsDoubleMode : Leaf
{
    public GameObjectReference m_workerGO = new GameObjectReference();


    public override void OnEnter()
    {
        m_workerGO.Value.gameObject.GetComponent<Worker_Team>().m_doubleMode = true;

	}

    public override NodeResult Execute()
    {
       
            return NodeResult.success;           
      
    }
}