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
    public InputAction counterAttack;

    private void Awake()
    {
        actions = new PlayerInputSystem();

        move = actions.Player.Move;
        jump = actions.Player.Jump;
        attack = actions.Player.ATK;
        dash = actions.Player.Dash;
        counterAttack = actions.Player.CounterAttack;
    }

    private void OnEnable()
    {
        move.Enable();
        jump.Enable();
        attack.Enable();
        dash.Enable();
        counterAttack.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        attack.Disable();
        dash.Disable();
        counterAttack.Disable();
    }

    #region Input Events
    public void RegisterToJump(Action<InputAction.CallbackContext> action) => jump.performed += action;
    public void UnRegisterToJump(Action<InputAction.CallbackContext> action) => jump.performed -= action;

    public void RegisterToAttack(Action<InputAction.CallbackContext> action) => attack.performed += action;
    public void UnRegisterToAttack(Action<InputAction.CallbackContext> action) => attack.performed -= action;

    public void RegisterToDash(Action<InputAction.CallbackContext> action) => dash.performed += action;
    public void UnRegisterToDash(Action<InputAction.CallbackContext> action) => dash.performed -= action;

    public void RegisterToCounterAttack(Action<InputAction.CallbackContext> action) => counterAttack.performed += action;
    public void UnRegisterToCounterAttack(Action<InputAction.CallbackContext> action) => counterAttack.performed -= action;
    #endregion

    public Vector2 GetMovingReading() => move.ReadValue<Vector2>();
}
