using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Alex Condition/Is Camp Worth It")]
public class IsCampWorthIt : Condition
{
    public override bool Check()
    {
        float distBetweenRessource = Collecting_Manager._Instance.m_predictionDistance;
        float workerSpeed = Collecting_Manager._Instance.WORKER_SPEED_BY_SECOND;
        int campCost = MapGenerator.CampCost.Value;

        int tripTime = (int)Mathf.Floor(((distBetweenRessource / workerSpeed)) + 2);

        if (tripTime < 5) tripTime = 5;

        float remaintingTime = TeamOrchestrator_Team._Instance.m_remainingTime;

        int possiblePointByWorker = (int)Mathf.Floor(remaintingTime / tripTime);


        if ((possiblePointByWorker * 4) > campCost)
        {
            print("WORTH IT");
            return true;
        }
		print("DONT -- WORTH IT");
		return false;
    }
}