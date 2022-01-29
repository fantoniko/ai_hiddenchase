using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour
{
    readonly Vector3 SpawnInitialPosition = new Vector3(2f, 0f, 2f);
    readonly Vector3 SpawnOffset = new Vector3(2f, 0f, 0f);
    
    [SerializeField] AiAgent AgentPrefab;
    [SerializeField] int AgentsCount;

    CoverController Covers;
    Transform TargetToFollow;
    List<AiAgent> Agents;
    List<AiAgent> AvailableAgents;

    public void Init(CoverController cover, Transform target)
    {
        Covers = cover;
        TargetToFollow = target;
        SpawnAgents();
    }

    public void OnFixedUpdate()
    {
        UpdateAgents();
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
        AvailableAgents = new List<AiAgent>(Agents);
    }

    void UpdateAgents()
    {
        var availableAgents = FillAvailableAgents();
        var sortedPoints = Covers.GetSortedPoints(TargetToFollow.position);

        for (int i = 0; i < sortedPoints.Count; i++)
        {
            var point = sortedPoints[i];
            if (!IsCovered(point))
            {
                continue;
            }
            
            if (!point.HasOwner)
            {
                SetClosestAvailableAgent(availableAgents, point);
            }
            else
            {
                RemoveAvailableAgent(point.Owner);
            }

            if (!AnyAvailableAgents())
            {
                break;
            }
        }
    }

    bool IsCovered(CoverPoint point)
    {
        return Covers.IsCovered(point.Position, TargetToFollow.position);
    }

    bool AnyAvailableAgents()
    {
        var result = false;
        for (int i = 0; i < AvailableAgents.Count; i++)
        {
            if (AvailableAgents[i])
            {
                result = true;
                break;
            }
        }
        return result;
    }
    
    List<AiAgent> FillAvailableAgents()
    {
        for (int i = 0; i < AvailableAgents.Count; i++)
        {
            AvailableAgents[i] = Agents[i];
        }
        return AvailableAgents;
    }

    void RemoveAvailableAgent(AiAgent agent)
    {
        for (int i = 0; i < AvailableAgents.Count; i++)
        {
            if (AvailableAgents[i] == agent)
            {
                AvailableAgents[i] = null;
                break;
            }
        }
    }

    void SetClosestAvailableAgent(List<AiAgent> availableAgents, CoverPoint point)
    {
        AiAgent closestAgent = null;
        var minSqrDistance = float.MaxValue;
        foreach (var agent in AvailableAgents)
        {
            if (!agent)
            {
                continue;
            }
            
            var sqrDistance = (agent.Position - point.Position).sqrMagnitude;
            if (sqrDistance < minSqrDistance)
            {
                closestAgent = agent;
                minSqrDistance = sqrDistance;
            }
        }
        closestAgent.SetDestination(point);
        
        RemoveAvailableAgent(closestAgent);
    }
}
