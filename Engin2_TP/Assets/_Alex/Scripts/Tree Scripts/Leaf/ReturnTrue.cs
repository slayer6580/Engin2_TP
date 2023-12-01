using MBT;
using UnityEngine;

[MBTNode("Alex Leaf/Return True")]
[AddComponentMenu("")]
public class ReturnTrue : Leaf
{

    public override void OnEnter()
    {
      
    }

    public override NodeResult Execute()
    {
		return NodeResult.success;
	}
}