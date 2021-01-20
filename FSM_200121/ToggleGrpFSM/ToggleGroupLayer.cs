using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AiGame.FSM
{

    public interface IToggleGroup { }

    public class ToggleGroupLayer<I> : FSMState
        where I : IToggleGroup
    {
        readonly ToggleGroupSystem tgSystem;
        public ToggleGroupLayer(string layerName) : base(layerName)
        {
            tgSystem = new ToggleGroupSystem(layerName);
        }
        public I Current
        {
            get
            {
                if (tgSystem.CurrentTab.TabObject is IToggleGroup)
                    return (I)tgSystem.CurrentTab.TabObject;
                throw new System.Exception("Interface IToggleGroup for [ToggleGroupLayer<I>:CurrentTab.TabObject] - is missing");
            }
        }
        public int AddNewTab<T>(T tabObj) => tgSystem.AddNewTab(tabObj);
        public void SwitchTo(int tabNum, EventData ed = null) => tgSystem.SwitchTo(tabNum, ed);
    }

    public class ToggleGroupLayer<I, E> : FSMState
         where I : IToggleGroup
         where E : System.Enum
    {
        readonly ToggleGroupSystem tgSystem;

        readonly Dictionary<E, int> mapEnum = new Dictionary<E, int>();

        public ToggleGroupLayer(string layerName) : base(layerName)
        {
            tgSystem = new ToggleGroupSystem(layerName);
        }
        public I Current
        {
            get
            {
                if (tgSystem.CurrentTab.TabObject is IToggleGroup)
                    return (I)tgSystem.CurrentTab.TabObject;
                throw new System.Exception("Interface IToggleGroup for [ToggleGroupLayer<I,E>:CurrentTab.TabObject] - is missing");
            }
        }

        public bool AddNewTab<T>(E tabNum, T tabObj)
        {
            if (mapEnum.ContainsKey(tabNum))
            {
                Debug.LogException(new System.Exception("Already exist"));
                return false;
            }
            int idx = tgSystem.AddNewTab(tabObj);
            mapEnum.Add(tabNum, idx);
            return true;
        }

        public void SwitchTo(E tabENum, EventData ed = null)
        {
            if (mapEnum.TryGetValue(tabENum, out int tabIdx))
            {
                if (tgSystem.CurrentTab == null)
                {
                    Debug.LogFormat("ToggleGroupLayer<{0}, {1}>.SwitchTo({2}) Current tab is null",
                        typeof(I).ToString(), typeof(E).ToString(), tabIdx);
                }
                tgSystem.SwitchTo(tabIdx, ed);
            }
        }

    }

}