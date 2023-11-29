using MBT;
using UnityEngine;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Engin2/Get New Camp Position")]
    public class Tommy_GetNewCampPosition : Leaf
    {
        [Space]
        [SerializeField]
        private Vector2Reference m_assignedPosition = new Vector2Reference();
        [SerializeField]
        private TransformReference m_workerTransform = new TransformReference();

        public override NodeResult Execute()
        {
			m_assignedPosition.Value = m_workerTransform.Value.gameObject.GetComponent<Tommy_Worker>().campToSpawnPos;
            m_workerTransform.Value.gameObject.GetComponent<Tommy_Worker>().needToSpawnCamp = false;

			return NodeResult.success;
        }
    }
}
