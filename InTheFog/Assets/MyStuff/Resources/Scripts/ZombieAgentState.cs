using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAgentState : MonoBehaviour
{
    public enum State
    {
        IDLE = 0,
        WANDERING=1,
        CHASING=2
    }

    private State currentState = State.IDLE;

    public ZombieAgentState(State startState)
    {
        currentState = startState;
    }

    public void ChangeState(int state)
    {
        currentState = (State)state;
    }

    public State GetCurrentState()
    {
        return currentState;
    }

    public int GetCurrentStateInt()
    {
        return (int) currentState;
    }


}
