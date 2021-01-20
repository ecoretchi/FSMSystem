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

        protected override void DoBeforeEntering(FSMStateIOData inputData)
        {
            if (typeof(IDLEStateOutData) == inputData.GetType())
            {
                Debug.Log("FSMState: Do some for IDLE");
            }

            if (typeof(SomeStateTutorialOutData) == inputData.GetType())
            {
                SomeStateTutorialOutData sst = (SomeStateTutorialOutData)inputData;

                Debug.Log("FSMState: Do some for SomeStateTutorial");
            }

            if (inputData.stateID == Move.stateID 
                && inputData.ed.eventName != GameEventNames.soldierCursorClick)
            {

            }
            else
            {
                ActionsController.Instance.OnSoldierSelected();
            }
        }

        protected override void DoBeforeLeaving(ref FSMStateIOData output)
        {
            
        }

        protected override void HandleEvents(EventHandlers eventHandlers)
        {
            eventHandlers
                .AddListener(InputEventNames.lmbDown, OnLMBDown);
        }

       
        void OnLMBDown(EventData ed)
        {
            var actions = ActionsController.Instance;
            var selectedSoldier = actions.SelectedSoldier;
            var hoveredSoldier = actions.LastHoverSoldier;


            if (hoveredSoldier)
            {
                if (selectedSoldier == hoveredSoldier)
                {
                    ActionsController.Instance.OnSoldierUnselected();

                    EventsMessanger.InvokeEvent(GameEventNames.cancelButton, ed);
                }
                else
                {
                    ActionsController.Instance.OnSoldierSelected();
                }
                return;
            }

            if (selectedSoldier)
            {
                selectedSoldier.TryMoveToPosition();
            }
           
        }
    }
}