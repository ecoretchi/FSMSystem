using System.Collections;
using UnityEngine;
using AiGame;
using AiGame.FSM;
using AiGame.FSM.SoldierStates;

public class SoldierFSM : FSMLayer
{
    public SoldierFSM() : base("FSMSoldierLayer")
    {
    }

    protected override void Init(FSMBuilder fsmBuilder)
    {
        
        fsmBuilder

        .FSMEventHandlersBegin
            .Suspend(GameHUDEventNames.HoverModeBegin)
            .Resume(GameHUDEventNames.HoverModeEnd)
        .FSMEventHandlersEnd

        .AddStatesBegin
            .StateDefault(new Idle())
            .State(new Selected())
            .State(new Move())
            .State(new SomeStateTutorial())
        .AddStatesEnd

        .AddTrnsitionsBegin
            .Transition(Idle.stateID, SomeStateTutorial.stateID)
                .FrwdEvent(GameEventNames.soldierCursorClick2)

            .Transition(SomeStateTutorial.stateID, Selected.stateID)
                .FrwdEvent(GameEventNames.soldierCommandA)

            .Transition(Idle.stateID, Selected.stateID)
                .FrwdEvent(GameEventNames.soldierCursorClick)
                .BackEvent(GameEventNames.cancelButton)

            .Transition(Selected.stateID, Move.stateID)
                .FrwdEvent(GameEventNames.soldierMoveBegin)
                .BackEvent(GameEventNames.soldierCursorClick)
                .BackEvent(GameEventNames.soldierMoveDone)
        .AddTrnsitionsEnd

        .BuilderFinish();
    }

}
