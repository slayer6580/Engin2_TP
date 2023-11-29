using System;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{



	[SerializeField]
	private GameObject m_collectiblePrefab;

	public const int WORKER_COST = 20;

	[field: SerializeField]
	public static int Seed { get; private set; }
	[SerializeField]
	private bool m_useRandomSeed = true;
    private int rejectionSamples = 30;

    [field: SerializeField]
	public static RandomIntBetweenRange CampCost { get; private set; } = new RandomIntBetweenRange(5, 80); //Valeur à laquelle vos workers peuvent accéder
																										
	[field: SerializeField]
	public static RandomIntBetweenRange MapDimension { get; private set; } = new RandomIntBetweenRange(60, 600); //Valeur à laquelle vos workers peuvent accéder

	[SerializeField]
	private RandomIntBetweenRange m_nodesDensity;	//Valeur INCONNUE de vos workers
	public static RandomIntBetweenRange SimulationDuration { get; private set; } = new RandomIntBetweenRange(10, 1000); //In seconds. Between 10 and 1000
																														//Valeur à laquelle vos workers peuvent accéder
	[Space]
	[SerializeField] private bool m_takeTestVariables;
	[SerializeField] [Range(5,80)] private int m_campCost;
	[SerializeField] [Range(60,600)] private int m_mapDimension;
	[SerializeField] [Range(10,1000)] private int m_simulationDuration;
	[SerializeField] [Range(12,80)] private int m_nodeDensity;

    private void Awake()
    {
		GenerateValues();
		GenerateMap();
		ShiftMap();
    }

    private void Update()
    {
        if (SimulationDuration.Value < Time.timeSinceLevelLoad)
		{
			TeamOrchestrator._Instance.OnGameEnded();
		}
    }

    private void GenerateValues()
	{
		if (m_useRandomSeed)
		{
            Seed = (int)DateTime.Now.Ticks;
        }
		UnityEngine.Random.InitState(Seed);

		if (m_takeTestVariables)
		{
			CampCost.Value = m_campCost;
			MapDimension.Value = m_mapDimension;
			m_nodesDensity.Value = m_nodeDensity;
			SimulationDuration.Value = m_simulationDuration;

		}
		else
		{
            CampCost.RollValue();
            MapDimension.RollValue();
            m_nodesDensity.RollValue();
            SimulationDuration.RollValue();
        }
		
    }

    private void GenerateMap()
    {
        List<Vector2> points;
        points = PoissonDiscSampling.GeneratePoints(m_nodesDensity.Value, Vector2.one * MapDimension.Value, rejectionSamples);

		foreach (var point in points)
		{
			Instantiate(m_collectiblePrefab, new Vector3(point.x, point.y, 0), Quaternion.identity, transform);
		}
		Debug.Log("Points generated: " + points.Count.ToString());
		Debug.Log("Game Timer: " + SimulationDuration.Value);
    }

    private void ShiftMap()
    {
        transform.Translate(-MapDimension.Value / 2, -MapDimension.Value / 2, 0);
    }
}

[System.Serializable]
public class RandomIntBetweenRange
{
	[SerializeField]
	private int m_minimumValue = 0;
	[SerializeField]
	private int m_maximumValue = 100;
	[field:SerializeField]
	public int Value { get;  set; }

	public RandomIntBetweenRange()
	{

	}

	public RandomIntBetweenRange(int min, int max)
	{
		m_minimumValue = min; m_maximumValue = max;
    }

	public int GetMin()
	{
		return m_minimumValue;
	}

    public int GetMax()
    {
        return m_maximumValue;
    }

    public void RollValue()
	{
        Value = UnityEngine.Random.Range(m_minimumValue, m_maximumValue + 1);
	}
}