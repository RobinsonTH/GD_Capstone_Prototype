using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class InputManager
{
    public static PlayerInputActions Actions { get; private set; } = new PlayerInputActions();
    public static PlayerInput Input { get; set; }
    
}
