using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace DBG
{
    public static class DebugerNN
    {
        public static void Debug(string msg)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log(msg);
#endif
        }

        public static void ErrorDebug(string msg)
        {
            UnityEngine.Debug.Log(msg);
        }
    }
}
