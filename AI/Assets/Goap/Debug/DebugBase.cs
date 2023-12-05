using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SRAI
{
    public abstract class DebugBase
    {
        public static DebugBase Instance { get; set; }

        public abstract void Log(string msg);

        public abstract void LogWaining(string msg);

        public abstract void LogError(string msg);
    }


    public class DebugMsg
    {
        public static void Log(string msg)
        {
            DebugBase.Instance.Log(msg);
        }
        public static void LogWarning(string msg)
        {
            DebugBase.Instance.Log(msg);
        }
        public static void LogError(string msg)
        {
            DebugBase.Instance.LogError(msg);
        }

    }

}