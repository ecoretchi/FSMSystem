using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AiGame.FSM
{
    public class FSMState
    {
        public string StateID { get; }

        public FSMState(string stateID)
        {
            this.StateID = stateID;
        }
        public virtual void DoBeforeEntering(EventData ed) { }
        public virtual void Processing() { }
        public virtual void DoBeforeLeaving(EventData ed) { }

    }
}