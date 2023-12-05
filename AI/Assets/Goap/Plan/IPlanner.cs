using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                plan.Enqueue(currentNode.ActionHandler);
                currentNode = currentNode.ParentNode;
            }

            DebugMsg.Log("isfinish!");

            return plan;
        }


        private TreeNode<TAction> Plan(IGoal<TGoal> goal)
        {
            Tree<TAction> tree = new Tree<TAction>();

            TreeNode<TAction> topNode = CreateTopNode(tree,goal);

            TreeNode<TAction> currentNode = topNode;

            TreeNode<TAction> cheapestNode = null;

            TreeNode<TAction> subNode = null;

            while (!IsEnd(currentNode))
            {
                List<IActionHandler<TAction>> handlers = GetSubHandlers(currentNode);
                foreach (var item in handlers)
                {
                     subNode = tree.CreateNormalNode(item);
                     SetSubNodeState(currentNode,subNode);
                     subNode.Cost = GetCost(subNode);
                     subNode.ParentNode = currentNode;
                     cheapestNode = GetCheapestNode(subNode,cheapestNode);
                }
                currentNode = cheapestNode;
                cheapestNode = null;
            }

            return currentNode;
        }

        private int GetCost(TreeNode<TAction> node)
        {
            int actionCost = 0;
            if (node.ActionHandler != null)
            {
                actionCost = node.ActionHandler.Action.Cost;
            }
            else
            {
                DebugMsg.LogError("ActionHandler can not null!!");
            }

            return node.Cost + GetStateDifferentNum(node) + actionCost;
        }


        private TreeNode<TAction> GetCheapestNode(TreeNode<TAction> nodeOne, TreeNode<TAction> nodeTwo) 
        {
            if (nodeOne==null||nodeOne.ActionHandler==null)
            {
                return nodeTwo;
            }
            if (nodeTwo == null || nodeTwo.ActionHandler == null)
            {
                return nodeOne;
            }
            if (nodeOne.Cost > nodeTwo.Cost)
            {
                return nodeTwo;
            }else if (nodeOne.Cost< nodeTwo.Cost)
            {
                return nodeOne;
            }
            else
            {
                if (nodeOne.ActionHandler.Action.Priority > nodeTwo.ActionHandler.Action.Priority)
                {
                    return nodeOne;
                }
                else
                {
                    return nodeTwo;
                }
            }


        }


        private void SetSubNodeState(TreeNode<TAction> currentnode,TreeNode<TAction> subnode)
        {
            if (subnode.ID > TreeNode<TAction>.DEFAULT_ID)
            {
                IAction<TAction> subAction = subnode.ActionHandler.Action;
                subnode.CopyState(currentnode);
                IState data=subnode.GoalState.GetSameData(subAction.Effect);
                subnode.CurrentState.Set(data);
                subnode.GoalState.Set(subAction.Preconditions);
                SetNodeCurrentState(subnode);
            }
            else
            {
                DebugMsg.LogError("node is topnode,that is error!");
            }
        }



        private List<IActionHandler<TAction>> GetSubHandlers(TreeNode<TAction> node)
        {
            List<IActionHandler<TAction>> handlers = new List<IActionHandler<TAction>>();

            if (node == null)
                return handlers;
            //获取到状态差异的所有键值
            var keys = node.CurrentState.GetValueDifference(node.GoalState);

            var map = _agent.ActionManager.EffectsAndActionMap;
            //对比所有的动作
            //找到能实现当前键值得动作
            //也就action的effects 中包含此键值

            foreach (var item in keys)
            {
                if (map.ContainsKey(item))
                {
                    foreach (IActionHandler<TAction> handler in map[item])
                    {
                        if (!handlers.Contains(handler))
                        {
                            handlers.Add(handler);
                        }
                    }
                }
                else
                {
                    DebugMsg.LogError("no action can make from currentState change GoalState :"+item);
                }
            }

            handlers = handlers.OrderByDescending(u=>u.Action.Priority).ToList();


            return handlers;
        }

        private bool IsEnd(TreeNode<TAction> currentNode)
        {
            if (currentNode == null)
            {
                return true;
            }
            if (GetStateDifferentNum(currentNode) == 0)
            {
                DebugMsg.Log("find action finish!");
                return true;
            }

            return false;

        }


        private int GetStateDifferentNum(TreeNode<TAction> currentNode)
        {
            return currentNode.CurrentState.GetValueDifference(currentNode.GoalState).Count;
        }




        private TreeNode<TAction> CreateTopNode(Tree<TAction> tree,IGoal<TGoal> goal)
        {
            TreeNode<TAction> topNode = tree.CreateTopNode();
            topNode.GoalState.Set(goal.GetEffect());
            var keys= topNode.CurrentState.GetNotExistKeys(topNode.GoalState);
            foreach (var item in keys)
            {
                topNode.CurrentState.Set(item, _agent.AgentSate.Get(item));
            }
            SetNodeCurrentState(topNode);
            return topNode;
        }


        private void SetNodeCurrentState(TreeNode<TAction> node)
        {
            //goal当中存在，current中不存在，获取这样的键值
            //通过键值 在agentstate当中获取数据
            var keys = node.CurrentState.GetNotExistKeys(node.GoalState);
            foreach (var item in keys)
            {
                node.CurrentState.Set(item, _agent.AgentSate.Get(item));
            }
        }

    }

}

