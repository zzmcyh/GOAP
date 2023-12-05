using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SRAI
{
    public class Tree<TAction>
    {
        public TreeNode<TAction> CreateTopNode()
        {
            TreeNode<TAction>.Reset();
            return new TreeNode<TAction>(null);
        }

        public TreeNode<TAction> CreateNormalNode(IActionHandler<TAction> actionHandler)
        {
            if (actionHandler == null)
                DebugMsg.LogError("actionHandler is null");
            return new TreeNode<TAction>(actionHandler);
        }

    }
    public class TreeNode<TAction>
    {

        public const int DEFAULT_ID = 0;

        private static int _id;
        public int ID { get; private set; }
        IActionHandler<TAction> ActionHandler { get;  set; }

        public IState CurrentState { get; set; }

        public IState GoalState { get; set; }

        public TreeNode<TAction> ParentNode { get; set; }

        public int Cost { get; set; }



        public TreeNode(IActionHandler<TAction> actionHandler)
        {
            ID=_id++;
            ActionHandler = actionHandler;
            Cost = 0;
            ParentNode = null;
            CurrentState = CurrentState.CreateState();
            GoalState = GoalState.CreateState();
        }

        public static void Reset()
        {
            _id = DEFAULT_ID;
           
        }

    }
}

