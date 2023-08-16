using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controls : MonoBehaviour
{
    [SerializeField] private LayerMask _baloonsLayerMask;
    public void Click(InputAction.CallbackContext context)
    {
        Vector2 WorldPoint = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
        RaycastHit2D hit = Physics2D.Raycast(WorldPoint, Vector2.zero, _baloonsLayerMask);
        if (hit.collider != null)
        {
            if(hit.collider.TryGetComponent(out Baloon baloon))
            {
                baloon.Destroyed(true);
            }
        }
    }
}
