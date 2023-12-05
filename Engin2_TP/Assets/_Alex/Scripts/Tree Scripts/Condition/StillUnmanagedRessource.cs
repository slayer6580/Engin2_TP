using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Alex Condition/Is there still unmanaged ressources")]

public class StillUnmanagedRessource : Condition
{
	public GameObjectReference m_workerGO = new GameObjectReference();

    public override bool Check()
    {

		Collecting_Manager collectManager = Collecting_Manager._Instance;
        if(collectManager.KnownCollectibles.Count > collectManager.m_alreadyUsedRessources.Count)
        {
			return true;
		}

		return false;

    }
}
