using MBT;
using UnityEngine;

[MBTNode("Alex Leaf/Search For Unexplored Zone")]
[AddComponentMenu("")]
public class SearchForUnexploredZone : Leaf
{
    public Vector2Reference m_targetPosition2D = new Vector2Reference(VarRefMode.DisableConstant);
    public TransformReference m_agentTransform = new TransformReference();
    public GameObjectReference m_worker = new GameObjectReference();

    private const int MAX_DETECTION_DISTANCE = 5;
    private const float MIN_DETECTION_DISTANCE = 2.5f;

    private bool m_foundZone = true;
    public override void OnEnter()
    {
        CheckForNextZone();
		
	}

    public override NodeResult Execute()
    {
		if (m_foundZone)
		{
			return NodeResult.success;
		}
		return NodeResult.failure;
	}

    private void CheckForNextZone()
    {

        Worker_Team worker = m_worker.Value.gameObject.GetComponent<Worker_Team>();
        EDirection workerDirection = worker.m_workerDirection;


        if (CheckZoneAtRightDirection(workerDirection))
<<<<<<< HEAD
        {           
=======
        {
>>>>>>> origin/main
            if (worker.m_extraExplorator == true)
            {
                worker.m_extraExplorator = false;
            }
            return;
        }
        else if (CheckZoneAtFrontDirectionExtra(workerDirection) && worker.m_extraExplorator == true)
        {
<<<<<<< HEAD
        
            return;
        }
        else if (CheckZoneAtFrontDirection(workerDirection))
        {    
=======
            return;
        }
        else if (CheckZoneAtFrontDirection(workerDirection))
        {
>>>>>>> origin/main
            if (worker.m_extraExplorator == true)
            {
                worker.m_extraExplorator = false;
            }
            return;
        }
        else if (CheckZoneAtLeftDirection(workerDirection))
        {
            if (worker.m_extraExplorator == true)
            {
                worker.m_extraExplorator = false;
            }
            return;
        }

        //si le joueur ne peut pas explorer, il va commencer a collecter
        //Debug.Log("Worker will start collecting because he cant explore anymore");
        m_worker.Value.GetComponent<Worker_Team>().m_workerState = EWorkerState.collecting;
        m_foundZone = false;

	}

    private bool CheckZoneAtRightDirection(EDirection direction)
    {

        Vector2 m_rightMaxDirection = GetRightDirection(direction);
        if (DetectZone(m_rightMaxDirection))
        {
            ChangeWorkerDirection(m_targetPosition2D.Value);
            return true;
        }


        return false;
    }
    private bool CheckZoneAtLeftDirection(EDirection direction)
    {

        Vector2 m_leftMaxDirection = GetLeftDirection(direction);
        if (DetectZone(m_leftMaxDirection))
        {
            ChangeWorkerDirection(m_targetPosition2D.Value);
            return true;
        }


        return false;
    }
    private bool CheckZoneAtFrontDirection(EDirection direction)
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
    private bool CheckZoneAtFrontDirectionExtra(EDirection direction)
    {

        Vector2 m_frontMaxDirection = GetFrontMaxDirection(direction);
        if (DetectZoneExtra(m_frontMaxDirection))
        {
            return true;
        }

        Vector2 m_frontMinDirection = GetFrontMinDirection(direction);
        if (DetectZoneExtra(m_frontMinDirection))
        {
            return true;
        }

        return false;

    }

    private bool DetectZone(Vector2 m_lookDirection)
    {
        Vector2 workerPosition = new Vector2(m_agentTransform.Value.position.x, m_agentTransform.Value.position.y);
        Vector2 targetPos = workerPosition + m_lookDirection;

        if (Exploring_Manager._Instance.m_zonesPositions.Contains(targetPos))
        {
            int index = Exploring_Manager._Instance.m_zonesPositions.IndexOf(targetPos);

            if (Exploring_Manager._Instance.m_zonesIsDetected[index] == false)
            {
                m_targetPosition2D.Value = targetPos;
                Exploring_Manager._Instance.m_zonesIsDetected[index] = true;
                Exploring_Manager._Instance.SpawnDetectedZone(targetPos);
                return true;
            }
        }

        return false;
    }

    private bool DetectZoneExtra(Vector2 m_lookDirection)
    {
        Vector2 workerPosition = new Vector2(m_agentTransform.Value.position.x, m_agentTransform.Value.position.y);
        Vector2 targetPos = workerPosition + m_lookDirection;

        if (Exploring_Manager._Instance.m_zonesPositions.Contains(targetPos))
        {
            int index = Exploring_Manager._Instance.m_zonesPositions.IndexOf(targetPos);
            if (Exploring_Manager._Instance.m_zonesIsDetected[index] == true)
            {
                m_targetPosition2D.Value = targetPos;
                return true;
            }
        }

        return false;
    }

    private void ChangeWorkerDirection(Vector2 zone)
    {
        if (zone.x < m_agentTransform.Value.position.x)
        {
            m_worker.Value.gameObject.GetComponent<Worker_Team>().m_workerDirection = EDirection.left;
        }
        else if (zone.x > m_agentTransform.Value.position.x)
        {
            m_worker.Value.gameObject.GetComponent<Worker_Team>().m_workerDirection = EDirection.right;
        }
        else if (zone.y > m_agentTransform.Value.position.y)
        {
            m_worker.Value.gameObject.GetComponent<Worker_Team>().m_workerDirection = EDirection.up;
        }
        else
        {
            m_worker.Value.gameObject.GetComponent<Worker_Team>().m_workerDirection = EDirection.down;
        }
    }

    private static Vector2 GetFrontMaxDirection(EDirection direction)
    {
        Vector2 m_straightDirection;
        switch (direction)
        {
            case EDirection.left:
                m_straightDirection = new Vector2(-MAX_DETECTION_DISTANCE, 0);
                break;
            case EDirection.right:
                m_straightDirection = new Vector2(MAX_DETECTION_DISTANCE, 0);
                break;
            case EDirection.up:
                m_straightDirection = new Vector2(0, MAX_DETECTION_DISTANCE);
                break;
            case EDirection.down:
                m_straightDirection = new Vector2(0, -MAX_DETECTION_DISTANCE);
                break;
            default:
                m_straightDirection = new Vector2(0, 0);
                break;
        }

        return m_straightDirection;
    }

    private static Vector2 GetRightDirection(EDirection direction)
    {
        Vector2 m_rightDirection;
        switch (direction)
        {
            case EDirection.left:
                m_rightDirection = new Vector2(0, MAX_DETECTION_DISTANCE);
                break;
            case EDirection.right:
                m_rightDirection = new Vector2(0, -MAX_DETECTION_DISTANCE);
                break;
            case EDirection.up:
                m_rightDirection = new Vector2(MAX_DETECTION_DISTANCE, 0);
                break;
            case EDirection.down:
                m_rightDirection = new Vector2(-MAX_DETECTION_DISTANCE, 0);
                break;
            default:
                m_rightDirection = new Vector2(0, 0);
                break;
        }

        return m_rightDirection;
    }

    private static Vector2 GetLeftDirection(EDirection direction)
    {
        Vector2 m_rightDirection;
        switch (direction)
        {
            case EDirection.left:
                m_rightDirection = new Vector2(0, -MAX_DETECTION_DISTANCE);
                break;
            case EDirection.right:
                m_rightDirection = new Vector2(0, MAX_DETECTION_DISTANCE);
                break;
            case EDirection.up:
                m_rightDirection = new Vector2(-MAX_DETECTION_DISTANCE, 0);
                break;
            case EDirection.down:
                m_rightDirection = new Vector2(MAX_DETECTION_DISTANCE, 0);
                break;
            default:
                m_rightDirection = new Vector2(0, 0);
                break;
        }

        return m_rightDirection;
    }

    private static Vector2 GetFrontMinDirection(EDirection direction)
    {
        Vector2 m_rightDirection;
        switch (direction)
        {
            case EDirection.left:
                m_rightDirection = new Vector2(-MIN_DETECTION_DISTANCE, MIN_DETECTION_DISTANCE);
                break;
            case EDirection.right:
                m_rightDirection = new Vector2(MIN_DETECTION_DISTANCE, -MIN_DETECTION_DISTANCE);
                break;
            case EDirection.up:
                m_rightDirection = new Vector2(MIN_DETECTION_DISTANCE, MIN_DETECTION_DISTANCE);
                break;
            case EDirection.down:
                m_rightDirection = new Vector2(-MIN_DETECTION_DISTANCE, -MIN_DETECTION_DISTANCE);
                break;
            default:
                m_rightDirection = new Vector2(0, 0);
                break;
        }

        return m_rightDirection;
    }
}
