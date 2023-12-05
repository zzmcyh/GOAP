using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SRAI
{
    /// <summary>
    /// 处理行为 
    /// </summary>
    /// <typeparam name="TAction"></typeparam>
    public interface IActionHandler<TAction>:IFSMState<TAction>
    {
        IAction<TAction> Action { get; }
        TAction Label { get; }
        bool isComplete { get; }
        bool CanPerformAction { get; }//当前是否可以播放该动作
        void AddFinishCallBack(System.Action onFinishAction);

        
    }

    public abstract class ActionHandlerBase<TAction, TGolal> : IActionHandler<TAction>
    {
        public  IAction<TAction> Action { get; private set; }
        public  TAction Label { get { return Action.Label; } }
        public  bool isComplete { get; private set; }
        public  bool CanPerformAction { get; private set; }


        private Action _onFinishAction;

        IAgent<TAction, TGolal> _agent;

        public ActionHandlerBase(IAgent<TAction, TGolal> agent,IAction<TAction> action)
        {
            if (action == null)
            {
                DebugMsg.LogError("action is null!");
            }
            Action = action;
            isComplete = false;
            CanPerformAction = false;
            _agent = agent;
        }


        public void AddFinishCallBack(Action onFinishAction) 
        {
            _onFinishAction = onFinishAction;
        }


        protected void OnComplete()
        {
            isComplete = true;
            _onFinishAction?.Invoke();
            SetAgentState(Action.Effect);
            SetAgentState(Action.Preconditions.InversionValue());

        }


        private void SetAgentState(IState state)
        {
            _agent.AgentSate.Set(state);
        }

        public virtual void Enter()
        {
            isComplete = false;
        }

        public virtual void Excute()
        {
          
        }

        public virtual void Exit()
        {
          
        }
    }
}