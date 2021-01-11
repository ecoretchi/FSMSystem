using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AiGame.FSM
{
    public enum FSMConditionValueCheck
    {
        Greater,
        Less,
        Equal,
        NotEqual
    }

    public abstract class FSMCondition
    {
        public string Name { get; private set; }
        public abstract bool DoCheck();
        public virtual void Remove() { }

        public FSMCondition(string name)
        {
            this.Name = name;
        }
    }

    public class FSMValueCondition : FSMCondition
    {
        public delegate float FSMSystemValue(string name);
        public float Value { get; private set; }
        public FSMConditionValueCheck ValueCheck { get; private set; }

        readonly FSMSystemValue fsmSystemValue;

        public FSMValueCondition(string name, float val, FSMConditionValueCheck valueCheck, FSMSystemValue fsmSystemValue) 
            : base(name)
        {
            this.Value = val;
            this.ValueCheck = valueCheck;
            this.fsmSystemValue = fsmSystemValue;
        }

        public override bool DoCheck()
        {
            return CheckValue(Value);
        }

        bool CheckValue(float val)
        {
            float fsmVal = fsmSystemValue(Name);
            switch (ValueCheck)
            {
                case FSMConditionValueCheck.Greater:
                    return fsmVal > val;
                case FSMConditionValueCheck.Less:
                    return fsmVal < val;
                case FSMConditionValueCheck.Equal:
                    return Mathf.Approximately(fsmVal, val);
                case FSMConditionValueCheck.NotEqual:
                    return !Mathf.Approximately(fsmVal, val);
                default:
                    break;
            }
            return false;
        }
    }

    public class FSMBoolCondition : FSMCondition
    {

        public delegate bool FSMSystemBoolValue(string name);
    
        public bool BoolValue { get; private set; }

        readonly FSMSystemBoolValue fsmSystemBoolValue;

        public FSMBoolCondition(string name, bool val, FSMSystemBoolValue fsmSystemBoolValue)
            : base(name)
        {
            this.BoolValue = val;
            this.fsmSystemBoolValue = fsmSystemBoolValue;
        }

        public override bool DoCheck()
        {
            return fsmSystemBoolValue(Name) == BoolValue;
        }
    }

    public class FSMTriggerCondition : FSMCondition
    {
        public delegate bool FSMSystemTriggerValue(string name);

        readonly FSMSystemTriggerValue fsmSystemTriggerValue;

        public FSMTriggerCondition(string name, FSMSystemTriggerValue fsmSystemTriggerValue)
            : base(name)
        {
            this.fsmSystemTriggerValue = fsmSystemTriggerValue;
        }
        public override bool DoCheck()
        {
            throw new System.NotImplementedException();
        }
    }


    public class FSMEventCondition : FSMCondition
    {

        public FSMEventCondition(string name)
            : base(name)
        {

            EventsMessanger.SubscribeToEvent(name, OnEvent);
        }
        ~FSMEventCondition()
        {
            Remove();
        }

        public override void Remove()
        {
           EventsMessanger.UnsubscribeFromEvent(Name, OnEvent);
        }

        void OnEvent(EventData ed)
        {
            EventsMessanger.InvokeEvent(FSMSystem.performTransitionEventName, new FSMEventData
            {
                transitionName = Name,
                ed = ed
            });
        }

        public override bool DoCheck()
        {
            return false;
        }
    }

}