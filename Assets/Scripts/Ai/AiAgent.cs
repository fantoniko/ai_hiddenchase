using System;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class AiAgent : MonoBehaviour
{
    const float Epsilon = 0.01f;
    
    [SerializeField] NavMeshAgent NavMeshAgent;

    public string Name { get; private set; }
    public CoverPoint CurrentPoint { get; private set; }
    
    public Vector3 Position => transform.position;

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

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Handles.Label(Position, Name, GUIStyle.none);
    }
#endif
}