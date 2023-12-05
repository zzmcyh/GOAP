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
         IActionManager<TAction> ActionManager { get; }
         IGoalManager<TGoal> GoalManager { get; }


    }


    public abstract class AgentBase< TAction, TGoal> : IAgent<TAction, TGoal>
    {
        public IState AgentSate { get; private set; }
        public  IMap<TAction, TGoal> Map { get; private set; }

        public IActionManager<TAction> ActionManager { get; set; }
        public IGoalManager<TGoal> GoalManager { get; set; }


        public AgentBase()
        {
            DebugBase.Instance = InitDebugBase();
            Map = InitMap();
            ActionManager = InitIActionManager();
            GoalManager = InitIGoalManager();

            AgentSate = new State();
            AgentSate.AddStateChangeListener(UpdateDate);

          

        }

        protected abstract DebugBase InitDebugBase();
        protected abstract IMap<TAction, TGolal> InitMap();
        protected abstract IActionManager<TAction> InitIActionManager();
        protected abstract IGoalManager<TGolal> InitIGoalManager();

        public void UpdateDate()
        {
            
        }

        public void FrameFun()
        {
            
        }
    }
}
