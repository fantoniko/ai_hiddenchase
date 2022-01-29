using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class AiAgent : MonoBehaviour
{
    [SerializeField] NavMeshAgent NavMeshAgent;

    public string Name { get; private set; }
    public CoverPoint CurrentPoint { get; private set; }

    public bool HasPoint => CurrentPoint != null;
    public Vector3 Position => transform.position;

    void OnDrawGizmos()
    {
        Handles.Label(Position, Name, GUIStyle.none);
    }

    public void Init(string name)
    {
        Name = name;
        transform.name = Name;
    }

    public void SetDestination(CoverPoint newPoint)
    {
        if (CurrentPoint == newPoint)
        {
            return;
        }
        
        CurrentPoint?.Free();
        NavMeshAgent.destination = newPoint.Position;
        newPoint.Lock(this);
        CurrentPoint = newPoint;
    }
}