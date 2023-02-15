using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerBaseState
{
    public PlayerDeathState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubstate();
    }

    public override void CheckSwitchStates()
    {
    }

    public override void EnterState()
    {
    }

    public override void ExitState()
    {
        Context.IsDied = false;
    }

    public override void InitializeSubstate()
    {
        Context.Animator.Play("Death");
        Context.RunSpeed = 0;
    }

    public override void UpdateState()
    {
        
    }
}
