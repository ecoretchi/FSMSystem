using System.Collections;
using UnityEngine;

namespace AiGame.FSM.SoldierStates
{
    public class SomeStateTutorialOutData : FSMStateIOData
    {

    }

    public class SomeStateTutorial : FSMState
    {
        public static readonly string stateID = "SomeStateTutorial";
        public SomeStateTutorial() : base(stateID)
        {

        }
        protected override void DoBeforeEntering(FSMStateIOData inputData)
        {
            //EventsMessanger.SubscribeToEvent(InputEventNames.lmbDown, OnLMBDown);
        }

        protected override void DoBeforeLeaving(ref FSMStateIOData outputData)
        {
            outputData = new SomeStateTutorialOutData();
            //EventsMessanger.UnsubscribeFromEvent(InputEventNames.lmbDown, OnLMBDown);
        }

        void OnLMBDown(EventData ed)
        {
            var sc = ActionsController.Instance.LastHoverSoldier;
            if (sc != null)
            {
                EventsMessanger.InvokeEvent(GameEventNames.soldierCommandA, new SoldierEventData(sc));
            }
        }
    }
}