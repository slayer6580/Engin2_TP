using MBT;
using UnityEngine;

[MBTNode("Alex Leaf/Wait for ressource cooldown")]
[AddComponentMenu("")]
public class WaitForRessourceCooldown : Leaf
{
    public GameObjectReference m_workerGO = new GameObjectReference();
    private Collectible_Team m_collectible;

    public override void OnEnter()
    {
      m_collectible = m_workerGO.Value.GetComponent<Worker_Team>().m_reservedCollectible;
    }
    public override NodeResult Execute()
    {
  
        if (m_collectible.m_hasBeenPickedInTheLastFiveSeconds)
        {
            return NodeResult.running;
        }
        else
        {
            return NodeResult.success;
        }

    }
}