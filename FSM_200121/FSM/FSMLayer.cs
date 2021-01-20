using System.Collections;
using UnityEngine;

namespace AiGame.FSM
{
    public abstract class FSMLayer : FSMState
    {
        readonly FSMBuilder fsmBuilder;
        readonly string prefixPerform = "Perform_";
        bool IsInitialized { get; set; }

        public FSMLayer(string layerID) : base(layerID)
        {
            fsmBuilder = new FSMBuilder(prefixPerform + layerID);
        }

        protected abstract void Init(FSMBuilder fsmBuilder);

        protected override void DoBeforeEntering(FSMStateIOData inputData)
        {
            CheckInit();
            fsmBuilder.Resume();
        }

        protected override void DoBeforeLeaving(ref FSMStateIOData outputData)
        {
            fsmBuilder.Suspend();
        }

        void CheckInit()
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                Init(fsmBuilder);
            }
        }

        public void Entry()
        {
            CheckInit();
            fsmBuilder.Entry();
        }

    }
}