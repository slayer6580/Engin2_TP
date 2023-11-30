using MBT;
using UnityEngine;


[AddComponentMenu("")]
[MBTNode(name = "Alex Condition/Suitable camp exist")]
public class SuitableCampExist : Condition
{
    public GameObjectReference m_workerGO = new GameObjectReference();
    public Vector2Reference m_targetPosition2D = new Vector2Reference(VarRefMode.DisableConstant);
    private Vector2 m_workerPos;

    public override void OnEnter()
    {
        m_workerPos = m_workerGO.Value.transform.position;
    }

    public override bool Check()
    {
        if (Constructing_Manager._Instance.Camps.Count == 0)
        {
            return false;
        }

        float minDistance = (MapGenerator.CampCost.GetValue() * Constructing_Manager._Instance.m_campDistanceByCostMultiplier) + 10;
        bool suitableCampExist = false;

        // regarde si la distance d'un camp est plus petit qu'une formule selon le cout du camp
        foreach (Camp_Alex camp in Constructing_Manager._Instance.Camps)
        {
            // trouver le camp le plus proche
            if (Vector2.Distance(camp.transform.position, m_workerPos) < minDistance)
            {
            
                minDistance = Vector2.Distance(camp.transform.position, m_workerPos);
                // donner une position du camp
                m_targetPosition2D.Value = camp.transform.position;
                suitableCampExist = true;
            }
        }

        if (suitableCampExist)
        {
            return true;
        }

        return false;

    }

  
}
