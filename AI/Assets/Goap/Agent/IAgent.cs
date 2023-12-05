using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SRAI
{
    public interface IAgent<TState,TAction,TGoal>
    {
        IState<TState> AgentSate { get; }

        IMap<TAction, TGoal, TState> Map { get; }

        void UpdateDate();

        void FrameFun();
    }


    public abstract class AgentBase<TState, TAction, TGolal> : IAgent<TState, TAction, TGolal>
    {
        public IState<TState> AgentSate { get; private set; }
        public  IMap<TAction, TGolal, TState> Map { get; private set; }

        public AgentBase()
        {
            DebugBase.Instance = InitDebugBase();
            Map = InitMap();
            AgentSate = new State<TState>();
            AgentSate.AddStateChangeListener(UpdateDate);

          

        }

        protected abstract DebugBase InitDebugBase();
        protected abstract IMap<TAction, TGolal, TState> InitMap();
        public void UpdateDate()
        {
            
        }

        public void FrameFun()
        {
            
        }
    }
}
