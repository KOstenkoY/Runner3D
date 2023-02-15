using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    public PlayerJumpingState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) 
    {
        IsRootState = true;
        InitializeSubstate();
    }

    public override void CheckSwitchStates()
    {
        if (Context.IsDied)
        {
            SwitchState(Factory.Die());
        }

        if (Context.IsSlidingPressed)
        {
            SwitchState(Factory.Slide());
        }

        SwitchState(Factory.Run());
    }

    public override void EnterState()
    {
    }

    public override void ExitState()
    {
        Context.Animator.StopPlayback();
        Context.RequireNewJump = true;
    }

    public override void InitializeSubstate()
    {
        Context.Animator.Play("Jump");
    }

    public override void UpdateState()
    {
        if (Context.CharacterController.isGrounded)
        {
            if (Context.IsJumpPressed)
            {
                Context.VerticalSpeed = Context.JumpForce;
                Context.IsJumpPressed = false;
            }
            else
            {
                Context.VerticalSpeed = Context.MinFall;
            }
        }
        else
        {
            Context.VerticalSpeed += Context.Gravity * Time.deltaTime;
            if (Context.VerticalSpeed < Context.TerminalVelocity)
            {
                Context.VerticalSpeed = Context.TerminalVelocity;
                CheckSwitchStates();
            }
        }
    }
}
