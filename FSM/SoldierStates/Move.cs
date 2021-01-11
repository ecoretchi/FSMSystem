using System.Collections;
using UnityEngine;

class SoldierMoveEventData : EventData
{
    public SoldierController soldierController;
    public Vector3 globPosOnMap;
}
namespace AiGame.FSM.SoldierStates
{
    public class Move : FSMState
    {
        public static readonly string stateID = "SoldierMove";

        SoldierController soldierController;
        public Move() : base(stateID)
        {

        }

        public override void DoBeforeEntering(EventData ed)
        {
            SoldierMoveEventData eventData = ((SoldierMoveEventData)ed);
            soldierController = eventData.soldierController;
            soldierController.MoveTo(eventData.globPosOnMap);

            EventsMessanger.SubscribeToEvent(GameEventNames.soldierMoveDone, OnMoveDone); 
        }

        public override void DoBeforeLeaving(EventData ed)
        {
            EventsMessanger.UnsubscribeFromEvent(GameEventNames.soldierMoveDone, OnMoveDone);
        }

        void OnMoveDone(EventData ed)
        {
            
        }

    }
}