using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SRAI
{
    public interface IGoalManager<TGoal, TState>
    {
        IGoal<TGoal, TState> Current { get; }
        void AddGoal(TGoal label);
        void Remove(TGoal label);
        IGoal<TGoal, TState> GetGoal(TGoal label);
        IGoal<TGoal, TState> FindGoal();
        void UpdateData();
    }

    public abstract class GoalManager<TGoal, TState, TAction> : IGoalManager<TGoal, TState>
    {
        private Dictionary<TGoal, IGoal<TGoal, TState>> _goalDic;
        private List<IGoal<TGoal, TState>> _activeGoals;


        IAgent<TState, TAction, TGoal> _agent;

        public GoalManager(IAgent<TState, TAction, TGoal> agent)
        {
            _agent = agent;
            _goalDic = new Dictionary<TGoal, IGoal<TGoal, TState>>();
            _activeGoals = new List<IGoal<TGoal, TState>>();
            InitGoals();
        }
        protected abstract void InitGoals();


        public  IGoal<TGoal, TState> Current { get; private set; }

        public void AddGoal(TGoal label)
        {
            var goal = _agent.Map.GetGoal(label);
            if (goal != null) {
                _goalDic.Add(label,goal);

                goal.AddGoalActivateListener((x)=> {
                    if (!_activeGoals.Contains(x))
                    {
                        _activeGoals.Add(x);
                    }
                    
                });
                goal.AddGoalInactivateListener((x)=> {
                    if (_activeGoals.Contains(x))
                    {
                        _activeGoals.Remove(x);
                    }
                  
                });
            }


        }
        public IGoal<TGoal, TState> FindGoal() {

            _activeGoals = _activeGoals.OrderByDescending((x) => x.GetPriority()).ToList();
            if (_activeGoals.Count > 0)
            {
                return _activeGoals[0];
            }
            else
            {
                DebugMsg.LogError("not find  goal");
                return null;
            }
        }
        public IGoal<TGoal, TState> GetGoal(TGoal label)
        {
            if (_goalDic.ContainsKey(label))
            {
                return _goalDic[label];
            }
            DebugMsg.LogError("not find label"+label);
            return null;
        }
        public void Remove(TGoal label) {
            _goalDic.Remove(label);
        }
        public void UpdateData() 
        {
            UpdateGoals();
            UpdateCurrentGoal();
        }

        private void UpdateGoals()
        {
            foreach (var item in _goalDic)
            {
                item.Value.UpdateData();
            }
        }

        private void UpdateCurrentGoal()
        {
            Current = FindGoal();
            if (Current == null)
            {
                DebugMsg.LogError("target is null");
            }
        }

    }

}

