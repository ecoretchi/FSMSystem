using System.Collections;
using UnityEngine;

namespace AiGame.FSM
{
    public interface ITabHandler
    {
    }
    interface IFSMSateHandleActivity : ITabHandler
    {
        void OnSuspend();
        void OnResume();
    }
    interface IFSMSateHandleEvents : ITabHandler
    {
        void HandleEvents(EventHandlers eventHandler);
    }
    interface IFSMSateHandleIOData : ITabHandler
    {
        void DoBeforeEntering(FSMStateIOData inputData);

        void DoBeforeLeaving(ref FSMStateIOData outputData);
    }


    public class ToggleTab : FSMState
    {
        public object TabObject { get; private set; }
        public ToggleTab(string stateName, object tabObj) : base(stateName)
        {
            this.TabObject = tabObj;
        }
        protected override void HandleEvents(EventHandlers eventHandler)
        {
            if (TabObject is IFSMSateHandleEvents)
            {
                var h = TabObject as IFSMSateHandleEvents;
                h.HandleEvents(eventHandler);
            }
        }
        protected override void OnSuspend()
        {
            if (TabObject is IFSMSateHandleActivity)
            {
                var h = TabObject as IFSMSateHandleActivity;
                h.OnSuspend();
            }
            base.OnSuspend();
        }
        protected override void OnResume()
        {
            base.OnResume();
            if (TabObject is IFSMSateHandleActivity)
            {
                var h = TabObject as IFSMSateHandleActivity;
                h.OnResume();
            }
        }
        protected override void DoBeforeEntering(FSMStateIOData inputData)
        {
            base.OnResume();
            if (TabObject is IFSMSateHandleIOData)
            {
                var h = TabObject as IFSMSateHandleIOData;
                h.DoBeforeEntering(inputData);
            }
        }
        protected override void DoBeforeLeaving(ref FSMStateIOData outputData)
        {
            base.OnResume();
            if (TabObject is IFSMSateHandleIOData)
            {
                var h = TabObject as IFSMSateHandleIOData;
                h.DoBeforeLeaving(ref outputData);
            }
        }

    }
}