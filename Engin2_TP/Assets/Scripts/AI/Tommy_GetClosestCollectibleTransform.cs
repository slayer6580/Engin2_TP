using MBT;
using UnityEngine;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Engin2/Tommy Get closest collectible Transform")]
    public class Tommy_GetClosestCollectibleTransform : Leaf
    {
        [Space]
        [SerializeField]
        private TransformReference m_closestColTrans = new TransformReference();
        [SerializeField]
        private TransformReference m_workerTransform = new TransformReference();

        public override NodeResult Execute()
        {
            if (Tommy_TeamOrchestrator._Instance.KnownCollectibles.Count == 0)
            {
                //On n'a pas trouvé de collectible. On retourne sans avoir updaté
                return NodeResult.failure;
            }

            Collectible nearestCollectible = Tommy_TeamOrchestrator._Instance.KnownCollectibles[0];

            foreach (var ressource in Tommy_TeamOrchestrator._Instance.KnownCollectibles)
            {
                if (Vector3.Distance(nearestCollectible.transform.position, m_workerTransform.Value.position)
                    > Vector3.Distance(ressource.transform.position, m_workerTransform.Value.position))
                {
					nearestCollectible = ressource;
                }
            }

			//Ceci est la ressource le plus près. On update sa valeur dans le blackboard et retourne true
			m_closestColTrans.Value = nearestCollectible.transform;


			return NodeResult.success;
        }
    }
}

[AddComponentMenu("")]
[MBTNode("Example/Set Random Position", 500)]
public class Tommy_SetRandomPosition : Leaf
{
    public Bounds bounds;
    public Vector3Reference blackboardVariable = new Vector3Reference(VarRefMode.DisableConstant);

    public override NodeResult Execute()
    {
        // Random values per component inside bounds
        blackboardVariable.Value = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
        return NodeResult.success;
    }
}