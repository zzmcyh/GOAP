﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SRAI
{
    public interface IAction<TAction>
    {
        TAction Label { get; }
        int Cost { get; }//花费
        int Priority { get; }//优先级。如果花费相同就看谁的优先级高
        bool CanInterruptiblePlan { get; }  //是否可以打断计划
        IState Preconditions { get; }//先决条件
        IState Effect { get; } //完成后对结果的影响
        bool VerifyPreconditions();//验证先决条件是否满足
    }


    public abstract class ActionBase<TAction, TState, TGoal> : IAction<TAction>
    {
        public abstract TAction Label { get; }

        public abstract int Cost { get; }

        public abstract int Priority { get; }

        public abstract bool CanInterruptiblePlan { get; }

        public IState Preconditions { get; private set; }

        public IState Effect { get; }


        private IAgent<TAction, TGoal> _agent;

        public ActionBase(IAgent< TAction, TGoal> agent)
        {
            Preconditions = InitPreconditions();
            Effect = InitEffects();
            _agent = agent;
        }


        public abstract IState InitPreconditions();
        public abstract IState InitEffects();

        public bool VerifyPreconditions()
        {
            return _agent.AgentSate.ContainState(Preconditions);
        }



    }
}