using MBT;
using System;
using UnityEngine;

[MBTNode("Alex Leaf/Go to random unexplored zone")]
[AddComponentMenu("")]
public class GoToUnexploredZone : Leaf
{
    public TransformReference transformToMove;
    public float speed = 0.1f;
    public float minDistance = 0f;
    public GameObjectReference m_workerGO = new GameObjectReference();
    private Vector2 m_target;


    public override void OnEnter()
    {
        Vector2 playerPos = m_workerGO.Value.gameObject.transform.position;
        m_target = Wandering_Manager._Instance.FindAnRandomExploredZone(playerPos);
    }


    public override NodeResult Execute()
    {
        Transform obj = transformToMove.Value;

        foreach (Collectible_Alex collectible in Collecting_Manager._Instance.KnownCollectibles)
        {
            if (collectible.m_designedWorker == null)
            {
                return NodeResult.failure;
            }
        }

        // Move as long as distance is greater than min. distance
        float dist = Vector2.Distance(m_target, obj.position);
        if (dist > minDistance)
        {
            // Move towards target
            obj.position = Vector2.MoveTowards(
                obj.position,
                m_target,
                (speed > dist) ? dist : speed
            );
            return NodeResult.running;
        }
        else
        {
            int index = Exploring_Manager._Instance.m_zonesPositions.IndexOf(m_target);

            if (Exploring_Manager._Instance.m_zonesIsDetected[index] == false)
            {
                Exploring_Manager._Instance.m_zonesIsDetected[index] = true;
                Exploring_Manager._Instance.SpawnDetectedZone(m_target);
            }
 
            return NodeResult.success;
        }
    }
}