using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Alex Condition/Look for available ressource?")]

public class LookIfRessourceAvailable : Condition
{
    public override bool Check()
    {
        foreach (Collectible_Alex collectible in Collecting_Manager._Instance.KnownCollectibles)
        {
            if (collectible.m_designedWorker == null) 
            {
                return true;
            }
        }

        return false;
    }
}
