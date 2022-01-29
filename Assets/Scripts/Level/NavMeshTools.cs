using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;

[Flags]
enum NavigationArea : int
{
    Walkable = 1
}
    
public static class NavMeshTools
{
    const float MaxDistanceOutsideObstacle = 0.1f;
    
    public static bool IsPositionReachable(Vector3 position)
    {
        return NavMesh.SamplePosition(position, out NavMeshHit hit, MaxDistanceOutsideObstacle, (int)NavigationArea.Walkable);
    }
}