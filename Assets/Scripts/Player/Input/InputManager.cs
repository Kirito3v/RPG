using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : InputEventHandler
{
    PlayerInputSystem actions;

    public InputAction move;
    public InputAction jump;
    public InputAction attack;
    public InputAction dash;

    private void Awake()
    {
        actions = new PlayerInputSystem();

        move = actions.Player.Move;
        jump = actions.Player.Jump;
        attack = actions.Player.ATK;
        dash = actions.Player.Dash;
    }

    private void OnEnable()
    {
        move.Enable();
        jump.Enable();
        attack.Enable();
        dash.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        attack.Disable();
        dash.Disable();
    }

    #region Input Events
    public void RegisterToJump(Action<InputAction.CallbackContext> action) => jump.performed += action;
    public void UnRegisterToJump(Action<InputAction.CallbackContext> action) => jump.performed -= action;

    public void RegisterToAttack(Action<InputAction.CallbackContext> action) => attack.performed += action;
    public void UnRegisterToAttack(Action<InputAction.CallbackContext> action) => attack.performed -= action;

    public void RegisterToDash(Action<InputAction.CallbackContext> action) => attack.performed += action;
    public void UnRegisterToDash(Action<InputAction.CallbackContext> action) => attack.performed -= action;
    #endregion

    public Vector2 GetMovingReading() => move.ReadValue<Vector2>();
}
