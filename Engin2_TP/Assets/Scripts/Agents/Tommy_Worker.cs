using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public enum ExploringDirection
{
	Up,
	Right,
	Down,
	Left,
	Count
}

public class Tommy_Worker : MonoBehaviour
{
    private const float EXTRACTION_DURATION = 1.0f;
    private const float DEPOSIT_DURATION = 1.0f;

    [SerializeField]
    private float m_radius = 5.0f;
    [SerializeField]
    private Transform m_radiusDebugTransform;
    [SerializeField]
    private ECollectibleType m_collectibleInInventory = ECollectibleType.None;
    [SerializeField]
    private Collectible m_currentExtractingCollectible;

    private bool m_isInDepot = false;
    private bool m_isInExtraction = false;
    private float m_currentActionDuration = 0.0f;


	public const float VISION_RADIUS = 5.0f;
	public List<Vector2Int> m_workerPath = new List<Vector2Int>();        //NEW
	public ExploringDirection m_currentExploreDir = ExploringDirection.Count;      //NEW
    public bool IsPathListFull;     //NEW
	public Vector2Int nextChunk = new Vector2Int(0, 0);      //NEW
	[SerializeField]
	private GameObject m_trailVisualizer;       //NEW
	private Tommy_TeamOrchestrator m_orchestrator;
	private Vector2Int m_initialChunk;      //NEW
	private int m_skipFirstSpeedTest = 2;
	private Vector2Int currentChunk;

	private float speedTest = 0;

	public Collectible assignedRessource;
	public bool needToSpawnCamp = false;
	public Vector2 campToSpawnPos;
	public int GetExplorePathCount()
	{
		return m_workerPath.Count;
	}
	private void OnValidate()
    {
        m_radiusDebugTransform.localScale = new Vector3(m_radius, m_radius, m_radius);
    }

    private void Start()
    {
		m_orchestrator = Tommy_TeamOrchestrator._Instance;
		m_orchestrator.WorkersList.Add(this);
    }

	private void FixedUpdate()
    {
        if (m_isInDepot || m_isInExtraction)
        {
            m_currentActionDuration -= Time.fixedDeltaTime;
            if (m_currentActionDuration < 0.0f)
            {
                if (m_isInDepot)
                {
                    DepositResource();
                }
                else
                {
                    GainCollectible();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var collectible = collision.GetComponent<Collectible>();
        if (collectible != null && m_collectibleInInventory == ECollectibleType.None)
        {
            m_currentExtractingCollectible = collectible;
            m_currentActionDuration = EXTRACTION_DURATION;
            m_isInExtraction = true;
            //Start countdown to collect it

        }

        var camp = collision.GetComponent<Camp>();
        if (camp != null && m_collectibleInInventory != ECollectibleType.None)
        {
            m_currentActionDuration = DEPOSIT_DURATION;
            m_isInDepot = true;
            //Start countdown to deposit my current collectible (if it exists)
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var collectible = collision.GetComponent<Collectible>();
        if (collectible != null && m_collectibleInInventory == ECollectibleType.None)
        {
            if (m_currentExtractingCollectible == collectible)
            {
                m_currentExtractingCollectible = null;
            }
            m_currentActionDuration = EXTRACTION_DURATION;
            m_isInExtraction = false;
        }

        var camp = collision.GetComponent<Camp>();
        if (camp != null && m_collectibleInInventory != ECollectibleType.None)
        {
            m_isInDepot = false;
            m_currentActionDuration = DEPOSIT_DURATION;
        }
    }

	public void FindInitialChunk()
	{
		//Add middle chunk as starting position
		m_initialChunk = new Vector2Int(m_orchestrator.nbOfChunckInLine / 2, m_orchestrator.nbOfChunckInLine / 2);
		m_workerPath.Add(m_initialChunk);
		m_orchestrator.m_chunkList[m_initialChunk.x, m_initialChunk.y] = true;

		m_currentExploreDir = AssignStartDirection();
	}

	private ExploringDirection AssignStartDirection()
	{
		Vector2Int nextChunk;
		
		for (int i = 0; i < (int)ExploringDirection.Count; i++)
		{
			nextChunk = GetDirectionValue((ExploringDirection)i);
			nextChunk += m_initialChunk;
			if (m_orchestrator.m_chunkList[nextChunk.x, nextChunk.y] == false)
			{
				return (ExploringDirection)i;
			}
		}
		//If everithing Failed, send Up
		return (ExploringDirection)0;
	}

	public void AssignRessource(Collectible ressource)
	{
		assignedRessource = ressource;

		m_orchestrator.m_chunkList[currentChunk.x, currentChunk.y] = false;

		//Set chunk from planned path as not visited
		for (int i = 0; i< m_workerPath.Count; i++)
		{
			print(transform.name + " #: " + i);
			m_orchestrator.m_chunkList[m_workerPath[i].x, m_workerPath[i].y] = false;
		}
		
	}

	public void AssignCamp(Vector2 campPos)
	{
		needToSpawnCamp = true;
		campToSpawnPos = campPos;
	}

	public void FindNextChunk()
	{
		Vector2Int lastChunk = m_workerPath.LastOrDefault();

		//Transform worker direction in a vector2 to navigate the 2D Array
		Vector2Int directionValues;

		//Try if possible to go to next direction
		//directionValues = GetDirectionValue(m_currentExploreDir + 1);	
		//directionValues += lastChunk;

		bool isOutOfBound = true;
		bool isNewDirectionWorking = false;

		for (int i = 0; i < 4; i++)
		{
			//Manage if the enum get out of bound
			int newDirection = (int)m_currentExploreDir + 1 - i;
			if(newDirection < 0)
			{
				newDirection = (int)ExploringDirection.Count -1;
			}
			if (newDirection >= (int)ExploringDirection.Count)
			{
				newDirection = 0;
			}

			directionValues = GetDirectionValue((ExploringDirection)newDirection);
			directionValues += lastChunk;

			if (IsStillInRange(directionValues))
			{
				if (m_orchestrator.m_chunkList[directionValues.x, directionValues.y] == false)
				{
					isNewDirectionWorking = true;
					//The chunk in NOT in any workers path so we can add it
					nextChunk = new Vector2Int(directionValues.x, directionValues.y);
					m_workerPath.Add(nextChunk);

					//Update the direction, and reset to UP if all direction has been checked
					m_currentExploreDir = (ExploringDirection)newDirection;
					if (m_currentExploreDir >= ExploringDirection.Count)
					{
						m_currentExploreDir = 0;
					}
					isOutOfBound = false;

					i = 5;

				}
			}
		}

		if(isNewDirectionWorking == false)
		{
			//Since we can't turn into a free chunk, continue in the same direction
			directionValues = GetDirectionValue(m_currentExploreDir);
			directionValues += lastChunk;
			if (IsStillInRange(directionValues))
			{
				nextChunk = new Vector2Int(directionValues.x, directionValues.y);
				m_workerPath.Add(nextChunk);

				isOutOfBound = false;
			}
		}
	

		if (!isOutOfBound)
		{
			//Set chosen Chunck so it won't be chose again.
			m_orchestrator.m_chunkList[nextChunk.x, nextChunk.y] = true;

		}
	}

	public bool IsStillInRange(Vector2Int directionValues)
	{
		if (directionValues.x < (m_orchestrator.nbOfChunckInLine) && directionValues.x > 0)
		{
			if (directionValues.y < (m_orchestrator.nbOfChunckInLine) && directionValues.y > 0)
			{
				return true;
			}
		}
		return false;
	}

	public Vector2Int GetDirectionValue(ExploringDirection currentDir)
	{
        if(currentDir == ExploringDirection.Up)
        {
            return new Vector2Int(0, 1);
        }
		if (currentDir == ExploringDirection.Right)
		{
			return new Vector2Int(1, 0);
		}
		if (currentDir == ExploringDirection.Down)
		{
			return new Vector2Int(0, -1);
		}
		if (currentDir == ExploringDirection.Left)
		{
			return new Vector2Int(-1, 0);
		}
		
	    //If other means it's Count so we restart == Up
		return new Vector2Int(0, 1);
		
	}

    public void CheckIfPathIsDone(Vector2Int lastChunk)
    {
		//End the path
		if (lastChunk.x > m_orchestrator.nbOfChunckInLine || lastChunk.x < 0)
		{
			IsPathListFull = true;
		}
		if (lastChunk.y > m_orchestrator.nbOfChunckInLine || lastChunk.y < 0)
		{
			IsPathListFull = true;
		}
	}
    public Vector2 GetChunkPosition()
    {
		//To get the worker's movement speed, the first 2 time don't count cause the worker set his position
		//Only check it once
		if(m_skipFirstSpeedTest == 0)
		{
			Tommy_TeamOrchestrator._Instance.WorkerSpeedByChunk = Time.time - speedTest;
			speedTest = Time.time;
			m_skipFirstSpeedTest--;
			
		}
		else
		{
			//TODO this don't need to be called after speed is set
			speedTest = Time.time;
			m_skipFirstSpeedTest--;
			
		}
		
		//Trail
		Instantiate(m_trailVisualizer, transform.position, Quaternion.identity);

		currentChunk = m_workerPath[0]; //Used to remove it when worker is assign to another task

		Vector2 chunkDif = m_workerPath[0] - m_initialChunk;
		Vector2 wantedChunkPosition = new Vector2(chunkDif.x * VISION_RADIUS, chunkDif.y * VISION_RADIUS);

        m_workerPath.RemoveAt(0);

		return wantedChunkPosition;
	}

	private void GainCollectible()
    {
        m_collectibleInInventory = m_currentExtractingCollectible.Extract();
        m_isInExtraction = false;
        m_currentExtractingCollectible = null;

		//if (Tommy_TeamOrchestrator._Instance.KnownCollectiblesTimers.ContainsKey(assignedRessource))
		//{
			Tommy_TeamOrchestrator._Instance.KnownCollectiblesTimers[assignedRessource] = Time.time;
		print("Time.time: " + Time.time);
		//}


		}

    private void DepositResource()
    {
        Tommy_TeamOrchestrator._Instance.GainResource(m_collectibleInInventory);
        m_collectibleInInventory = ECollectibleType.None;
        m_isInDepot = false;
    }
}