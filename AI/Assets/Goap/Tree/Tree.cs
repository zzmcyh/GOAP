using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SRAI
{
    public class Tree<TAction>
    {

    }
    public class TreeNode<TAction>
    {
        private static int _id;
        public int ID { get; private set; }
        IActionHandler<IAction<TAction>> ActionHandler { get;  set; }

        public IState CurrentState { get; set; }

        public IState GoalState { get; set; }

        public TreeNode<TAction> ParentNode { get; set; }

        public int Cost { get; set; }



        public TreeNode(IActionHandler<IAction<TAction>> actionHandler)
        {
            ID=_id++;
            ActionHandler = actionHandler;
            Cost = 0;
            ParentNode = null;
            CurrentState = CurrentState.CreateState();
            GoalState = GoalState.CreateState();
        }

        private void Reset()
        {
            _id = 0;
        }

    }
}

