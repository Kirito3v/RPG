using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputEventHandler : MonoBehaviour
{
    Player player;

    private void Start()
    {
        player = FindAnyObjectByType<Player>();
    }

    public void wallslidejump(InputAction.CallbackContext context) 
    {
        player.stateMachine.ChangeState(player.wallJumpState);
        return;
    }

    public void groundattack(InputAction.CallbackContext context) => player.stateMachine.ChangeState(player.ATK1State);

    public void groundjump(InputAction.CallbackContext context) => player.stateMachine.ChangeState(player.jumpState);

    public void dash(InputAction.CallbackContext context) => player.stateMachine.ChangeState(player.dashState);
}
