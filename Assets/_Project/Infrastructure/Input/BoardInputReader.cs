using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class BoardInputReader : MonoBehaviour
{
    public event Action<Vector2> PointerDown;

    private void Update()
    {
        if (Pointer.current == null)
            return;

        if (Pointer.current.press.wasPressedThisFrame)
        {
            PointerDown?.Invoke(
                Pointer.current.position.ReadValue());
        }
    }

}
