using MBT;
using System.Linq;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Alex Condition/Time For Endphase")]
public class TimeForEndPhase : Condition
{
    //public GameObjectReference m_workerGO = new GameObjectReference();
    private float m_remainingTime;
    private float timeForEndPhase =40.0f; // formule de Chris pour calculer


    private void Start()
    {
        m_remainingTime = MapGenerator.SimulationDuration.Value;
    }
    public override bool Check()
    {
        m_remainingTime = TeamOrchestrator_Team._Instance.ShareTimeLeft();

        if (m_remainingTime< timeForEndPhase)
        {
            return true;
                //Debug.Log("ENDPHASE  yet time left" + m_remainingTime+ "time for endphase" + timeForEndPhase);
        }
        Debug.Log("ENDPHASE not yet time left" + m_remainingTime + "time for endphase" + timeForEndPhase);
        return false;
      
    }


}
