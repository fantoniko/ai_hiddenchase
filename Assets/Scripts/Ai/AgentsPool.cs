using System.Collections.Generic;

public class AgentsPool
{
    List<AiAgent> AvailableAgents;

    public AgentsPool(List<AiAgent> initialPool)
    {
        AvailableAgents = new List<AiAgent>(initialPool);
    }
    
    public void FillAvailableAgents(List<AiAgent> initialPool)
    {
        for (int i = 0; i < AvailableAgents.Count; i++)
        {
            AvailableAgents[i] = initialPool[i];
        }
    }

    public void HandlePoint(CoverPoint point)
    {
        AiAgent agentToRemove;
        if (!point.HasOwner)
        {
            // Set closest agent and remove him from pool
            agentToRemove = SetClosestAvailableAgent(point);
        }
        else
        {
            // If good point already has owner, then just removing him from pool
            agentToRemove = point.Owner;
        }
        RemoveAvailableAgent(agentToRemove);
    }

    public bool AnyAvailableAgents()
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
    
    AiAgent SetClosestAvailableAgent(CoverPoint point)
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
        return closestAgent;
    }
}