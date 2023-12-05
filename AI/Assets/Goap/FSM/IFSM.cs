using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SRAI
{
    public interface IFSM <Tfsm>
    {
        Tfsm CurrentState { get; }
        Tfsm PreviousState { get; }

        void AddState(Tfsm label, IFSMState<Tfsm> state);
        void ChangeState(Tfsm newstate);
        void FrameFun();
    }

    public interface IFSMState<TFsmState>
    {
        TFsmState Label { get; }
        void Enter();
        void Excute();
        void Exit();
    }



    public class FSM<Tfsm> : IFSM<Tfsm>
    {
        public Tfsm CurrentState { get { return _currentState.Label; } }

        public Tfsm PreviousState { get { return _previousState.Label; } }


        private IFSMState<Tfsm> _currentState;
        private IFSMState<Tfsm> _previousState;


        private Dictionary<Tfsm, IFSMState<Tfsm>> _stateDic;

        public FSM()
        {
            _stateDic = new Dictionary<Tfsm, IFSMState<Tfsm>>();
        }

        public void AddState(Tfsm label, IFSMState<Tfsm> state)
        {
            _stateDic.Add(label,state);
        }

        public void ChangeState(Tfsm newstate)
        {
            if (!_stateDic.ContainsKey(newstate))
            {
                DebugMsg.LogError("not find state："+ newstate);
                return;
            }

            _previousState = _currentState;
            _currentState = _stateDic[newstate];
            if (_previousState != null)
            {
                _previousState.Exit();
            }
            if (_currentState != null)
            {
                _currentState.Enter();
            }
        }

        public void FrameFun()
        {
            if (_currentState != null)
                _currentState.Excute();
        }
    }
}

