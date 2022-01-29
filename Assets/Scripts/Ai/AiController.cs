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
        Agents = new List<AiAgent>();
        for (int i = 0; i < AgentsCount; i++)
        {
            var spawnPosition = SpawnInitialPosition + SpawnOffset * i;
            var agent = Instantiate(AgentPrefab, spawnPosition, Quaternion.identity);
            agent.Init($"Agent-{i.ToString()}");
            Agents.Add(agent);
        }
    }

    void UpdateAgents()
    {
        foreach (var agent in Agents)
        {
            if (!agent.HasPoint || !Covers.IsCovered(agent.CurrentPoint.Position, TargetToFollow.position))
            {
                var newDestination = Covers.GetClosest(agent.Position, TargetToFollow.position);
                agent.SetDestination(newDestination);
            }
        }
    }
    
}
