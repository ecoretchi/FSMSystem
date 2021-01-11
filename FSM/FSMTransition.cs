using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace AiGame.FSM
{
    public class FSMTransition
    { 
        public string ToState { get; private set; }

        public int Priority { get; private set; }

        protected List<FSMCondition> conditions = new List<FSMCondition>();

        public FSMTransition(string toState, int priority)
        {
            this.ToState = toState;
            this.Priority = priority;
        }
        
        public bool IsCondition(string name)
        {
            foreach(var c in conditions)
            {
                if (c.Name == name)
                    return true;
            }
            return false;
        }

        public void AddCondition(FSMCondition condition)
        {
            conditions.Add(condition);
        }
        /// <summary>
        ///  Remove all condition entries of the same name, and if all = false, only first occur in list
        /// </summary>
        public void RemoveCondition(string name, bool all = false) 
        {
            List<FSMCondition> fsmConditionsToRemove = new List<FSMCondition>();
            foreach (var c in conditions)
            {
                if (c.Name == name)
                {
                    c.Remove();
                    fsmConditionsToRemove.Add(c);
                    if (!all)
                        break;
                }
            }
            foreach (var c in fsmConditionsToRemove)
                conditions.Remove(c);
        }
        public bool DoCheck()
        {
            foreach(var c in conditions)
            {
                if (c.DoCheck())
                    return true;
            }
            return false;
        }
    }
}