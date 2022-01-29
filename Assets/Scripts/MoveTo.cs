using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public sealed class MoveTo : MonoBehaviour
{
    [SerializeField] NavMeshAgent Agent;
    [SerializeField] Transform Target;

    void Start()
    {
        UpdateDestination();
    }

    void OnGUI()
    {
        if (GUILayout.Button(nameof(UpdateDestination)))
        {
            UpdateDestination();
        }
    }

    void UpdateDestination()
    {
        Agent.destination = Target.position;
    }
}
