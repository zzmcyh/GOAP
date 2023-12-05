using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SRAI
{
    public interface IAgent<TState>
    {
        IState<TState> AgentSate { get; }

        void UpdateDate();

        void FrameFun();
    }


    public abstract class AgentBase<TState> : IAgent<TState>
    {
        public IState<TState> AgentSate { get; private set; }
        public AgentBase()
        {
            DebugBase.Instance = InitDebugBase();
            AgentSate = new State<TState>();
            AgentSate.AddStateChangeListener(UpdateDate);
        }

        protected abstract DebugBase InitDebugBase();

        public void UpdateDate()
        {
            
        }

        public void FrameFun()
        {
            
        }
    }
}
