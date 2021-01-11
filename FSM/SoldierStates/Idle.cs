using System.Collections;
using UnityEngine;

namespace AiGame.FSM.SoldierStates
{
    public class Idle : FSMState
    {
        public static readonly string stateID = "SoldierIdle";
        public Idle():base(stateID)
        {

        }

        SoldierController lastSoldierController;

        public override void DoBeforeEntering(EventData ed)
        {
            EventsMessanger.SubscribeToEvent(InputEventNames.lmbDown, OnLMBDown);

            EventsMessanger.SubscribeToEvent(GameEventNames.soldierCursorEnter, OnSoldierCursourEnter);
            EventsMessanger.SubscribeToEvent(GameEventNames.soldierCursorRelease, OnSoldierCursourRelease);
        }

        public void OnSoldierCursourEnter(EventData ed)
        {
            SoldierCursorEnterEventData eventData = (SoldierCursorEnterEventData)ed;
            lastSoldierController = eventData.soldierController;
        }

        public void OnSoldierCursourRelease(EventData ed)
        {
            lastSoldierController = null;
        }

        public override void DoBeforeLeaving(EventData ed)
        {
            EventsMessanger.UnsubscribeFromEvent(InputEventNames.lmbDown, OnLMBDown);

            EventsMessanger.UnsubscribeFromEvent(GameEventNames.soldierCursorEnter, OnSoldierCursourEnter);
            EventsMessanger.UnsubscribeFromEvent(GameEventNames.soldierCursorRelease, OnSoldierCursourRelease);
        }

        void OnLMBDown(EventData ed)
        {
            if (lastSoldierController != null)
            {
                EventsMessanger.InvokeEvent(GameEventNames.soldierCursorClick, new SoldierCursorClickEventData()
                {
                    soldierController = lastSoldierController
                });
            }
        }
    }
}