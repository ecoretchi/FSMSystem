using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AiGame.FSM
{
    struct FSMTransitionsBatch
    {
        public List<FSMTransition> list;
    }
    public class FSMStateData 
    {
        public FSMState state;
        
        private readonly SortedDictionary<int, FSMTransitionsBatch> transitions = new SortedDictionary<int, FSMTransitionsBatch>();

        public FSMTransition GetTransition( string toState, int priority)
        {
            var transition = GetTransitionToState(toState);

            if (transition == null)
            {
                transition = new FSMTransition(toState, priority);
                if (transitions.TryGetValue(priority, out FSMTransitionsBatch val))
                {
                    val.list.Add(transition);
                }
                else
                {
                    var tl = new FSMTransitionsBatch() { list = new List<FSMTransition>() };
                    tl.list.Add(transition);
                    transitions.Add(priority, tl);
                }
            }
            return transition;
        }

        public string GetOutputStateID(string name)
        {
            foreach (var  it in transitions)
            {
                foreach (var transition in it.Value.list)
                {
                    if (transition.IsCondition(name))
                    {
                        return transition.ToState;
                    }
                }
            }
            return "";
        }

        FSMTransition GetTransitionToState(string stateID)
        {
            foreach (var it in transitions)
            {
                foreach (var transition in it.Value.list)
                {
                    if (transition.ToState == stateID)
                    {
                        return transition;
                    }
                }
            }
            return null;
        }
    }
}