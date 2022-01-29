using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public sealed class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody Rigidboby;
    [SerializeField] float MaxSpeed;
    
    [Header("Input")]
    [SerializeField] KeyCode KeyUp;
    [SerializeField] KeyCode KeyDown;
    [SerializeField] KeyCode KeyRight;
    [SerializeField] KeyCode KeyLeft;
    
    [Space]
    [SerializeField] KeyCode KeyUpAlternate;
    [SerializeField] KeyCode KeyDownAlternate;
    [SerializeField] KeyCode KeyRightAlternate;
    [SerializeField] KeyCode KeyLeftAlternate;

    void FixedUpdate()
    {
        UpdateVelocity();
    }

    void UpdateVelocity()
    {
        var newVelocity = GetHorizontalVelocityByInput();
        SetHorizontalVelocity(newVelocity);
    }

    Vector2 GetHorizontalVelocityByInput()
    {
        Vector2 newVelocity = Vector2.zero;
        if (GetKey(KeyUp) || GetKey(KeyUpAlternate))
        {
            newVelocity.y += 1f;
        }
        if (GetKey(KeyDown) || GetKey(KeyDownAlternate))
        {
            newVelocity.y += -1f;
        }
        if (GetKey(KeyRight) || GetKey(KeyRightAlternate))
        {
            newVelocity.x += 1f;
        }
        if (GetKey(KeyLeft) || GetKey(KeyLeftAlternate))
        {
            newVelocity.x += -1f;
        }
        return newVelocity.normalized * MaxSpeed;
    }

    void SetHorizontalVelocity(Vector2 newVelocity)
    {
        Rigidboby.velocity = new Vector3(newVelocity.x, Rigidboby.velocity.y, newVelocity.y);
    }

    bool GetKey(KeyCode key)
    {
        return Input.GetKey(key);
    }
}
