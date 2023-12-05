using MBT;
using UnityEngine;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Engin2/Check If No Camp")]
    public class CheckIfNoCamp : Leaf
    {
        [Space]
        [SerializeField]
        private Vector2Reference m_assignedPosition = new Vector2Reference();
        [SerializeField]
        private TransformReference m_workerTransform = new TransformReference();

        public override NodeResult Execute()
        {
			
            //If no camp was found, set one in center
			if (m_assignedPosition.Value.x == float.PositiveInfinity)
            {
				m_assignedPosition.Value = Vector2.zero;
				Collecting_Manager._Instance.m_campList.Add(m_assignedPosition.Value);
			}

            

			return NodeResult.success;
        }
    }
}
