using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SRAI
{
    public interface IGoal<TGoal>
    {
        TGoal label { get; }
        float GetPriority();

        IState GetEffect();

        bool IsGoalComplete();

        void AddGoalActivateListener(System.Action<IGoal<TGoal>> onActivate);

        void AddGoalInactivateListener(System.Action<IGoal<TGoal>> onInactivate);

        void UpdateData();
    }


    public abstract class GoalBase<TGoal> : IGoal<TGoal>
    {
        public abstract TGoal label { get; }
        private System.Action<IGoal<TGoal>>_onActivate;
        private System.Action<IGoal<TGoal>>_onInactivate;


        public GoalBase()
        {

        }
        protected abstract bool ActiveCondition();//激活条件
        public abstract IState GetEffect();
        public abstract float GetPriority();
        public abstract bool IsGoalComplete();


        public void AddGoalActivateListener(Action<IGoal<TGoal>> onActivate) { _onActivate = onActivate; }
        public void AddGoalInactivateListener(Action<IGoal<TGoal>> onInactivate) { _onInactivate = onInactivate; }
     
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

