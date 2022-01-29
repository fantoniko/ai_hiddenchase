using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class CoverController : MonoBehaviour
{
    [Flags]
    enum RaycastLayer : int
    {
        Player = 1 << 6,
        Obstacles = 1 << 7
    }

    const float AgentYCenter = 0.5f;
    const float MaxRaycastDistance = 100f;
    
    readonly int RaycastLayerMask = (int)(RaycastLayer.Player | RaycastLayer.Obstacles);
    
    List<CoverPoint> CoverPoints;

    static Vector3 TargetPosition;
    static Comparison<CoverPoint> ComparePoints = (a, b) =>
    {
        var targetPosition = TargetPosition;
        return (int)((a.Position - targetPosition).sqrMagnitude - (b.Position - targetPosition).sqrMagnitude);
    };

    public void Init()
    {
        CoverPoints = new List<CoverPoint>();

        var covers = GameObject.FindObjectsOfType<Cover>();
        foreach (var cover in covers)
        {
            AddCoverPoints(cover);
        }

        Debug.Log($"Created {CoverPoints.Count} cover points");
    }

    void AddCoverPoints(Cover cover)
    {
        foreach (var transform in cover.PointTransformsEnum)
        {
            var pointPosition = transform.position;
            if (NavMeshTools.IsPositionReachable(pointPosition))
            {
                CoverPoints.Add(new CoverPoint(pointPosition));
            }
        }
    }

    public bool IsCovered(Vector3 position, Vector3 coveredFrom)
    {
        var direction = coveredFrom - position;
        position.y += AgentYCenter;
        var ray = new Ray(position, direction);
        var raycast = Physics.Raycast(ray, out RaycastHit hit, MaxRaycastDistance, RaycastLayerMask);
        return !hit.collider || 1 << hit.collider.gameObject.layer != (int)RaycastLayer.Player;
    }

    public List<CoverPoint> GetSortedPoints(Vector3 newTargetPosition)
    {
        SortPoints(newTargetPosition);
        return CoverPoints;
    }
    
    void SortPoints(Vector3 newTargetPosition)
    {
        TargetPosition = newTargetPosition;
        CoverPoints.Sort(ComparePoints);
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (CoverPoints == null)
        {
            return;
        }
        
        foreach (var point in CoverPoints)
        {
            point.OnDrawGizmos();
        }
    }
#endif
}