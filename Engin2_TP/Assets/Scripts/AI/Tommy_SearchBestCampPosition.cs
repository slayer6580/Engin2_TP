using MBT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[MBTNode("Engin2/Search Best Camp Position")]
[AddComponentMenu("")]
public class Tommy_SearchBestCampPosition : Leaf
{
	List<Collectible> pack = new List<Collectible>();
	public override void OnEnter()
    {
        
       pack = Tommy_TeamOrchestrator._Instance.FindRessourcePackWithMiddlePoint();
       // m_packCenter.Value = Tommy_TeamOrchestrator._Instance.GetCenterOfPack(pack);
	}

    public override NodeResult Execute()
    {

        if(pack.Count > 0)
        {
            print("SEARCH SUCESS");
			//Debug.Log("On SEARCH BEST CAMP execute");
			return NodeResult.success;
		}
		print("SEARCH FAILURE");
		return NodeResult.failure;
		
    }
}
