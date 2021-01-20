using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AiGame.FSM
{
    public class FSMState 
    {

        public string StateID { get; }
        public bool IsActive { get; private set; } = false;

        public FSMState(string stateID)
        {
            this.StateID = stateID;
        }
        public virtual void Processing() { }

        public void SetActive(bool isActive)
        {
            if (isActive && !this.IsActive)
                OnResume();
            else
            if (!isActive && this.IsActive)
                OnSuspend();
            this.IsActive = isActive;
        }
        protected virtual void OnSuspend() { HandleEvents(new EventHandlers(false)); }
        protected virtual void OnResume() { HandleEvents(new EventHandlers(true)); }
        protected virtual void HandleEvents(EventHandlers eventHandler) { }

        public void Enter(FSMStateIOData inputData)
        {
            DoBeforeEntering(inputData);
            SetActive(true);
        }
        public void Leave(ref FSMStateIOData outputData)
        {
            DoBeforeLeaving(ref outputData);
            SetActive(false);
        }

        protected virtual void DoBeforeEntering(FSMStateIOData inputData) { }
        protected virtual void DoBeforeLeaving(ref FSMStateIOData outputData) { }

        //bool isActive
    }

}