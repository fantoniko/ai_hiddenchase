using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour
{
    readonly Vector3 SpawnInitialPosition = new Vector3(2f, 0f, 2f);
    readonly Vector3 SpawnOffset = new Vector3(2f, 0f, 0f);
    
    [SerializeField] AiAgent AgentPrefab;
    [SerializeField] int AgentsCount;
    
    [Tooltip("Period for AI ticks to reduce FixedUpdate load with a large number of agents and obstacles.")]
    [SerializeField, Range(1, 10)] int FixedUpdatePeriod;

    int PeriodCounter;
    
    CoverController Covers;
    Transform TargetToFollow;
    List<AiAgent> Agents;

    AgentsPool Pool;

    public void Init(CoverController cover, Transform target)
    {
        Covers = cover;
        TargetToFollow = target;
        PeriodCounter = 0;
        SpawnAgents();
        Pool = new AgentsPool(Agents);
    }

    public void OnFixedUpdate()
    {
        UpdateAiPeriod();
    }
    
    void SpawnAgents()
    {
        Agents = new List<AiAgent>(AgentsCount);
        for (int i = 0; i < AgentsCount; i++)
        {
            var spawnPosition = SpawnInitialPosition + SpawnOffset * i;
            var agent = Instantiate(AgentPrefab, spawnPosition, Quaternion.identity);
            agent.Init($"Agent-{i.ToString()}");
            Agents.Add(agent);
        }
    }

    void UpdateAiPeriod()
    {
        PeriodCounter++;
        if (PeriodCounter == FixedUpdatePeriod)
        {
            UpdateAgents();
            PeriodCounter = 0;
        }
    }

    void UpdateAgents()
    {
        // Filling Agents pool for current AI tick
        Pool.FillAvailableAgents(Agents);
        
        // Sorting cover points by distance to target to start with most appropriate
        var sortedPoints = Covers.GetSortedPoints(TargetToFollow.position);

        // Distributing points to agents
        for (int i = 0; i < sortedPoints.Count; i++)
        {
            var point = sortedPoints[i];
            if (!IsCovered(point))
            {
                // Skip bad points with straight line of sight to target
                continue;
            }

            // Check point to stay on it or set closest agent 
            Pool.HandlePoint(point);
            
            if (!Pool.AnyAvailableAgents())
            {
                // If all agents are set, then finishing
                break;
            }
        }
    }

    bool IsCovered(CoverPoint point)
    {
        return Covers.IsCovered(point.Position, TargetToFollow.position);
    }
    
}
