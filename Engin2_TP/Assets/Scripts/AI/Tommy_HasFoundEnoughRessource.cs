using MBT;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/Has Found Enough Ressources")]


public class Tommy_HasFoundEnoughRessource : Condition
{
	public enum MinimumRessourceMethod
	{
		MinimumAmount,
		Percentage
	}

	[SerializeField]
	private MinimumRessourceMethod m_usedMethod;

	[SerializeField]
	private int m_minimumAmount;
	[SerializeField]
	private int m_percentage;


	public override bool Check()
    {
		if(m_usedMethod == MinimumRessourceMethod.MinimumAmount)
		{
			int knownCollectible = Tommy_TeamOrchestrator._Instance.KnownCollectibles.Count;
            if (knownCollectible >= m_minimumAmount)
            {
				
				return true;
			}
        }
		//Tommy_Worker agentScript = m_agentTransform.Value.gameObject.GetComponent<Tommy_Worker>();
		return false;
    }
}
