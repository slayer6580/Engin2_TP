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
            Vector2 campPos = m_workerTransform.Value.gameObject.GetComponent<Worker_Team>().m_reservedCollectible.m_associatedCamp;
            m_assignedPosition.Value = campPos;

            Collecting_Manager._Instance.m_campList.Add(campPos);

			return NodeResult.success;
        }
    }
}
