using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace SRAI
{
    public interface IActionManager<TAction>
    {
        TAction GetDefualtActionLabel();
        bool IsPerformAction { get; set; }
        void AddHander(TAction label);
        void RemoveHandler(TAction label);
        IActionHandler<TAction> GetHandler(TAction label);
        void UpdateData();
        void FrameFun();
        void ChangeCurrentAction(TAction label);
        void AddActionCompleteListener(System.Action complete);
    }



    public abstract class ActionManagerBase<TAction, TGoal> : IActionManager<TAction>
    {
        private Dictionary<TAction, IActionHandler<TAction>> _handlerDic;
        private List<IActionHandler<TAction>> _InterruptibleHandlers;

        private IFSM<TAction> _fsm;
    
        private IAgent<TAction, TGoal> _agent;
        private System.Action _onActionComplete;

        public bool IsPerformAction { get; set; }

        public abstract TAction GetDefualtActionLabel(); 
       

        public ActionManagerBase(IAgent<TAction, TGoal> agent)
        {
            IsPerformAction = false;
            _onActionComplete = null;
            _handlerDic = new Dictionary<TAction, IActionHandler<TAction>>();
            _InterruptibleHandlers = new List<IActionHandler<TAction>>();
            _fsm = new FSM<TAction>();
            _agent = agent;
            InitActionHandlers();
            InitFSM();
            InitInterruptibleHandlers();
        }

        private void InitFSM()
        {
            foreach (var item in _handlerDic)
            {
                _fsm.AddState(item.Key,item.Value);
            }
        }

        private void InitInterruptibleHandlers()
        {
            foreach (var item in _handlerDic)
            {
                if (item.Value.Action.CanInterruptiblePlan)
                {
                    _InterruptibleHandlers.Add(item.Value);
                }
            }

            _InterruptibleHandlers = _InterruptibleHandlers.OrderByDescending(u=>u.Action.Priority).ToList();
        }


        protected abstract void InitActionHandlers();

        public void AddHander(TAction label)
        {
            var handler = _agent.Map.GetActionHandler(label);
            if (handler != null)
            {
                _handlerDic.Add(label, handler);
                handler.AddFinishCallBack(()=> { _onActionComplete(); });
            }
            else
            {
                DebugMsg.LogError("not find handler :" + label);

            }        
        }

        public void AddActionCompleteListener(Action complete) {
            _onActionComplete = complete;
        }
   
        public void ChangeCurrentAction(TAction label) 
        {
            _fsm.ChangeState(label);
        }
      
        public IActionHandler<TAction> GetHandler(TAction label)
        {
            if (_handlerDic.ContainsKey(label))
            {
                return _handlerDic[label];
            }
            else
            {
                DebugMsg.LogError("not find handler :" + label);
                return null;
            }
        }
        public void RemoveHandler(TAction label)
        {
            _handlerDic.Remove(label);
        }

        public void UpdateData() 
        {
            foreach (var item in _InterruptibleHandlers)
            {
                if (item.CanPerformAction)
                {
                    //todo  打断计划的api
                }
            }
        }

        public void FrameFun()
        {
            if (IsPerformAction)
            {
                _fsm.FrameFun();
            } 
        }
    }
}

