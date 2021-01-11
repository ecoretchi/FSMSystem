using System.Collections;
using UnityEngine;

namespace AiGame.FSM
{
    public class FSMProcessor : MBSingleton<FSMProcessor>
    {
        protected FSMSystem fsm;

        public void SetFSM(FSMSystem fsm)
        {
            this.fsm = fsm;
        }

        public void Update()
        {
            if (fsm != null)
                fsm.CurrentState.Processing();
        }
    }
}