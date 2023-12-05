using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SRAI
{
    public interface IAgent<TAction,TGoal>
      
        
    {
        IState AgentSate { get; }

        IMap<TAction, TGoal> Map { get; }

        void UpdateDate();

        void FrameFun();
    }


    public abstract class AgentBase< TAction, TGolal> : IAgent<TAction, TGolal>
    {
        public IState AgentSate { get; private set; }
        public  IMap<TAction, TGolal> Map { get; private set; }

        public AgentBase()
        {
            DebugBase.Instance = InitDebugBase();
            Map = InitMap();
            AgentSate = new State();
            AgentSate.AddStateChangeListener(UpdateDate);

          

        }

        protected abstract DebugBase InitDebugBase();
        protected abstract IMap<TAction, TGolal> InitMap();
        public void UpdateDate()
        {
            
        }

        public void FrameFun()
        {
            
        }
    }
}
