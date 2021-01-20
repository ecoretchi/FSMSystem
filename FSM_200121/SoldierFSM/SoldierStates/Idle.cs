using System.Collections;
using UnityEngine;

namespace AiGame.FSM.SoldierStates
{

    public class IDLEStateOutData : FSMStateIOData
    {
        public string myData = "";
    }

    public class Idle : FSMState
    {
        public static readonly string stateID = "SoldierIdle";
        public Idle() : base(stateID)
        {
        }

        protected override void DoBeforeEntering(FSMStateIOData input)
        {
        
        }

        protected override void DoBeforeLeaving(ref FSMStateIOData output)
        {
            output = new IDLEStateOutData() { myData = "Hello" };
        }

        protected override void HandleEvents(EventHandlers eventHandlers) 
        {
            eventHandlers
                .AddListener(InputEventNames.lmbDown, OnLMBDown)
                .AddListener(InputEventNames.rmbDown, OnRMBDown);
        }

        void OnLMBDown(EventData ed)
        {
            var sc = ActionsController.Instance.LastHoverSoldier;
            if (sc != null)
            {
                EventsMessanger.InvokeEvent(GameEventNames.soldierCursorClick, new SoldierEventData(sc));
            }
        }

        void OnRMBDown(EventData ed)
        {
            var sc = ActionsController.Instance.LastHoverSoldier;
            if (sc != null)
            {
                EventsMessanger.InvokeEvent(GameEventNames.soldierCursorClick2, new SoldierEventData(sc));
            }
        }
    }
}