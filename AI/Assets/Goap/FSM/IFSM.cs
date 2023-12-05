using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SRAI
{
    public interface IFSM <TState>
    {
        TState currentState { get; }
        TState previousState { get; }

        void AddState(TState label, IFrameState<TState> state);
        void ChangeState(TState newstate);
        void FrameFun();
    }

    public interface IFrameState<TState>
    {
        TState label { get; }
        void Enter();
        void Excute();
        void Exit();
    }


}

