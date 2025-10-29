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

    public void WallSlideJump(InputAction.CallbackContext context) 
    {
        player.stateMachine.ChangeState(player.wallJumpState);
        return;
    }

    public void GroundAttack(InputAction.CallbackContext context) => player.stateMachine.ChangeState(player.ATK1State);

    public void GroundJump(InputAction.CallbackContext context)
    {
        if (player.isGroundDetected())
            player.stateMachine.ChangeState(player.jumpState);
    }

    public void Dash(InputAction.CallbackContext context) => player.Dash();

    public void CounterAttack(InputAction.CallbackContext context) => player.stateMachine.ChangeState(player.counterAttackState);
}
