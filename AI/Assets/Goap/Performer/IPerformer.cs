using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SRAI
{
    public interface IPerformer
    {
        void UpdateData();
        void  Interruptible();
    }
    public class Performer<TAction, TGoal> : IPerformer
    {
        private IPlanner<TAction, TGoal> _planner;
        private IPlannerHandler<TAction> _plannerHandler;
        private IGoalManager<TGoal> _goalManager;
        private IActionManager<TAction> _actionManager;


        public Performer(IAgent<TAction, TGoal> agent)
        {
            _plannerHandler = new PlannerHandler<TAction>();
            _plannerHandler.AddCompleteCallBack(PlanComplete);
            _planner = new Planner<TAction, TGoal>(agent);

            _goalManager = agent.GoalManager;
            _actionManager = agent.ActionManager;
            _actionManager.AddActionCompleteListener(PlanActionComplete);
        }

        private void PlanComplete()
        {
            DebugMsg.Log("plan is finish!");
            _actionManager.IsPerformAction = false;
        }

        private void PlanActionComplete()
        {
            DebugMsg.Log("next!");
            _plannerHandler.NextAction();
        }


        public void UpdateData()
        {
            if (WetherToRePlan())
            {
                BuildAndStartPlane();
            }
        }

        public void Interruptible()
        {
            _plannerHandler.Interruptible();
        }

        /// <summary>
        /// 是否需要重新制定计划
        /// </summary>
        /// <returns></returns>
        private bool WetherToRePlan()
        {
            return _plannerHandler.IsComplete;
        }


        private void BuildAndStartPlane()
        {
            var plan = _planner.BuildPlan(_goalManager.Current);
            if (plan!=null&&plan.Count>0)
            {
                _plannerHandler.Init(_actionManager,plan);
                _plannerHandler.StartPlan();
                _actionManager.IsPerformAction = true;
            }
        }

    }

}

