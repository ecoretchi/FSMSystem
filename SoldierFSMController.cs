using System.Collections;
using UnityEngine;
using AiGame;
using AiGame.FSM;
using AiGame.FSM.SoldierStates;

public class SoldierFSMController
{
    readonly FSMBuilder fsmBuilder = new FSMBuilder();

    public SoldierFSMController()
    {
        fsmBuilder

        .AddStatesBegin
            .StateDefault(new Idle())
            .State(new Selected())
            .State(new Move())
        .AddStatesEnd

        .Transition(Idle.stateID, Selected.stateID)
            .FrwdEvent(GameEventNames.soldierCursorClick)
            .BackEvent(GameEventNames.cancelButton)

        .Transition(Selected.stateID, Move.stateID)
            .FrwdEvent(GameEventNames.soldierMove);
    }


    public void Entry()
    {
        fsmBuilder.Entry();
    }
}
