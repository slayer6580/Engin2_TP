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

        float minDistance = 35;
        bool suitableCampExist = false;

        suitableCampExist = LookForCloseCamp(minDistance, suitableCampExist);

        if (suitableCampExist)
        {
            return true;
        }

        return false;

    }

    private bool LookForCloseCamp(float minDistance, bool suitableCampExist)
    {
        foreach (Camp_Alex camp in Constructing_Manager._Instance.Camps)
        {
            if (Vector2.Distance(camp.transform.position, m_workerPos) < minDistance)
            {
                suitableCampExist = FoundCloseCampAndSetPosition(camp);
            }

        }

        return suitableCampExist;
    }

    private bool FoundCloseCampAndSetPosition(Camp_Alex camp)
    {
        bool suitableCampExist;
        m_targetPosition2D.Value = camp.transform.position;
        suitableCampExist = true;
        return suitableCampExist;
    }
}
