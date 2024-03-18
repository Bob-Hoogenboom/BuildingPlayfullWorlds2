using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public MoveState moveState;
    public bool playerInRange;

    public override State RunCurrentState()
    {
        return this;
    }
}
