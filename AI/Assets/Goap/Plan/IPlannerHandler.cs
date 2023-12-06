using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SRAI
{
    public interface IPlannerHandler<TAction>
    {
        bool IsComplete { get; }
        void Init(IActionManager<TAction> actionManager,Queue<IActionHandler<TAction>> plan);       

        void StartPlan();

        void NextAction();

        void Interruptible();//通知被打断了

        void AddCompleteCallBack(System.Action callback);
    }

    public class PlannerHandler<TAction> : IPlannerHandler<TAction>
    {
        public bool IsComplete {
            get
            {
                if (_isInterruptible)
                    return true;
                if (_plan == null)
                    return true;
                if (_currentActionHandler == null)
                {
                    return _plan.Count == 0;
                }
                else
                {
                    return _currentActionHandler.isComplete && _plan.Count == 0;
                }

                
            }
        }

        private Queue<IActionHandler<TAction>> _plan;
        private IActionManager<TAction> _actionManager;

        private System.Action _onComplete;

        private bool _isInterruptible;

        private IActionHandler<TAction> _currentActionHandler;
        public void Init(IActionManager<TAction> actionManager,Queue<IActionHandler<TAction>> plan)
        {
            _isInterruptible = false;
            _plan = plan;
            _currentActionHandler = null;
            _onComplete = null;
            _actionManager = actionManager;
        }

        public void Interruptible()
        {
            _isInterruptible = true;
        }

        public void NextAction()
        {
            if (IsComplete)
            {
                _onComplete?.Invoke();
            }
            else
            {
                _currentActionHandler = _plan.Dequeue();
                _actionManager.ChangeCurrentAction(_currentActionHandler.Label);
            }
        }

        public void StartPlan()
        {
            NextAction();
        }

        public void AddCompleteCallBack(Action callback)
        {
            _onComplete = callback;
        }
    }

}

