using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerRunningState : PlayerBaseState
{
    public PlayerRunningState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubstate();
    }

    public override void CheckSwitchStates()
    {
        // if player is grounded and jump is pressed, switch to jump state
        if (Context.IsJumpPressed && !Context.RequireNewJump)
        {
            SwitchState(Factory.Jump());
        }
        // if player is grounded and slide is pressed, swithc to slide state
        if (Context.IsSlidingPressed)
        {
            SwitchState(Factory.Slide());
        }

        // if player hits an obstacle
        if (Context.IsDied)
        {
            SwitchState(Factory.Die());
        }
    }

    public override void EnterState()
    {
        // set up animators
    }

    public override void ExitState()
    {
    }

    public override void InitializeSubstate()
    {
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }
}
