using MBT;
using UnityEngine;

[MBTNode("Alex/Search For Unexplored Zone")]
[AddComponentMenu("")]
public class SearchForUnexploredZone : Leaf
{
    public Vector2Reference m_targetPosition2D = new Vector2Reference(VarRefMode.DisableConstant);
    public TransformReference m_agentTransform = new TransformReference();
    public GameObjectReference m_worker = new GameObjectReference();

    private const int MAX_DETECTION_DISTANCE = 5;
    private const float MIN_DETECTION_DISTANCE = 2.5f;

    public override void OnEnter()
    {
        CheckForNextZone();
    }

    public override NodeResult Execute()
    {
        return NodeResult.success;
    }

    private void CheckForNextZone()
    {
        Worker_Alex worker = m_worker.Value.gameObject.GetComponent<Worker_Alex>();
        Worker_Alex.EDirection workerDirection = worker.m_workerDirection;

        if (CheckZoneAtRightDirection(workerDirection))
        {
            Debug.Log("worker find a zone at is right");
            return;
        }
        else if (CheckZoneAtFrontDirection(workerDirection))
        {
            Debug.Log("worker find a zone at is front");
            return;
        }

        //si le joueur ne peut pas explorer, il va commencer a collecter
        Debug.Log("Worker will start collecting because he cant explore anymore");
        m_worker.Value.GetComponent<Worker_Alex>().m_workerState = Worker_Alex.EWorkerState.collecting;

    }

    private bool CheckZoneAtRightDirection(Worker_Alex.EDirection direction)
    {

        Vector2 m_rightMaxDirection = GetRightDirection(direction);
        if (DetectZone(m_rightMaxDirection))
        {
            ChangeWorkerDirection(m_targetPosition2D.Value);
            return true;
        }


        return false;
    }

    private bool CheckZoneAtFrontDirection(Worker_Alex.EDirection direction)
    {

        Vector2 m_frontMaxDirection = GetFrontMaxDirection(direction);
        if (DetectZone(m_frontMaxDirection))
        {
            return true;
        }

        Vector2 m_frontMinDirection = GetFrontMinDirection(direction);
        if (DetectZone(m_frontMinDirection))
        {
            return true;
        }


        return false;

    }

    private bool DetectZone(Vector2 m_lookDirection)
    {
        Vector2 workerPosition = new Vector2(m_agentTransform.Value.position.x, m_agentTransform.Value.position.y);
        Vector2 targetPos = workerPosition + m_lookDirection;

        if (ExplorationOrchestrator._Instance.m_zonesPositions.Contains(targetPos))
        {
            int index = ExplorationOrchestrator._Instance.m_zonesPositions.IndexOf(targetPos);

            if (ExplorationOrchestrator._Instance.m_zonesIsDetected[index] == false)
            {
                m_targetPosition2D.Value = targetPos;
                ExplorationOrchestrator._Instance.m_zonesIsDetected[index] = true;
                ExplorationOrchestrator._Instance.SpawnDetectedZone(targetPos);
                return true;
            }
        }

        return false;
    }

    private void ChangeWorkerDirection(Vector2 zone)
    {
        if (zone.x < m_agentTransform.Value.position.x)
        {
            m_worker.Value.gameObject.GetComponent<Worker_Alex>().m_workerDirection = Worker_Alex.EDirection.left;
        }
        else if (zone.x > m_agentTransform.Value.position.x)
        {
            m_worker.Value.gameObject.GetComponent<Worker_Alex>().m_workerDirection = Worker_Alex.EDirection.right;
        }
        else if (zone.y > m_agentTransform.Value.position.y)
        {
            m_worker.Value.gameObject.GetComponent<Worker_Alex>().m_workerDirection = Worker_Alex.EDirection.up;
        }
        else
        {
            m_worker.Value.gameObject.GetComponent<Worker_Alex>().m_workerDirection = Worker_Alex.EDirection.down;
        }
    }

    private static Vector2 GetFrontMaxDirection(Worker_Alex.EDirection direction)
    {
        Vector2 m_straightDirection;
        switch (direction)
        {
            case Worker_Alex.EDirection.left:
                m_straightDirection = new Vector2(-MAX_DETECTION_DISTANCE, 0);
                break;
            case Worker_Alex.EDirection.right:
                m_straightDirection = new Vector2(MAX_DETECTION_DISTANCE, 0);
                break;
            case Worker_Alex.EDirection.up:
                m_straightDirection = new Vector2(0, MAX_DETECTION_DISTANCE);
                break;
            case Worker_Alex.EDirection.down:
                m_straightDirection = new Vector2(0, -MAX_DETECTION_DISTANCE);
                break;
            default:
                m_straightDirection = new Vector2(0, 0);
                break;
        }

        return m_straightDirection;
    }

    private static Vector2 GetRightDirection(Worker_Alex.EDirection direction)
    {
        Vector2 m_rightDirection;
        switch (direction)
        {
            case Worker_Alex.EDirection.left:
                m_rightDirection = new Vector2(0, MAX_DETECTION_DISTANCE);
                break;
            case Worker_Alex.EDirection.right:
                m_rightDirection = new Vector2(0, -MAX_DETECTION_DISTANCE);
                break;
            case Worker_Alex.EDirection.up:
                m_rightDirection = new Vector2(MAX_DETECTION_DISTANCE, 0);
                break;
            case Worker_Alex.EDirection.down:
                m_rightDirection = new Vector2(-MAX_DETECTION_DISTANCE, 0);
                break;
            default:
                m_rightDirection = new Vector2(0, 0);
                break;
        }

        return m_rightDirection;
    }

    private static Vector2 GetFrontMinDirection(Worker_Alex.EDirection direction)
    {
        Vector2 m_rightDirection;
        switch (direction)
        {
            case Worker_Alex.EDirection.left:
                m_rightDirection = new Vector2(-MIN_DETECTION_DISTANCE, MIN_DETECTION_DISTANCE);
                break;
            case Worker_Alex.EDirection.right:
                m_rightDirection = new Vector2(MIN_DETECTION_DISTANCE, -MIN_DETECTION_DISTANCE);
                break;
            case Worker_Alex.EDirection.up:
                m_rightDirection = new Vector2(MIN_DETECTION_DISTANCE, MIN_DETECTION_DISTANCE);
                break;
            case Worker_Alex.EDirection.down:
                m_rightDirection = new Vector2(-MIN_DETECTION_DISTANCE, -MIN_DETECTION_DISTANCE);
                break;
            default:
                m_rightDirection = new Vector2(0, 0);
                break;
        }

        return m_rightDirection;
    }
}
