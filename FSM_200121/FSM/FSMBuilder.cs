using System.Collections;
using UnityEngine;

namespace AiGame.FSM
{
    enum FSMBuilderState
    {
        None,
        AddingStates,
        AddingEventHandlers,
        AddingTransitions
    }
    public class FSMBuilder 
    {
        protected readonly FSMSystem fsm;

        public string from;
        public string to;
        public string defaultStateID = null;

        FSMBuilderState state = FSMBuilderState.None;
        FSMEventName fsmPerformID;

        public FSMBuilder(FSMEventName fsmPerformID)
        {
            this.fsmPerformID = fsmPerformID;
            fsm = new FSMSystem(fsmPerformID);
        }

        public void Suspend() => fsm.Suspend();
        public void Resume()
        {
            if (fsm.CurrentState == null)
                fsm.EntryDefaultState(defaultStateID);
            else
                fsm.Resume();
        }

        public void Entry()
        {
            fsm.EntryDefaultState(defaultStateID);
        }

        public FSMBuilder FSMEventHandlersBegin
        {
            get
            {
                state = FSMBuilderState.AddingEventHandlers;
                return this;
            }
        }
        public FSMBuilder FSMEventHandlersEnd
        {
            get
            {
                if (state != FSMBuilderState.AddingEventHandlers)
                    throw new System.Exception("invalid call, should be at after FSMEventHandlersBegin");
                state = FSMBuilderState.None;
                return this;
            }
        }

        public FSMBuilder Suspend(EventName eventName)
        {

            if (state != FSMBuilderState.AddingEventHandlers)
                throw new System.Exception("invalid call, should be at after FSMEventHandlersBegin");
            EventsMessanger.SubscribeToEvent(eventName, (EventData) => { fsm.Suspend(); });
            return this;
        }

        public FSMBuilder Resume(EventName eventName)
        {
            if (state != FSMBuilderState.AddingEventHandlers)
                throw new System.Exception("invalid call, should be at after FSMEventHandlersBegin");
            EventsMessanger.SubscribeToEvent(eventName, (EventData) => { fsm.Resume(); });
            return this;
        }
        public FSMBuilder AddStatesBegin
        {
            get
            {
                defaultStateID = "";
                state = FSMBuilderState.AddingStates;
                return this;
            }

        }
        public FSMBuilder AddStatesEnd
        {
            get
            {
                if (state != FSMBuilderState.AddingStates)
                    throw new System.Exception("invalid call, should be at after AddStateBegin");
                state = FSMBuilderState.None;
                if (defaultStateID.Length == 0)
                    throw new System.Exception("invalid call, StateDefault not set");

                return this;
            }
        }
        public FSMBuilder StateDefault(FSMState st)
        {
            if (state != FSMBuilderState.AddingStates)
                throw new System.Exception("invalid call, should be at after AddStateBegin");
            fsm.AddState(st);
            defaultStateID = st.StateID;
            return this;
        }

        public FSMBuilder State(FSMState st)
        {
            if (state != FSMBuilderState.AddingStates)
                throw new System.Exception("invalid call, should be at after AddStateBegin");
            fsm.AddState(st);
            return this;
        }
        public FSMBuilder AddTrnsitionsBegin
        {
            get
            {
                state = FSMBuilderState.AddingTransitions;
                return this;
            }
        }
        public FSMBuilder AddTrnsitionsEnd
        {
            get
            {
                if (state != FSMBuilderState.AddingTransitions)
                    throw new System.Exception("invalid call, should be at after FSMTrnsitionBegin");
                state = FSMBuilderState.None;
                return this;
            }
        }

        public FSMBuilder Transition(string from, string to)
        {
            if (state != FSMBuilderState.AddingTransitions)
                throw new System.Exception("invalid call, should be at after Transition method");
            this.from = from;
            this.to = to;
            return this;
        }
        public FSMBuilder FrwdEvent(EventName eventName)
        {
            if (state != FSMBuilderState.AddingTransitions)
                throw new System.Exception("invalid call, should be at after Transition method");
            fsm.AddTransition(from, to, new FSMEventCondition(eventName, fsmPerformID));
            return this;
        }
        public FSMBuilder BackEvent(EventName eventName)
        {
            if (state != FSMBuilderState.AddingTransitions)
                throw new System.Exception("invalid call, should be at after Transition method");
            fsm.AddTransition(to, from, new FSMEventCondition(eventName, fsmPerformID));
            return this;
        }
        //
        // use to make the end for FSMBuilder rValue chain
        // example:
        //  var fsmBuilder = new FSMBuilder();
        //  /* ...  */
        //  fsmBuilder
        //        .FSMEventHandlersBegin
        //             /* ...  */
        //        .FSMEventHandlersEnd
        //        .AddStatesBegin
        //             /* ...  */
        //        .AddStatesEnd
        //        .AddTrnsitionsBegin
        //             /* ...  */
        //        .AddTrnsitionsEnd
        //        .BuilderFinish();
        //  /* ...  */
        //
        public void BuilderFinish() 
        {
            if (defaultStateID == null)
                throw new System.Exception("FSM does not complete constructed");
        }

    }
}