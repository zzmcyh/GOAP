using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SRAI
{
    public interface IAgent<TAction, TGoal>
    {
        IState AgentSate { get; }

        IMap<TAction, TGoal> Map { get; }

        void UpdateDate();

        void FrameFun();
        IActionManager<TAction> ActionManager { get; }
        IGoalManager<TGoal> GoalManager { get; }

        IPerformer Performer { get;  }
    }


    public abstract class AgentBase< TAction, TGoal> : IAgent<TAction, TGoal>
    {
        public IState AgentSate { get; private set; }
        public  IMap<TAction, TGoal> Map { get; private set; }
        public IActionManager<TAction> ActionManager { get; set; }
        public IGoalManager<TGoal> GoalManager { get; set; }
        public IPerformer Performer { get; private set; }
        private ITriggerManager _triggerManager;
        public AgentBase()
        {
            DebugBase.Instance = InitDebugBase();
            Map = InitMap();
            ActionManager = InitIActionManager();
            GoalManager = InitIGoalManager();
            AgentSate = InitIState();
            AgentSate.AddStateChangeListener(UpdateDate);
            Performer = new Performer<TAction, TGoal>(this);
            _triggerManager = InitITriggerManager();

            JudgeException(Map,"Map");
            JudgeException(ActionManager, "ActionManager");
            JudgeException(GoalManager, "GoalManager");
            JudgeException(_triggerManager, "_triggerManager");

        }


        private void JudgeException(object obj,string name) 
        {
            if (obj == null)
            {
                DebugMsg.LogError($"代理中{name}对象为空，请在代理子类中初始化对象");
            }
        }

        protected abstract DebugBase InitDebugBase();
        protected abstract IMap<TAction, TGoal> InitMap();
        protected abstract IActionManager<TAction> InitIActionManager();
        protected abstract IGoalManager<TGoal> InitIGoalManager();
        protected abstract ITriggerManager InitITriggerManager();
        protected abstract IState InitIState();
        public void UpdateDate()
        {
            if (ActionManager!=null)
            {
                ActionManager.UpdateData();
            }
            if (GoalManager != null)
            {
                GoalManager.UpdateData();
            }
            if (Performer != null)
            {
                Performer.UpdateData();
            }

        }
        public void FrameFun()
        {
            if (_triggerManager != null)
            {
                _triggerManager.FrameFun();
            }

            if (ActionManager != null)
            {
                ActionManager.FrameFun();
            }
        }
    }
}
