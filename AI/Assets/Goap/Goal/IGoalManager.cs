using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SRAI
{
    public interface IGoalManager<TGoal>
    {
        IGoal<TGoal> Current { get; }
        void AddGoal(TGoal label);
        void Remove(TGoal label);
        IGoal<TGoal> GetGoal(TGoal label);
        IGoal<TGoal> FindGoal();
        void UpdateData();
    }

    public abstract class GoalManager<TGoal, TAction> : IGoalManager<TGoal>
    {
        private Dictionary<TGoal, IGoal<TGoal>> _goalDic;
        private List<IGoal<TGoal>> _activeGoals;


        IAgent<TAction, TGoal> _agent;

        public GoalManager(IAgent< TAction, TGoal> agent)
        {
            _agent = agent;
            _goalDic = new Dictionary<TGoal, IGoal<TGoal>>();
            _activeGoals = new List<IGoal<TGoal>>();
            InitGoals();
        }
        protected abstract void InitGoals();


        public  IGoal<TGoal> Current { get; private set; }

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
        public IGoal<TGoal> FindGoal() {

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
        public IGoal<TGoal> GetGoal(TGoal label)
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

