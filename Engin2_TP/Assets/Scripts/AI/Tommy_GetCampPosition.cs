using MBT;
using UnityEngine;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Engin2/Get Camp Position")]
    public class Tommy_GetCampPosition : Leaf
    {
        [Space]
        [SerializeField]
        private Vector2Reference m_assignedPosition = new Vector2Reference();
        [SerializeField]
        private TransformReference m_workerTransform = new TransformReference();

        public override NodeResult Execute()
        {
            Vector2 campPos = m_workerTransform.Value.gameObject.GetComponent<Worker_Team>().m_campPosition;
            m_assignedPosition.Value = campPos;

			return NodeResult.success;
        }
    }
}
