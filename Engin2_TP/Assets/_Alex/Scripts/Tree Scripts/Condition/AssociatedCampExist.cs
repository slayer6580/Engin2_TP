using MBT;
using System.Linq;
using UnityEngine;


[AddComponentMenu("")]
[MBTNode(name = "Alex Condition/Associated Camp Exist")]
public class AssociatedCampExist : Condition
{
    public GameObjectReference m_workerGO = new GameObjectReference();


    public override void OnEnter()
    {

    }

    public override bool Check()
    {
        Collectible_Alex reservedRessource = m_workerGO.Value.gameObject.GetComponent<Worker_Alex>().m_reservedCollectible;

        if (reservedRessource == null)
        {
            Debug.LogError("CAMP NULL");
        }
        

      
        if (Collecting_Manager._Instance.m_campList.Contains(reservedRessource.m_associatedCamp))
        {
			return true;
		}

        return false;
    }

}