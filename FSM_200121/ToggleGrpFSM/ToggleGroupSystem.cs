using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AiGame.FSM
{
    public class ToggleGroupSystem
    {
        readonly List<ToggleTab> tabs = new List<ToggleTab>();

        public ToggleTab CurrentTab { get; private set; }
        readonly string ID;
        public ToggleGroupSystem(string ID)
        {
            this.ID = ID;
        }
        string NextTabName { get { return ID + "_TAB_" + tabs.Count; } }
        public int AddNewTab<T>(T tabObj)
        {
            var tab = new ToggleTab(NextTabName, tabObj);
            int tabNum = tabs.Count;
            tabs.Add(tab);
            return tabNum;
        }


        public void SwitchTo(int tabNum, EventData ed)
        {

            try
            {
                var nextTab = tabs[tabNum];

                var stateIOData = new FSMStateIOData()
                {
                    ed = ed,
                    stateID = nextTab.StateID,
                    userObj = nextTab.TabObject
                };

                if (CurrentTab == null)
                {
                    //Debug.LogFormat("FSMToggleGroup[SwitchTo]:{0} Current tab is null", tabNum);
                    stateIOData.stateID = null;
                    CurrentTab = nextTab;
                    CurrentTab.Enter(stateIOData);
                    return;
                }

                CurrentTab.Leave(ref stateIOData);

                stateIOData.ed = ed;
                stateIOData.stateID = CurrentTab.StateID;

                CurrentTab = nextTab;

                CurrentTab.Enter(stateIOData);
            }
            catch (System.IndexOutOfRangeException)
            {
                return;
            }
        }

        public T SwitchTo<T>(int tabNum, EventData ed) where T : class
        {
            SwitchTo(tabNum, ed);
            return (T)CurrentTab.TabObject;
        }
    }

}