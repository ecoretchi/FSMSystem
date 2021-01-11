using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMEventData : EventData
{
    public string transitionName;
    public EventData ed;
}
namespace AiGame.FSM
{
    public class FSMSystem
    {
        public static readonly string performTransitionEventName = "FSMPerformTransitionEvent";
        protected Dictionary<string, FSMStateData> states = new Dictionary<string, FSMStateData>();

        FSMStateData currentStateData = null;
        public FSMState CurrentState 
        { 
            get 
            {
                return currentStateData.state; 
            } 
        }

        public void EntryDefaultState(string stateID)
        {
            if (!states.ContainsKey(stateID))
            {
                Debug.LogErrorFormat("FSM ERROR: state with ID {0} not found", stateID);
                return;
            }

            


            Debug.LogFormat("FSM: EntryDefaultState {0}", stateID);

            SetNewState(GetStateData(stateID));
            
            CurrentState.DoBeforeEntering(null);

            
        }

        void SetNewState(FSMStateData newStateData)
        {
            currentStateData = newStateData;
            UpdateFSMSubscriber();
            Debug.LogFormat("FSM: SetNewState {0}", newStateData.state.StateID);
        }
        void UpdateFSMSubscriber()
        {
            EventsMessanger.UnsubscribeFromEvent(performTransitionEventName, OnPerformTransitionEvent);
            EventsMessanger.SubscribeToEvent(performTransitionEventName, OnPerformTransitionEvent);
        }

        void OnPerformTransitionEvent(EventData ed)
        {
            FSMEventData eventData = ((FSMEventData)ed);
            PerformTransition(eventData.transitionName, eventData.ed);
        }

        public void AddState(FSMState fsmState)
        {
            // Check for Null reference before deleting
            if (fsmState == null)
            {
                Debug.LogError("FSM ERROR: Null reference is not allowed");
                return;
            }
            if (states.ContainsKey(fsmState.StateID))
            {
                Debug.LogErrorFormat("FSM ERROR: state with ID {0} already exist", fsmState.StateID);
                return;
            }

            states.Add(fsmState.StateID, new FSMStateData() { state = fsmState });
        }

        public void DeleteState(string stateID)
        {
            // Check for NullState before deleting
            if (stateID.Length == 0)
            {
                Debug.LogError("FSM ERROR: Empty `stateID` is not allowed for a real state");
                return;
            }

            // Search the List and delete the state if it's inside it
            if (states.ContainsKey(stateID))
            {
                states.Remove(stateID);
                return;
            }
            Debug.LogErrorFormat("FSM ERROR: Impossible to delete state {0}. It was not on the list of states", stateID);
        }

        FSMStateData GetStateData(string stateID)
        {
            if (states.ContainsKey(stateID))
                return states[stateID];
            return null;
        }

        // That transtion can`t be removed
        public FSMTransition AddTransition(string fromStateID, string toStateID, FSMCondition condition, int priority = 100)
        {
            var fromStateData = GetStateData(fromStateID);

            if (fromStateData == null)
            {
                Debug.LogErrorFormat("FSM ERROR: Impossible to add transition state {0}. It was not on the list of states", fromStateID);
                return null;
            }

            var toStateData = GetStateData(toStateID);

            if (toStateData == null)
            {
                Debug.LogErrorFormat("FSM ERROR: Impossible to add transition state {0}. It was not on the list of states", toStateID);
                return null;
            }

            var transition = fromStateData.GetTransition(toStateData.state.StateID, priority);

            transition.AddCondition(condition);

            return transition;
        }

        public void PerformTransition(string conditionName, EventData ed = null)
        {
            // Check for NullTransition before changing the current state
            if (conditionName.Length == 0)
            {
                Debug.LogError("FSM ERROR:  Empty `transName` is not allowed for a real transition");
                return;
            }

            if (currentStateData == null)
            {
                Debug.LogException(new Exception("FSM EXEPTION: current state missing"));
                return;
            }
            // Check if the currentState has the transition passed as argument
            string nextStateID = currentStateData.GetOutputStateID(conditionName);
            if (nextStateID.Length == 0)
            {
                Debug.LogFormat("FSM: Current State {0} does not have any condition {1} to perform next state",
                    CurrentState.StateID, conditionName);
                return;
            }

            FSMStateData nextStateData;
            if (!states.TryGetValue(nextStateID, out nextStateData))
            {
                Debug.LogException(new Exception("FSM EXCEPTION: State lost from dictionary"));
                return;
            }


            // Do the post processing of the state before setting the new one
            CurrentState.DoBeforeLeaving(ed);

            SetNewState(nextStateData);

            // Reset the state to its desired condition before it can reason or act
            CurrentState.DoBeforeEntering(ed);
        }

    }

}
