using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationOrchestrator : MonoBehaviour
{

    [SerializeField] private GameObject m_zone;
    private int m_zoneLenght = 0;
    private int m_mapDimension = 0;
    private List<float> m_xPositions = new List<float>();
    private List<float> m_yPositions = new List<float>();
    private const int EXPLORING_WORKERS = 4;
    private static int m_workerInExploration = 0;

    void Start()
    {
        m_mapDimension = MapGenerator.MapDimension.Value;
        Debug.Log("Map dimension: " + m_mapDimension.ToString());

        m_zoneLenght = (m_mapDimension / 5) + 1;
        SetZoneListPosition();

        for (int i = 0; i < m_zoneLenght; i++)
        {
            for (int j = 0; j < m_zoneLenght; j++)
            {
                Vector2 zonePosition = new Vector2(m_xPositions[j], m_yPositions[i]);
                GameObject zone = Instantiate(m_zone, zonePosition, transform.rotation);
                zone.transform.SetParent(transform);
            }
        }
    }

    private void SetZoneListPosition()
    {
        int mapDimension = m_mapDimension;

        while (mapDimension % 5 != 0)
        {
            mapDimension--;
        }

        float halfMapDimension = mapDimension / 2;

        while (halfMapDimension % 2.5f != 0)
        {
            halfMapDimension--;
        }


        for (int i = 0; i < m_zoneLenght; i++)
        {
            float x = -halfMapDimension + (i * 5);
            m_xPositions.Add(x);

            float y = halfMapDimension - (i * 5);
            m_yPositions.Add(y);
        }

    }

    public static void SetExploringToWorker(Worker_Alex worker)
    {

        if (m_workerInExploration >= EXPLORING_WORKERS)
        {
            return;
        }

        switch (m_workerInExploration)
        {
            case 0:
                worker.m_workerDirection = Worker_Alex.EDirection.left;
                break;
            case 1:
                worker.m_workerDirection = Worker_Alex.EDirection.right;
                break;
            case 2:
                worker.m_workerDirection = Worker_Alex.EDirection.up;
                break;
            case 3:
                worker.m_workerDirection = Worker_Alex.EDirection.down;
                break;
            default:
                break;
        }

        worker.m_workerState = Worker_Alex.EWorkerState.exploring;
        m_workerInExploration++;
    }
}
