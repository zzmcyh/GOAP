using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SRAI
{
    public interface IGoal<TGoal, TState>
    {
        TGoal label { get; }
        float GetPriority();

        IState<TState> GetEffect();

        bool IsGoalComplete();

        void AddGoalActivateListener(System.Action<IGoal<TGoal, TState>> onActivate);

        void AddGoalInactivateListener(System.Action<IGoal<TGoal, TState>> onInactivate);

        void UpdateData();
    }


    public abstract class GoalBase<TGoal, TState> : IGoal<TGoal, TState>
    {
        public abstract TGoal label { get; }
        private System.Action<IGoal<TGoal, TState>> _onActivate;
        private System.Action<IGoal<TGoal, TState>> _onInactivate;


        public GoalBase()
        {

        }
        protected abstract bool ActiveCondition();//激活条件
        public abstract IState<TState> GetEffect();
        public abstract float GetPriority();
        public abstract bool IsGoalComplete();


        public void AddGoalActivateListener(Action<IGoal<TGoal, TState>> onActivate) { _onActivate = onActivate; }
        public void AddGoalInactivateListener(Action<IGoal<TGoal, TState>> onInactivate) { _onInactivate = onInactivate; }
     
        public void UpdateData() 
        {
            if (ActiveCondition())
            {
                _onActivate(this);
            }
            else
            {
                _onInactivate(this);
            }
        }
    }
}

