using System.Collections;
using UnityEngine;

public class SoldierMoveDoneEventData : EventData
{
    public SoldierController soldierController;
    public SoldierController lastCursorSoldierController;
}
namespace AiGame.FSM.SoldierStates
{
    public class MoveStateOutData : FSMStateIOData
    { }

    public class Move : FSMState
    {
        public static readonly string stateID = "SoldierMove";

        public Move() : base(stateID)
        {

        }

        protected override void DoBeforeEntering(FSMStateIOData input)
        {
            var selectedSC = ActionsController.Instance.SelectedSoldier;
        }

        protected override void DoBeforeLeaving(ref FSMStateIOData output)
        {
            output = new MoveStateOutData();
        }

        protected override void HandleEvents(EventHandlers eventHandlers)
        {
            eventHandlers
                .AddListener(GameEventNames.soldierMoveEnd, OnMoveEnd)
                .AddListener(InputEventNames.lmbDown, OnLMBDown);
        }

        void OnLMBDown(EventData ed)
        {
            var sc = ActionsController.Instance.LastHoverSoldier;
            if (sc != null)
            {
                EventsMessanger.InvokeEvent(GameEventNames.soldierCursorClick, new SoldierEventData(sc));
            }
        }

        void OnMoveEnd(EventData ed)
        {
            EventsMessanger.InvokeEvent(GameEventNames.soldierMoveDone, ed);
        }

    }
}