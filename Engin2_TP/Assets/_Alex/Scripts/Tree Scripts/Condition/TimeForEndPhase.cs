using MBT;
using System.Linq;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Alex Condition/Time For Endphase")]
public class TimeForEndPhase : Condition
{
    //public GameObjectReference m_workerGO = new GameObjectReference();
    private float m_remainingTime;


    private void Start()
    {
        m_remainingTime = MapGenerator.SimulationDuration.Value;
    }
    public override bool Check()
    {
        m_remainingTime = MapGenerator.SimulationDuration.Value;

        if (m_remainingTime<100)
        {
            return true;
            Debug.Log("ENDPHASE  yet");
        }
        Debug.Log("ENDPHASE not yet");
        return false;
      
    }
}
