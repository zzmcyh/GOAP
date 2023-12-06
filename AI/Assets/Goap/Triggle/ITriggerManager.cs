using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SRAI
{
    public interface ITriggerManager
    {
        void FrameFun();
    }
    public abstract class TriggerManagerBase<TAction, TGoal> : ITriggerManager
    {
        private HashSet<ITrigger> _triggers;

        private IAgent<TAction, TGoal> _agent;

        public TriggerManagerBase(IAgent<TAction, TGoal> agent)
        {
            _agent = agent;
            _triggers = new HashSet<ITrigger>();
            InitTrigger();
        }

        protected abstract void InitTrigger();


        protected void AddTrigger(ITrigger trigger)
        {
            _triggers.Add(trigger);
        }



        public void FrameFun()
        {
            foreach (var item in _triggers)
            {
                item.FrameFun();
            }
        }

       
    }
}

