using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlidingState : PlayerBaseState
{
    private float _colliderHeight;
    private Vector3 _colliderCenterPosition;
    public PlayerSlidingState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) 
    {
        IsRootState = true;
        InitializeSubstate();
    }

    public override void InitializeSubstate()
    {
        Context.Animator.Play("Slide");
    }

    public override void CheckSwitchStates()
    {
        if (Context.IsDied)
        {
            SwitchState(Factory.Die());
        }

        if (Context.CharacterController.isGrounded)
        {
            SwitchState(Factory.Run());
        }

        if (Context.IsJumpPressed)
        {
            SwitchState(Factory.Jump());
        }
    }

    public override void EnterState()
    {
        _colliderHeight = Context.CharacterController.height;
        _colliderCenterPosition = Context.CharacterController.center;
    }

    public override void ExitState()
    {
        Context.Animator.StopPlayback();
    }

    public override void UpdateState()
    {
        if (Context.IsSlidingPressed)
        {
            // change collider
            Context.CharacterController.height = Context.ColliderHeight;
            Context.CharacterController.center = Context.ColliderCenterPosition;

            // wait time
            Context.Coroutine = Context.StartCoroutine(DoSlide());

            Context.IsSlidingPressed = false;

            CheckSwitchStates();
        }
    }

    private IEnumerator DoSlide()
    {
        yield return new WaitForSeconds(Context.SlideTime);

        // return base value 
        Context.CharacterController.height = _colliderHeight;
        Context.CharacterController.center = _colliderCenterPosition;
    }
}
