using UnityEditor;
using UnityEngine;

public sealed class CoverPoint
{
    public Vector3 Position;
    public AiAgent Owner;

    public bool HasOwner => Owner != null;

    public CoverPoint(Vector3 position)
    {
        Position = position;
        Owner = null;
    }

    public void Lock(AiAgent newOwner)
    {
        Owner = newOwner;
    }
    
    public void Free()
    {
        Owner = null;
    }
    
    public void OnDrawGizmos()
    {
        Gizmos.DrawSphere(Position, 0.2f);
        
        if (HasOwner)
        {
            Handles.Label(Position, Owner.Name);    
        }
    }
}