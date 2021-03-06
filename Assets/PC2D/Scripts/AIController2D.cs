﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public enum AIState
{
    Right, Left,CatchupStand,CatchupMoney,CatchupBomb,ThrowBomb,Wait
}

public class AIController2D : CharacterController2D
{
    private List<State<AIController2D>> stateList;
    [SerializeField]
    string stateName;
    State<AIController2D> currentState;
    float speed;
    StandManager breakTargetStand;

    protected override void Start()
    {
        base.Start();
        stateList = new List<State<AIController2D>>();
        stateList.Add(new StateRight(this, 60));
        stateList.Add(new StateLeft(this, 60));
        stateList.Add(new StateCatchupStand(this, 2f));
        stateList.Add(new StateCatchupMoney(this));
        stateList.Add(new StateCatchupBomb(this, 2f));
        stateList.Add(new StateThrowBomb(this, 2f));
        stateList.Add(new StateWait(this, 40));
        speed = 0.7f;
        ChangeState(AIState.CatchupStand);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlayable)
        {
            _motor.normalizedXMovement = 0;
            return;
        }

        if (currentState != null)
        {
            currentState.Execute();
        }
        stateName = currentState.GetType().Name;
    }

    public void ChangeState(AIState state)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = stateList[(int)state];
        currentState.Enter();
    }

    private class StateCatchupStand : State<AIController2D>
    {
        float findRangeY;
        StandManager[] stands;
        StandManager targetStand;

        public StateCatchupStand(AIController2D owner, float rangeY) : base(owner)
        {
            findRangeY = rangeY;
            stands = FindObjectsOfType<StandManager>();
        }

        public override void Enter()
        {
            targetStand = null;
            foreach (StandManager stand in stands)
            {
                if (stand.canCreate
                    && Math.Abs(owner.transform.position.y - stand.transform.position.y) < findRangeY)
                {
                    if (targetStand == null
                        || (stand.transform.position - owner.transform.position).magnitude
                        < (targetStand.transform.position - owner.transform.position).magnitude)
                    {
                        targetStand = stand;
                    }
                }
            }
            if (targetStand == null)
            {
                owner.ChangeState(AIState.CatchupMoney);
            }
        }

        public override void Execute()
        {
            if (targetStand.canCreate
                && Math.Abs(owner.transform.position.y - targetStand.transform.position.y) < findRangeY)
            {
                if (owner.owner.aroundStand&&owner.owner.aroundStand.canCreate)
                {
                    owner._motor.normalizedXMovement = 0;
                    owner.owner.aroundStand.CreateStand(owner.owner);
                    owner.ChangeState(AIState.Wait);
                }
                else if (owner.transform.position.x < targetStand.transform.position.x)
                {
                    owner._motor.normalizedXMovement = owner.speed;
                }
                else
                {
                    owner._motor.normalizedXMovement = -owner.speed;
                }
            }
            else
            {
                owner.ChangeState(AIState.Wait);
            }
        }
    }

    private class StateCatchupMoney : State<AIController2D>
    {
        Money targetMoney;

        public StateCatchupMoney(AIController2D owner) : base(owner) { }

        public override void Enter()
        {
            targetMoney = null;
            foreach (Money money in FindObjectsOfType<Money>())
            {
                if (money.colorID == -1 || money.colorID == (int)owner.owner.teamColor)
                {
                    if (targetMoney == null
                           || (money.transform.position - owner.transform.position).magnitude
                           < (targetMoney.transform.position - owner.transform.position).magnitude)
                    {
                        targetMoney = money;
                    }
                }
            }
            if (targetMoney == null)
            {
                owner.ChangeState(AIState.CatchupBomb);
            }
        }

        public override void Execute()
        {
            if (targetMoney)
            {
                if (Mathf.Abs(owner.transform.position.x - targetMoney.transform.position.x)<0.5f)
                {
                    owner._motor.normalizedXMovement = 0;
                    owner._motor.Jump();
                    Debug.Log("Jump");
                }
                else if (owner.transform.position.x < targetMoney.transform.position.x)
                {
                    owner._motor.normalizedXMovement = owner.speed;
                }
                else
                {
                    owner._motor.normalizedXMovement = -owner.speed;
                }
            }
            else
            {
                owner.ChangeState(AIState.Wait);
            }

        }
    }

    private class StateCatchupBomb : State<AIController2D>
    {
        float findRangeY;
        Bomb targetBomb;

        public StateCatchupBomb(AIController2D owner, float rangeY) : base(owner)
        {
            findRangeY = rangeY;
        }

        public override void Enter()
        {
            targetBomb = null;
            foreach (Bomb bomb in FindObjectsOfType<Bomb>())
            {
                if (Math.Abs(owner.transform.position.y - bomb.transform.position.y) < findRangeY)
                {
                    if (targetBomb == null
                           || (bomb.transform.position - owner.transform.position).magnitude
                           < (targetBomb.transform.position - owner.transform.position).magnitude)
                    {
                        targetBomb = bomb;
                    }
                }
            }
            if (targetBomb == null)
            {
                owner.ChangeState(AIState.Wait);
            }
        }

        public override void Execute()
        {
            if (targetBomb)
            {
                if (owner.owner.aroundItem)
                {
                    owner._motor.normalizedXMovement = 0;
                    owner.owner.aroundItem.PickUpReaction(owner.owner);
                    owner.ChangeState(AIState.ThrowBomb);
                }
                else if (owner.transform.position.x < targetBomb.transform.position.x)
                {
                    owner._motor.normalizedXMovement = owner.speed;
                }
                else
                {
                    owner._motor.normalizedXMovement = -owner.speed;
                }
            }
            else
            {
                owner.ChangeState(AIState.Wait);
            }
        }
    }
    
    private class StateThrowBomb : State<AIController2D>
    {
        float findRangeY;
        StandManager targetStand;

        public StateThrowBomb(AIController2D owner, float rangeY) : base(owner)
        {
            findRangeY = rangeY;
        }

        public override void Enter()
        {
            targetStand = null;
            foreach (StandManager stand in FindObjectsOfType<StandManager>())
            {
                if (stand.owner&&stand.owner!=owner.owner
                    && Math.Abs(owner.transform.position.y - stand.transform.position.y) < findRangeY)
                {
                    if (targetStand == null
                        || (stand.transform.position - owner.transform.position).magnitude
                        < (targetStand.transform.position - owner.transform.position).magnitude)
                    {
                        targetStand = stand;
                    }
                }
            }
            if (targetStand == null)
            {
                owner.ChangeState(AIState.Wait);
            }
        }

        public override void Execute()
        {
            if (owner.owner.havingItem
                &&targetStand.owner
                && Math.Abs(owner.transform.position.y - targetStand.transform.position.y) < findRangeY)
            {
                if (Mathf.Abs(owner.transform.position.x - targetStand.transform.position.x) < 8f)
                {
                    owner._motor.normalizedXMovement = 0;
                    owner.anim._animator.Play("throw");
                    owner.ChangeState(AIState.Wait);
                }
                else if (owner.transform.position.x < targetStand.transform.position.x)
                {
                    owner._motor.normalizedXMovement = owner.speed;
                }
                else
                {
                    owner._motor.normalizedXMovement = -owner.speed;
                }
            }
            else
            {
                owner.ChangeState(AIState.Wait);
            }
        }
    }

    private class StateWait : State<AIController2D>
    {
        Counter counter;
        
        public StateWait(AIController2D owner,int waitFrame) : base(owner)
        {
            counter = new Counter(waitFrame);
        }

        public override void Enter()
        {
            counter.Initialize();
        }

        public override void Execute()
        {
            if (counter.Count())
            {
                owner.ChangeState(AIState.CatchupStand);
            }
        }
    }

    private class StateRight : State<AIController2D>
    {
        Counter counter;

        public StateRight(AIController2D owner, int moveFrame) : base(owner)
        {
            counter = new Counter(moveFrame);
        }

        public override void Enter()
        {
            owner._motor.normalizedXMovement = owner.speed;
            counter.Initialize();
        }

        public override void Execute()
        {
            if (counter.Count())
            {
                owner._motor.normalizedXMovement = 0;
                owner.ChangeState(AIState.Left);
            }
        }
    }

    private class StateLeft : State<AIController2D>
    {
        Counter counter;

        public StateLeft(AIController2D owner, int moveFrame) : base(owner)
        {
            counter = new Counter(moveFrame);
        }

        public override void Enter()
        {
            owner._motor.normalizedXMovement = -owner.speed;
            counter.Initialize();
        }

        public override void Execute()
        {
            if (counter.Count())
            {
                owner._motor.normalizedXMovement = 0;
                owner.ChangeState(AIState.Right);
            }
        }
    }
}