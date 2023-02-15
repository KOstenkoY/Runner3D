public class PlayerStateFactory
{
    PlayerStateMachine _context;
    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
    }

    public PlayerBaseState Run()
    {
        return new PlayerRunningState(_context, this);
    }

    public PlayerBaseState Jump()
    {
        return new PlayerJumpingState(_context, this);
    }

    public PlayerBaseState Slide()
    {
        return new PlayerSlidingState(_context, this);
    }

    public PlayerDeathState Die()
    {
        return new PlayerDeathState(_context, this);
    }
}
