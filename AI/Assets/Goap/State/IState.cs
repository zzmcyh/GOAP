using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SRAI
{
    public interface IState<TState>
    {
        void Set(TState key,bool value);
        bool Get(TState key);
        void Set(IState<TState> otherState);
        ICollection<TState> GetKeys();
        bool ContainKey(TState key);
        bool ContainState(IState<TState> otherState);
        void AddStateChangeListener(Action onchange);
        void Clear();

        IState<TState> InversionValue();

    }

    public class State<TState> : IState<TState>
    {
        Dictionary<TState, bool> _dataTable;

        private Action _onChange;
        public State()
        {
            _dataTable = new Dictionary<TState, bool>();
        }

        public bool Get(TState key)
        {
            if (!_dataTable.ContainsKey(key))
            {
                DebugMsg.Log("当前状态不包含此key："+key);
                return false;
            }

            return true;
        }

        public void Set(TState key, bool value)
        {
            if (_dataTable.ContainsKey(key)&&_dataTable[key]!=value)
            {
                ChangeValue(key, value);
            }
            else if (!_dataTable.ContainsKey(key))
            {
                ChangeValue(key, value);
            }
        }

        private void ChangeValue(TState key,bool value) 
        {
            _dataTable[key] = value;
            _onChange?.Invoke();
        }

        public void AddStateChangeListener(Action onchange)
        {
            _onChange = onchange;
        }

        public void Set(IState<TState> otherState)
        {
            foreach (var key in otherState.GetKeys())
            {
                Set(key,otherState.Get(key));
            }
        }

        public ICollection<TState> GetKeys()
        {
            return _dataTable.Keys;
        }

        public bool ContainKey(TState key)
        {
            return _dataTable.ContainsKey(key);
        }

        public bool ContainState(IState<TState> otherState)
        {
            foreach (var key in otherState.GetKeys())
            {
                if (!ContainKey(key) || _dataTable[key] != otherState.Get(key))
                {
                    return false;
                }
            }
            return true;
        }

        public void Clear()
        {
            _dataTable.Clear();
        }


        public override string ToString()
        {
            StringBuilder temp = new StringBuilder();
            foreach (var item in _dataTable)
            {
                temp.Append("key:");
                temp.Append(item.Key);
                temp.Append("value:");
                temp.Append(item.Value);
                temp.Append("\r\n");
            }

            return temp.ToString();
        }

        public IState<TState> InversionValue()
        {
            IState<TState> state = new State<TState>();
            foreach (var item in _dataTable)
            {
                state.Set(item.Key,!item.Value);
            }
            return state;

        }
    }




}

