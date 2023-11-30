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
		// (OLD) pack = Tommy_TeamOrchestrator._Instance.FindRessourcePackWithMiddlePoint();


		/* Variable N�C�SSAIRE dans TeamOrchestrator
        m_ressourceToUse =    List<Collectible> : liste des ressources choisi pour �tre min� par les workers, la ressource est enlev� de la liste une fois
		                      qu'un worker y a �t� assign�
		
		m_alreadyUsedRessources = List<Collectible> : Les ressource y sont ajouter en m�me temps que dans m_ressourceToUse mais il ne sont jamais retir�
		                          cette liste est utilis� afin d'�viter de r�assigner plusieurs fois la m�me ressource.
		
		m_campToSpawn =    List<Vector2> : Liste de position de camps � spawn� bien centr� par rapport a ressource dans m_ressourceToUse
         
        m_workerSpeedUnitBySecond = vitesse des worker (m/secondes)
         */

		Tommy_TeamOrchestrator instance = Tommy_TeamOrchestrator._Instance;
		float remainingTime = MapGenerator.SimulationDuration.Value - Time.timeSinceLevelLoad;

		pack = FindBestPackOfRessources(instance.KnownCollectibles, instance.m_ressourceToUse, instance.m_alreadyUsedRessources, instance.m_campToSpawn, instance.m_workerSpeedUnitBySecond, instance.m_minimumDistanceBetweenRessources, remainingTime);


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


	public List<Collectible> FindBestPackOfRessources(List<Collectible> knownCollectibles, List<Collectible> ressourceToUse, List<Collectible> alreadyUsedRessources, List<Vector2> campToSpawn, float workerSpeedUnitBySecond, float minimumDistanceBetweenRessources, float remainingTime)
	{

		int packSize = 4;   //Taile maximal d'un pack de ressource

		List<Collectible> bestPack = new List<Collectible>();
		float bestPossiblePoints = 0;

		List<Collectible> potentialRessourcePack = new List<Collectible>();

		foreach (Collectible ressourceToCheck in knownCollectibles)
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

			//On ne veux pas de ressource seule (pour l'instant) d'o� le >1
			//TODO si les ressource sont TRES �loign�, il peut valloir la peine de faire un camps par ressource, si camps est pas cher
			while (packSize > 1)
			{
				float closestDistance = Mathf.Infinity;
				Collectible closestRessource = null;

				//Find closest collectible
				foreach (Collectible collectible in knownCollectibles)
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
						foreach (Collectible collectible in potentialRessourcePack)
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
			foreach (Collectible collectible in bestPack)
			{
				ressourceToUse.Add(collectible);
				alreadyUsedRessources.Add(collectible);
			}
			campToSpawn.Add(GetCenterOfPack(bestPack));
		}

		return bestPack;
	}

	public Vector2 GetCenterOfPack(List<Collectible> pack)
	{
		Vector3 center = Vector3.zero;

		foreach (Collectible collectible in pack)
		{
			center += collectible.transform.position;
		}
		center /= pack.Count;

		return new Vector2(center.x, center.y);
	}

	public int CheckPossiblePointAtThatPosition(List<Collectible> ressourcePack, Vector2 positionToCheck, float workerSpeedUnitBySecond, float remainingTime)
	{
		int possiblePoints = 0;

		foreach (Collectible collectible in ressourcePack)
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
