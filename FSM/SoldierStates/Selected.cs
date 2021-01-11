using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace AiGame.FSM.SoldierStates
{
    public class Selected : FSMState
    {
        public static readonly string stateID = "SoldierSelected";
        public Selected() : base(stateID)
        {

        }

        private SoldierController selectedSoldierController;
        private SoldierController lastSoldierController;
        private Vector3 globPosOnMap;



        public override void DoBeforeEntering(EventData ed)
        {
            Assert.IsNotNull(ed);

            SoldierCursorClickEventData eventData = ((SoldierCursorClickEventData)ed);
            selectedSoldierController = eventData.soldierController;
            selectedSoldierController.SetSelected(true);
            lastSoldierController = selectedSoldierController;

            EventsMessanger.SubscribeToEvent(InputEventNames.lmbDown, OnLMBDown);
            EventsMessanger.SubscribeToEvent(GameEventNames.soldierCursorEnter, OnSoldierCursourEnter);
            EventsMessanger.SubscribeToEvent(GameEventNames.soldierCursorRelease, OnSoldierCursourRelease);
            EventsMessanger.SubscribeToEvent(GameEventNames.cursorMapChanged, OnCursorMap);
        }

        public override void DoBeforeLeaving(EventData ed)
        {
            selectedSoldierController.SetSelected(false);
            selectedSoldierController = null;
            EventsMessanger.UnsubscribeFromEvent(InputEventNames.lmbDown, OnLMBDown);
            EventsMessanger.UnsubscribeFromEvent(GameEventNames.soldierCursorEnter, OnSoldierCursourEnter);
            EventsMessanger.UnsubscribeFromEvent(GameEventNames.soldierCursorRelease, OnSoldierCursourRelease);
        }

        void OnCursorMap(EventData ed)
        {
            MapCursorEvent eventData = (MapCursorEvent)ed;
            globPosOnMap = eventData.globPosOnMap;
        }

        void OnSoldierCursourEnter(EventData ed)
        {
            SoldierCursorEnterEventData eventData = ((SoldierCursorEnterEventData)ed);
            lastSoldierController = eventData.soldierController;
        }

        void OnSoldierCursourRelease(EventData ed)
        {
            lastSoldierController = null;
        }

        void OnLMBDown(EventData ed)
        {
            if (lastSoldierController != null)
            {
                if (selectedSoldierController == lastSoldierController)
                {
                    EventsMessanger.InvokeEvent(GameEventNames.cancelButton, ed);
                }
                else
                {
                    selectedSoldierController.SetSelected(false);
                    selectedSoldierController = lastSoldierController;
                    selectedSoldierController.SetSelected(true);
                }
                return;
            }

            EventsMessanger.InvokeEvent(GameEventNames.soldierMove, new SoldierMoveEventData()
            {
                soldierController = selectedSoldierController,
                globPosOnMap = globPosOnMap
            });


        }
    }
}