using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Alex Condition/Suitable camp exist")]
public class SuitableCampExist : Condition
{
    public GameObjectReference m_workerGO = new GameObjectReference();
    public Vector2Reference m_targetPosition2D = new Vector2Reference(VarRefMode.DisableConstant);

    public override bool Check()
    {
        if (Constructing_Manager._Instance.Camps.Count == 0)
        {
            return false;
        }

        float minDistance = 1000; // 1000 pour test
        Vector2 workerPos = m_workerGO.Value.gameObject.transform.position;
        
        foreach (Camp_Alex camp in Constructing_Manager._Instance.Camps)
        {
            if (Vector2.Distance(workerPos, camp.transform.position) < minDistance)
            {
                minDistance = Vector2.Distance(workerPos, camp.transform.position);
                m_targetPosition2D.Value = camp.transform.position;
            }
        }

        return true;
    }
}
