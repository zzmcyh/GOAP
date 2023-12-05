using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SRAI
{
    public interface IMap<TAction, TGoal> 
    {
        IActionHandler<TAction> GetActionHandler(TAction actionLabel);

        IGoal<TGoal> GetGoal(TGoal label);

        void SetGameData<Tkey>(Tkey key,object data);
        object GetGameData<Tkey>(Tkey key);
    }

    public abstract class MapBase<TAction, TGoal> : IMap<TAction, TGoal>
    {
        private Dictionary<TAction, IActionHandler<TAction>> _actionHandlerDic;
        private Dictionary<TGoal, IGoal<TGoal>> _goalDic;

        private Dictionary<string, object> _gameDataDic;


        public MapBase()
        {
            _actionHandlerDic = new Dictionary<TAction, IActionHandler<TAction>>();
            _goalDic = new Dictionary<TGoal, IGoal<TGoal>>();
            _gameDataDic = new Dictionary<string, object>();
            InitActionMap();
            InitGoalMap();
            InitGameData();
        }
        public IActionHandler<TAction> GetActionHandler(TAction actionLabel)
        {
            IActionHandler<TAction> handler;
            _actionHandlerDic.TryGetValue(actionLabel, out handler);
            if (handler==null)
            {
                DebugMsg.LogError("not find label:" +actionLabel);
            }

            return handler;
        }

        protected abstract void InitActionMap();
        protected abstract void InitGoalMap();

        protected abstract void InitGameData();


        protected void AddAction(IActionHandler<TAction> handler)
        {
            if (!_actionHandlerDic.ContainsKey(handler.Label))
            {
                _actionHandlerDic.Add(handler.Label,handler);
            }
            else
            {
                DebugMsg.LogError("has label already:"+handler.Label);
            }
        }

        public IGoal<TGoal> GetGoal(TGoal label)
        {
            IGoal<TGoal> goal;
            _goalDic.TryGetValue(label, out goal);
            if (goal == null)
            {
                DebugMsg.LogError("not find label:" + label);
            }

            return goal;
        }

        protected void AddGoal(IGoal<TGoal> goal)
        {
            if (!_goalDic.ContainsKey(goal.label))
            {
                _goalDic.Add(goal.label, goal);
            }
            else
            {
                DebugMsg.LogError("has label already:" + goal.label);
            }
        }

        public void SetGameData<Tkey>(Tkey key, object data)
        {
            _gameDataDic.Add(key.ToString(),data);
        }

        public object GetGameData<Tkey>(Tkey key)
        {
            if (_gameDataDic.ContainsKey(key.ToString()))
            {
                return _gameDataDic[key.ToString()];
            }
            else
            {
                DebugMsg.LogError("not find key"+key.ToString());
                return null;
            }    
           
        }
    }

}

