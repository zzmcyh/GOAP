using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SRAI
{
    public interface ITrigger
    {
        bool IsTrigger { get; set; }

        void FrameFun();

    }


    public abstract class TriggerBase<TAction, TGoal> : ITrigger
    {
        public abstract bool IsTrigger { get; set; }
        private IAgent<TAction, TGoal> _agent;
        private IState _effects; //该触发器触发了，就会产生什么影响
        public TriggerBase(IAgent<TAction, TGoal> agent)
        {
            _agent = agent;
            _effects = InitEffects();
        }

        protected abstract IState InitEffects();

        public void FrameFun()
        {
            if (IsTrigger)
            {
                _agent.AgentSate.Set(_effects);
            }
        }
    }

}

