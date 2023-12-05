using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SRAI
{
    public interface IState
    {
        IState CreateState();
        void Set(string key,bool value);
        bool Get(string key);
        void Set(IState otherState);
        ICollection<string> GetKeys();
        bool ContainKey(string key);
        bool ContainState(IState otherState);
        void AddStateChangeListener(Action onchange);
        void Clear();

        IState InversionValue();

        ICollection<string> GetNotExistKeys(IState otherState);

        ICollection<string> GetValueDifference(IState otherState);


        void CopyState(IState otherState);

        IState GetSameData(IState otherState);

    }

    public static class IStateExtend
    {
        public static IState CreateState(this IState state)
        {
            return new State();
        }
    }



    public class State : IState
    {
        Dictionary<string, bool> _dataTable;

        private Action _onChange;
        public State()
        {
            _dataTable = new Dictionary<string, bool>();
        }




        public bool Get(string key)
        {
            if (!_dataTable.ContainsKey(key))
            {
                DebugMsg.Log("当前状态不包含此key："+key);
                return false;
            }

            return true;
        }

        public void Set(string key, bool value)
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

        private void ChangeValue(string key,bool value) 
        {
            _dataTable[key] = value;
            _onChange?.Invoke();
        }

        public void AddStateChangeListener(Action onchange)
        {
            _onChange = onchange;
        }

        public void Set(IState otherState)
        {
            foreach (var key in otherState.GetKeys())
            {
                Set(key,otherState.Get(key));
            }
        }

        public ICollection<string> GetKeys()
        {
            return _dataTable.Keys;
        }

        public bool ContainKey(string key)
        {
            return _dataTable.ContainsKey(key);
        }

        public bool ContainState(IState otherState)
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

        public IState InversionValue()
        {
            IState state = new State();
            foreach (var item in _dataTable)
            {
                state.Set(item.Key,!item.Value);
            }
            return state;

        }

        public IState CreateState()
        {
            return new State();
        }

        public ICollection<string> GetNotExistKeys(IState otherState)
        {
            List<string> keys = new List<string>();

            foreach (var item in otherState.GetKeys())
            {
                if (!_dataTable.ContainsKey(item))
                {
                    keys.Add(item);
                }
            }

            return keys;
        }

        public ICollection<string> GetValueDifference(IState otherState)
        {
            List<string> keys = new List<string>();
            foreach (var item in otherState.GetKeys())
            {
                if (!_dataTable.ContainsKey(item) || otherState.Get(item) != _dataTable[item])
                {
                    keys.Add(item);
                }
            }
            return keys;
        }

        public void CopyState(IState otherState)
        {
            Clear();
            Set(otherState);
        }

        public IState GetSameData(IState otherState)
        {
            IState temp = new State();


            foreach (var item in _dataTable)
            {
                if (otherState.ContainKey(item.Key))
                {
                    temp.Set(item.Key,item.Value);
                }
            }

            return temp;
        }
    }




}

