using MBT;
using UnityEngine;

[MBTNode("Alex/Search For Unexplored Zone")]
[AddComponentMenu("")]
public class SearchForUnexploredZone : Leaf
{
    public Vector2Reference m_targetPosition2D = new Vector2Reference(VarRefMode.DisableConstant);
    public TransformReference m_agentTransform = new TransformReference();
    public GameObjectReference m_worker = new GameObjectReference();

    private LayerMask zoneLayer;
    private const int DETECTION_DISTANCE = 4;

    public override void OnEnter()
    {
        zoneLayer = LayerMask.GetMask("Zone");
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

        if (CheckOverlapRight(workerDirection))
        {
            Debug.Log("worker turn right");
            return;
        }
        else if (CheckOverlapStraight(workerDirection))
        {
            Debug.Log("worker go straight");
            return;
        }

        //si le joueur ne peut pas explorer, il va commencer a collecter
        Debug.Log("Worker will start collecting because he cant explore anymore");
        m_worker.Value.GetComponent<Worker_Alex>().m_workerState = Worker_Alex.EWorkerState.collecting;

    }

    private bool CheckOverlapStraight(Worker_Alex.EDirection direction)
    {
        Vector2 m_straightDirection;
        m_straightDirection = LookStraight(direction);
        return DetectZone(m_straightDirection);
    }

    private bool CheckOverlapRight(Worker_Alex.EDirection direction)
    {
        Vector2 m_rightDirection;
        m_rightDirection = LookRight(direction);
        return DetectZone(m_rightDirection);

    }

    private static Vector2 LookStraight(Worker_Alex.EDirection direction)
    {
        Vector2 m_straightDirection;
        switch (direction)
        {
            case Worker_Alex.EDirection.left:
                m_straightDirection = new Vector2(-DETECTION_DISTANCE, 0);
                break;
            case Worker_Alex.EDirection.right:
                m_straightDirection = new Vector2(DETECTION_DISTANCE, 0);
                break;
            case Worker_Alex.EDirection.up:
                m_straightDirection = new Vector2(0, DETECTION_DISTANCE);
                break;
            case Worker_Alex.EDirection.down:
                m_straightDirection = new Vector2(0, -DETECTION_DISTANCE);
                break;
            default:
                m_straightDirection = new Vector2(0, 0);
                break;
        }

        return m_straightDirection;
    }

    private static Vector2 LookRight(Worker_Alex.EDirection direction)
    {
        Vector2 m_rightDirection;
        switch (direction)
        {
            case Worker_Alex.EDirection.left:
                m_rightDirection = new Vector2(0, DETECTION_DISTANCE);
                break;
            case Worker_Alex.EDirection.right:
                m_rightDirection = new Vector2(0, -DETECTION_DISTANCE);
                break;
            case Worker_Alex.EDirection.up:
                m_rightDirection = new Vector2(DETECTION_DISTANCE, 0);
                break;
            case Worker_Alex.EDirection.down:
                m_rightDirection = new Vector2(-DETECTION_DISTANCE, 0);
                break;
            default:
                m_rightDirection = new Vector2(0, 0);
                break;
        }

        return m_rightDirection;
    }

    private bool DetectZone(Vector2 m_lookDirection)
    {
        Vector2 workerPosition = new Vector2(m_agentTransform.Value.position.x, m_agentTransform.Value.position.y);
        Vector2 targetPos = workerPosition + m_lookDirection;

        Collider2D zoneCollider = Physics2D.OverlapCircle(targetPos, 0.2f, zoneLayer);
        Zone_Alex zone = zoneCollider.gameObject.GetComponent<Zone_Alex>();

        if (zone == null)
        {
            return false;
        }

        if (!zone.m_isExplored)
        {
            m_targetPosition2D.Value = zone.transform.position;
            ChangeWorkerDirection(zone);

            return true;
        }

        return false;
    }

    private void ChangeWorkerDirection(Zone_Alex zone)
    {
        if (zone.transform.position.x < m_agentTransform.Value.position.x)
        {
            m_worker.Value.gameObject.GetComponent<Worker_Alex>().m_workerDirection = Worker_Alex.EDirection.left;
        }
        else if (zone.transform.position.x > m_agentTransform.Value.position.x)
        {
            m_worker.Value.gameObject.GetComponent<Worker_Alex>().m_workerDirection = Worker_Alex.EDirection.right;
        }
        else if (zone.transform.position.y > m_agentTransform.Value.position.y)
        {
            m_worker.Value.gameObject.GetComponent<Worker_Alex>().m_workerDirection = Worker_Alex.EDirection.up;
        }
        else
        {
            m_worker.Value.gameObject.GetComponent<Worker_Alex>().m_workerDirection = Worker_Alex.EDirection.down;
        }
    }
}
