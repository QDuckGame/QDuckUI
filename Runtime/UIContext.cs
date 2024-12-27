using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;
namespace QDuck.UI
{
    public class UIContext:MonoBehaviour
    {
        internal UIPool Pool { get; private set; }

        private List<UIPanel> _panelFreeList = new List<UIPanel>();
        private List<UIPanel> _panelDisplayQueue = new List<UIPanel>();
        private List<UIPanel> _panelHideStack = new List<UIPanel>();
        private List<UIPanel> _tmpList = new List<UIPanel>();

        public List<UIPanel> PanelDisplays => _panelDisplayQueue;

        public static UIContext Create(string name = "[UIContext]")
        {
            GameObject go = new GameObject(name);
            GameObject.DontDestroyOnLoad(go);
            UIContext context = go.AddComponent<UIContext>();
            context.Pool = new UIPool(context);
            return context;
        }

        public UIPanel OpenPanel(Type type ,object data)
        {
            UIContext context = this;
            UIPanel curPanel = null;
            int startIndex = _panelHideStack.Count - 1;
            _tmpList.Clear();
            for (int i = startIndex; i >= 0; i--)
            {
                if (curPanel == null) //not find
                {
                    if (_panelHideStack[i].GetType() == type)
                    {
                        startIndex = i;
                        curPanel = _panelHideStack[i];
                        _tmpList.Add(curPanel);
                    }
                }
                else
                {
                    if (_panelHideStack[i].StackInfo.Priority >= curPanel.StackInfo.Priority)
                        break;
                    _tmpList.Add(_panelHideStack[i]);
                }
            }

            if (_tmpList.Count > 0) _panelHideStack.RemoveRange(startIndex - _tmpList.Count + 1, _tmpList.Count);
            if (curPanel == null)
                curPanel = Pool.Get(type);

            int needRemoveCount = 0;
            for (int i = _panelHideStack.Count - 1; i >= 0; i--)
            {
                if (_panelHideStack[i].StackInfo.Priority < curPanel.StackInfo.Priority)
                {
                    needRemoveCount++;
                }
                else
                {
                    break;
                }
            }

            if (needRemoveCount > 0)
                _panelHideStack.RemoveRange(_panelHideStack.Count - needRemoveCount, needRemoveCount);

            HandleDisplayToHideStack(curPanel);
            foreach (var panel in _tmpList)
            {
                OpenPanel(panel,null);
                _panelDisplayQueue.Add(panel);
            }
            OpenPanel(curPanel,data);   
            return curPanel;
        }

        private void OpenPanel(UIPanel panel,object data)
        {
            if (panel.State == UIState.None)
            {
                panel.Create((p) =>
                {
                    panel.Open(data,true);
                });
            }else if(panel.State != UIState.Opening && panel.State != UIState.Opened)
            {
                panel.Open(data,true);
            }
        }

        private void HandleDisplayToHideStack(UIPanel curPanel)
        {
            if (curPanel.StackInfo.Compare)
            {
                for (int i = _panelDisplayQueue.Count - 1; i >= 0; i--)
                {
                    var panel = _panelDisplayQueue[i];
                    if (panel.StackInfo.Priority > curPanel.StackInfo.Priority) break;
                    if (panel.StackInfo.Priority == curPanel.StackInfo.Priority)
                    {
                        if (!panel.StackInfo.Compare) continue;
                        for (int j = _panelDisplayQueue.Count - 1; j >= i; j--)
                        {
                            panel = _panelDisplayQueue[j];
                            bool curRootNode = (j == i) && (panel.StackInfo.Priority == curPanel.StackInfo.Priority);
                            if (!panel.NeedRetain && !curRootNode)
                            {
                                panel.Destroy();
                            }

                            if (panel.NeedRetain || curRootNode)
                            {
                                if (panel.StackInfo.StackType == PanelStackType.Push)
                                {
                                    panel.Close(true);
                                    _panelHideStack.Add(panel);
                                }
                                else
                                {
                                    panel.Destroy();
                                }
                            }
                        }

                        _panelDisplayQueue.RemoveRange(i, _panelDisplayQueue.Count - i);
                        break;
                    }
                }
            }

            if (curPanel.StackInfo.StackType >= PanelStackType.SinglePush)
            {
                _panelDisplayQueue.Add(curPanel);
            }
            else
            {
                _panelFreeList.Add(curPanel);
            }
        }


        public void ClosePanel(UIPanel uiPanel)
        {
            if (_panelDisplayQueue == null) return;
            if (_panelFreeList.Contains(uiPanel))
            {
                uiPanel.Destroy();
                _panelFreeList.Remove(uiPanel);
                return;
            }

            if (!uiPanel.StackInfo.Compare)
            {
                uiPanel.Destroy();
                _panelDisplayQueue.Remove(uiPanel);
            }
            else
            {
                int startIndex = 0;
                for (int i = 0; i < _panelDisplayQueue.Count; i++)
                {
                    var panel = _panelDisplayQueue[i];
                    if (panel == uiPanel)
                    {
                        startIndex = i;
                        panel.Destroy();
                        _panelDisplayQueue.Remove(panel);
                        break;
                    }

                    if (i > startIndex)
                    {
                        if (panel.StackInfo.Priority < uiPanel.StackInfo.Priority)
                        {
                            panel.Destroy();
                            _panelDisplayQueue.Remove(panel);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (uiPanel.StackInfo.Compare && _panelHideStack.Count > 0)
                {
                    var panel = _panelHideStack[^1];
                    if (panel.StackInfo.Priority == uiPanel.StackInfo.Priority)
                    {
                        OpenPanel(panel,null);
                        _panelDisplayQueue.Add(panel);
                        int counter = 1;
                        for (int i = _panelHideStack.Count - 2; i >= 0; i--)
                        {
                            panel = _panelHideStack[i];
                            if (panel.StackInfo.Priority < uiPanel.StackInfo.Priority)
                            {
                                OpenPanel(panel,null);
                                _panelDisplayQueue.Add(panel);
                                counter++;
                            }
                            else break;
                        }

                        _panelHideStack.RemoveRange(_panelHideStack.Count - counter, counter);
                    }
                }
            }
        }
        

    }
}