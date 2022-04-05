#region Copyright Notice

// ******************************************************************************************************************
// 
// UBoatEscapeRoom-Unity.UBER.Player.FunctionTimer.cs © SilentWolf6662 - All Rights Reserved
// Unauthorized copying of this file, via any medium is strictly prohibited
// Proprietary and confidential
// 
// This work is licensed under the Creative Commons Attribution-NonCommercial-NoDerivs 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-nd/3.0/
// 
// Created & Copyrighted @ 2022-03-29
// 
// ******************************************************************************************************************

#endregion
#region

using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

#endregion
namespace UBER.Utils
{

    /*
     * Triggers a Action after a certain time 
     * */
    public class FunctionTimer
    {


        private static List<FunctionTimer> timerList; // Holds a reference to all active timers
        private static GameObject initGameObject; // Global game object used for initializing class, is destroyed on scene change
        private readonly Action action;
        private readonly string functionName;





        private readonly GameObject gameObject;
        private readonly bool useUnscaledDeltaTime;
        private bool active;
        private float timer;



        public FunctionTimer(GameObject gameObject, Action action, float timer, string functionName, bool useUnscaledDeltaTime)
        {
            this.gameObject = gameObject;
            this.action = action;
            this.timer = timer;
            this.functionName = functionName;
            this.useUnscaledDeltaTime = useUnscaledDeltaTime;
        }

        private static void InitIfNeeded()
        {
            if (initGameObject == null)
            {
                initGameObject = new GameObject("FunctionTimer_Global");
                timerList = new List<FunctionTimer>();
            }
        }




        public static FunctionTimer Create(Action action, float timer) => Create(action, timer, "", false, false);
        public static FunctionTimer Create(Action action, float timer, string functionName) => Create(action, timer, functionName, false, false);
        public static FunctionTimer Create(Action action, float timer, string functionName, bool useUnscaledDeltaTime) => Create(action, timer, functionName, useUnscaledDeltaTime, false);
        public static FunctionTimer Create(Action action, float timer, string functionName, bool useUnscaledDeltaTime, bool stopAllWithSameName)
        {
            InitIfNeeded();

            if (stopAllWithSameName)
                StopAllTimersWithName(functionName);

            GameObject obj = new GameObject("FunctionTimer Object " + functionName, typeof(MonoBehaviourHook));
            FunctionTimer funcTimer = new FunctionTimer(obj, action, timer, functionName, useUnscaledDeltaTime);
            obj.GetComponent<MonoBehaviourHook>().OnUpdate = funcTimer.Update;

            timerList.Add(funcTimer);

            return funcTimer;
        }
        public static void RemoveTimer(FunctionTimer funcTimer)
        {
            InitIfNeeded();
            timerList.Remove(funcTimer);
        }
        public static void StopAllTimersWithName(string functionName)
        {
            InitIfNeeded();
            for (int i = 0; i < timerList.Count; i++)
                if (timerList[i].functionName == functionName)
                {
                    timerList[i].DestroySelf();
                    i--;
                }
        }
        public static void StopFirstTimerWithName(string functionName)
        {
            InitIfNeeded();
            for (int i = 0; i < timerList.Count; i++)
                if (timerList[i].functionName == functionName)
                {
                    timerList[i].DestroySelf();
                    return;
                }
        }

        private void Update()
        {
            if (useUnscaledDeltaTime)
                timer -= Time.unscaledDeltaTime;
            else
                timer -= Time.deltaTime;
            if (timer <= 0)
            {
                // Timer complete, trigger Action
                action();
                DestroySelf();
            }
        }
        private void DestroySelf()
        {
            RemoveTimer(this);
            if (gameObject != null)
                Object.Destroy(gameObject);
        }

        // Create a Object that must be manually updated through Update();
        public static FunctionTimerObject CreateObject(Action callback, float timer) => new FunctionTimerObject(callback, timer);

        /*
         * Class to hook Actions into MonoBehaviour
         * */
        private class MonoBehaviourHook : MonoBehaviour
        {

            public Action OnUpdate;

            private void Update()
            {
                if (OnUpdate != null) OnUpdate();
            }
        }




        /*
         * Class to trigger Actions manually without creating a GameObject
         * */
        public class FunctionTimerObject
        {
            private readonly Action callback;

            private float timer;

            public FunctionTimerObject(Action callback, float timer)
            {
                this.callback = callback;
                this.timer = timer;
            }

            public void Update() => Update(Time.deltaTime);
            public void Update(float deltaTime)
            {
                timer -= deltaTime;
                if (timer <= 0)
                    callback();
            }
        }
    }
}
