using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SRAI
{
    public interface IAction<TAction, TState>
    {
        TAction Label { get; }
        int Cost { get; }//花费
        int Priority { get; }//优先级。如果花费相同就看谁的优先级高
        bool CanInterruptiblePlan { get; }  //是否可以打断计划
        IState<TState> Preconditions { get; }//先决条件
        IState<TState> Effect { get; } //完成后对结果的影响
        bool VerifyPreconditions();//验证先决条件是否满足
    }


    public abstract class ActionBase<TAction, TState> : IAction<TAction, TState>
    {
        public abstract TAction Label { get; }

        public abstract int Cost { get; }

        public abstract int Priority { get; }

        public abstract bool CanInterruptiblePlan { get; }

        public IState<TState> Preconditions { get; private set; }

        public IState<TState> Effect { get; }


        private IAgent<TState> _agent;

        public ActionBase(IAgent<TState> agent)
        {
            Preconditions = InitPreconditions();
            Effect = InitEffects();
            _agent = agent;
        }


        public abstract IState<TState> InitPreconditions();
        public abstract IState<TState> InitEffects();

        public bool VerifyPreconditions()
        {
            return _agent.AgentSate.ContainState(Preconditions);
        }



    }
}