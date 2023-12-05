using MBT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[MBTNode("Engin2/Search Best Camp Position")]
[AddComponentMenu("")]
public class Tommy_SearchBestCampPosition : Leaf
{
	List<Collectible_Team> pack = new List<Collectible_Team>();


	public override void OnEnter()
    {
		

		Collecting_Manager instance = Collecting_Manager._Instance;
		float remainingTime = MapGenerator.SimulationDuration.Value - Time.timeSinceLevelLoad;

		pack = FindBestPackOfRessources(instance.KnownCollectibles, instance.m_ressourceToUse, instance.m_alreadyUsedRessources, instance.WORKER_SPEED_BY_SECOND, instance.m_predictionDistance, remainingTime);


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
		return NodeResult.success;
		
    }


	public List<Collectible_Team> FindBestPackOfRessources(List<Collectible_Team> knownCollectibles, List<Collectible_Team> ressourceToUse, List<Collectible_Team> alreadyUsedRessources, float workerSpeedUnitBySecond, float minimumDistanceBetweenRessources, float remainingTime)
	{

		int packSize = 4;   //Taile maximal d'un pack de ressource

		List<Collectible_Team> bestPack = new List<Collectible_Team>();
		float bestPossiblePoints = 0;

		List<Collectible_Team> potentialRessourcePack = new List<Collectible_Team>();

		foreach (Collectible_Team ressourceToCheck in knownCollectibles)
		{
			potentialRessourcePack.Clear();

			//Reduce packSize if not enough ressource available
			int availableRessourceToCheck = knownCollectibles.Count - alreadyUsedRessources.Count;
			while (availableRessourceToCheck < packSize)
			{
				packSize--;
			}
			
			//Avoid using same ressource twice
			if (alreadyUsedRessources.Contains(ressourceToCheck) == true)
			{
				continue;
			}

			potentialRessourcePack.Add(ressourceToCheck);

			Vector2 packCenterPos = ressourceToCheck.transform.position;

			
			//TODO si les ressource sont TRES éloigné, il peut valloir la peine de faire un camps par ressource, si camps est pas cher
			while (packSize > 1)
			{
				float closestDistance = Mathf.Infinity;
                Collectible_Team closestRessource = null;

				//Find closest collectible
				foreach (Collectible_Team collectible in knownCollectibles)
				{
					//Test with all ressource EXCEPT for those already in the pack AND those used by other pack
					if (potentialRessourcePack.Contains(collectible) == false)
					{
						if (alreadyUsedRessources.Contains(collectible) == false)
						{
							//Only keep the nearest ressource of the pack center
							float currentDistance = Vector2.Distance(packCenterPos, collectible.transform.position);
							if (currentDistance < closestDistance)
							{
								closestDistance = currentDistance;
								closestRessource = collectible;
							}
						}
					}
				}

				potentialRessourcePack.Add(closestRessource);
				packCenterPos = GetCenterOfPack(potentialRessourcePack);
				int checkPossiblePoint = CheckPossiblePointAtThatPosition(potentialRessourcePack, packCenterPos, workerSpeedUnitBySecond, remainingTime);

				//If there's not the maximum of ressource for one camp (4), suggest that another camp will be required somewhere else
				int campCost = MapGenerator.CampCost.Value;
				if (potentialRessourcePack.Count < 4)
				{
					campCost += campCost;
				}

				//Calculate if it's currently the best option
				if (checkPossiblePoint - campCost >= bestPossiblePoints)
				{
					//Verify if ressource are not too far from each other
					bool closeEnough = true;
					for (int i = 0; i < potentialRessourcePack.Count; i++)
					{
						//We check the distance of each ressource from the pack with each other
						int j = i + 1;
						if (j >= potentialRessourcePack.Count)
						{
							j = 0;
						}
						float dist = Vector2.Distance(potentialRessourcePack[i].transform.position, potentialRessourcePack[j].transform.position);
						//TODO change 2 to global const to make some test
						if (dist > (minimumDistanceBetweenRessources * 2f))
						{
							closeEnough = false;
						}
					}
					//All test has passed, this potentialPack is for now the best option
					if (closeEnough == true)
					{
						bestPossiblePoints = checkPossiblePoint - campCost;
						bestPack.Clear();
						foreach (Collectible_Team collectible in potentialRessourcePack)
						{
							bestPack.Add(collectible);
						}
					}
				}
				packSize--;
			}
		}//End foreach

		if (bestPack.Count > 0)
		{
			Vector2 campPosition = GetCenterOfPack(bestPack);

			foreach (Collectible_Team collectible in bestPack)
			{
				float distance = Vector2.Distance(campPosition, collectible.transform.position);
				while (distance < 1)
				{

					campPosition.x += 1;
					campPosition.y += 1;
					distance = Vector2.Distance(campPosition, collectible.transform.position);
				}
			}

			foreach (Collectible_Team collectible in bestPack)
			{
				collectible.m_associatedCamp = campPosition;
				ressourceToUse.Add(collectible);
				alreadyUsedRessources.Add(collectible);
			}
		}

		return bestPack;
	}

	public Vector2 GetCenterOfPack(List<Collectible_Team> pack)
	{
		Vector3 center = Vector3.zero;

		foreach (Collectible_Team collectible in pack)
		{
			center += collectible.transform.position;
		}
		center /= pack.Count;

		return new Vector2(center.x, center.y);
	}

	public int CheckPossiblePointAtThatPosition(List<Collectible_Team> ressourcePack, Vector2 positionToCheck, float workerSpeedUnitBySecond, float remainingTime)
	{
		int possiblePoints = 0;

		foreach (Collectible_Team collectible in ressourcePack)
		{
			float distanceFromRessource = Vector2.Distance(positionToCheck, collectible.transform.position);
			float travelTime = distanceFromRessource / workerSpeedUnitBySecond;
			float timeToCollect = travelTime + 2;
			if (timeToCollect < 5)
			{
				timeToCollect = 5;
			}

			possiblePoints += Mathf.FloorToInt(remainingTime / timeToCollect);
		}


		// print("Possible points with " + ressourcePack.Count + " ressources. = " + possiblePoints);
		return possiblePoints;
	}
}
