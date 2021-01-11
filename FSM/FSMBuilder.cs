using System.Collections;
using UnityEngine;

namespace AiGame.FSM
{   public class FSMBuilder 
    {
        protected readonly FSMSystem fsm = new FSMSystem();

        public string from;
        public string to;
        public string defaultStateID;

        bool addingState = false;
        public void Entry()
        {
            fsm.EntryDefaultState(defaultStateID);
        }

        public FSMBuilder AddStatesBegin
        {
            get 
            {
                defaultStateID = "";
                addingState = true; 
                return this; 
            }
            
        }
        public FSMBuilder AddStatesEnd
        {
            get
            {
                if (!addingState)
                    throw new System.Exception("invalid call, should be at after AddStateBegin");
                addingState = false;
                if (defaultStateID.Length == 0)
                    throw new System.Exception("invalid call, StateDefault not set");

                return this;
            }
        }
        public FSMBuilder StateDefault( FSMState st)
        {
            if (!addingState)
                throw new System.Exception("invalid call, should be at after AddStateBegin");
            fsm.AddState(st);
            defaultStateID = st.StateID;
            return this;
        }

        public FSMBuilder State(FSMState st)
        {
            if (!addingState)
                throw new System.Exception("invalid call, should be at after AddStateBegin");
            fsm.AddState(st);
            return this;
        }

        public FSMBuilder Transition(string from, string to)
        {
            this.from = from;
            this.to = to;
            return this;
        }
        public FSMBuilder FrwdEvent(string eventName)
        {
            fsm.AddTransition(from, to, new FSMEventCondition(eventName));
            return this;
        }
        public FSMBuilder BackEvent(string eventName)
        {
            fsm.AddTransition(to, from, new FSMEventCondition(eventName));
            return this;
        }

    }

}