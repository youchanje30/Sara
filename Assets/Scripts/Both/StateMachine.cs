public class StateMachine<T> where T : class
{
    private T           owner;
    private State<T>    curState;
    private State<T>    prevState;

    public void Setup(T entity, State<T> entryState)
    {
        owner = entity;
        curState = null;
        prevState = null;
        
        
    }

    public void FrameUpdate()
	{
		if ( curState != null )
		{
			curState.FrameUpdate(owner);
		}
	}

    public void PhysicsUpdate()
	{
		if ( curState != null )
		{
			curState.PhysicsUpdate(owner);
		}
	}

    public void ChangeState(State<T> newState)
	{
		if ( newState == null ) return;

        if ( curState != null )
		{
			prevState = curState;

			curState.Exit(owner);
		}

		curState = newState;
		curState.Enter(owner);
	}

    
	public void RevertToPreviousState()
	{
		ChangeState(prevState);
	}

}
