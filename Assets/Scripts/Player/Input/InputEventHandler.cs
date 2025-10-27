using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputEventHandler : MonoBehaviour
{
    Player player;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    public void wallSlideJump(InputAction.CallbackContext context) 
    {
        player.stateMachine.ChangeState(player.wallJumpState);
        return;
    }

    public void groundAttack(InputAction.CallbackContext context) => player.stateMachine.ChangeState(player.ATK1State);

    public void groundJump(InputAction.CallbackContext context)
    {
        if (player.isGroundDetected())
            player.stateMachine.ChangeState(player.jumpState);
    }

    public void Dash(InputAction.CallbackContext context) => player.Dash();
}
