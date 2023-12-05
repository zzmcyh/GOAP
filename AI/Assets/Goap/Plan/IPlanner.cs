using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SRAI
{
    public interface IPlanner<TAction, TGoal>
    {
        Queue<IActionHandler<TAction>> BuildPlan(IGoal<TGoal> goal);


    }
    public class Planner<TAction, TGoal, TState> : IPlanner<TAction, TGoal>
    {
        IAgent<TAction, TGoal> _agent;
        public Planner(IAgent< TAction, TGoal> agent)
        {
            _agent = agent;
        }
        public Queue<IActionHandler<TAction>> BuildPlan(IGoal<TGoal> goal)
        {
            Queue<IActionHandler<TAction>> plan = new Queue<IActionHandler<TAction>>();
            if (goal == null)
            {
                return plan;
            }
            return plan;
        }


        private void Plan(IGoal<TGoal> goal)
        {

        }
    }

}

