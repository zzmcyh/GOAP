using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SRAI
{
    public interface IPlanner<TAction, TGoal>
    {
        Queue<IActionHandler<TAction>> BuildPlan(IGoal<TGoal> goal);


    }
    public class Planner<TAction, TGoal> : IPlanner<TAction, TGoal>
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
            TreeNode<TAction> currentNode = Plan(goal);

            if (currentNode == null)
            {
                TAction label = _agent.ActionManager.GetDefualtActionLabel();
                plan.Enqueue(_agent.ActionManager.GetHandler(label));

                DebugMsg.LogError("currentNode is null");

            }

            while (currentNode.ID!=TreeNode<TAction>.DEFAULT_ID)
            {

            }



            return plan;
        }


        private TreeNode<TAction> Plan(IGoal<TGoal> goal)
        {

        }
    }

}

