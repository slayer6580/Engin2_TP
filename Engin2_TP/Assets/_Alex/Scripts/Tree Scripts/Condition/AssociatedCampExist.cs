using MBT;
using System.Linq;
using UnityEngine;


[AddComponentMenu("")]
[MBTNode(name = "Alex Condition/Associated Camp Exist")]
public class AssociatedCampExist : Condition
{
    public GameObjectReference m_workerGO = new GameObjectReference();
    Collectible_Team reservedRessource = null;


	public override void OnEnter()
    {
        reservedRessource = m_workerGO.Value.gameObject.GetComponent<Worker_Team>().m_reservedCollectible;
	}

    public override bool Check()
    {
        if (reservedRessource == null)
        {
            Debug.LogError("CAMP NULL");
        }
        
        if(reservedRessource.m_associatedCamp.x != float.PositiveInfinity)
        {
			if (Collecting_Manager._Instance.m_campList.Contains(reservedRessource.m_associatedCamp))
			{
				return true;
			}
		}  
        return false;
    }

}
