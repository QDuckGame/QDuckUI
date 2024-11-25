using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;
namespace QDuck.UI
{
    public class UIContext
    {
        private UIPool _uiPool;

        private List<UIPanel> _panelFreeList = new List<UIPanel>();
        private List<UIPanel> _panelDisplayQueue = new List<UIPanel>();
        private List<UIPanel> _panelHideStack = new List<UIPanel>();
        private List<UIPanel> _tmpList = new List<UIPanel>();

        private IUILoader _uiLoader;

        private Dictionary<string, UIPanelInfo> _panelInfoDic = new Dictionary<string, UIPanelInfo>();

        public UIContext(IUILoader uiLoader)
        {
            _uiPool = new UIPool(this);
            _uiLoader = uiLoader;
        }

        public int OpenPanel<T>(string uiName) where T : UIPanel, new()
        {
            UIContext context = this;
            UIPanel curPanel = null;
            int startIndex = _panelHideStack.Count - 1;
            _tmpList.Clear();
            for (int i = startIndex; i >= 0; i--)
            {
                if (curPanel == null) //not find
                {
                    if (_panelHideStack[i] is T)
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
                curPanel = _uiPool.Get<T>(GetPanelInfo(uiName));

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
                panel.Open(context);
                _panelDisplayQueue.Add(panel);
            }

            curPanel.Open(this);
            return curPanel.UIIndex;
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
                            if (!panel.Info.NeedRetain && !curRootNode)
                            {
                                _uiPool.Recycle(panel);
                            }

                            if (panel.Info.NeedRetain || curRootNode)
                            {
                                if (panel.StackInfo.StackType == PanelStackType.Push)
                                {
                                    panel.Close();
                                    _panelHideStack.Add(panel);
                                }
                                else
                                {
                                    _uiPool.Recycle(panel);
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
                _panelFreeList.Remove(uiPanel);
                _uiPool.Recycle(uiPanel);
                return;
            }

            if (!uiPanel.StackInfo.Compare)
            {
                _panelDisplayQueue.Remove(uiPanel);
                _uiPool.Recycle(uiPanel);
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
                        panel.Close();
                        _uiPool.Recycle(panel);
                        _panelDisplayQueue.Remove(panel);
                        break;
                    }

                    if (i > startIndex)
                    {
                        if (panel.StackInfo.Priority < uiPanel.StackInfo.Priority)
                        {
                            panel.Close();
                            _uiPool.Recycle(panel);
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
                        panel.Open(this);
                        _panelDisplayQueue.Add(panel);
                        int counter = 1;
                        for (int i = _panelHideStack.Count - 2; i >= 0; i--)
                        {
                            panel = _panelHideStack[i];
                            if (panel.StackInfo.Priority < uiPanel.StackInfo.Priority)
                            {
                                panel.Open(this);
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

        #region PanelDic

        public void RegisterPanelInfo(string name, UIPanelInfo panelInfo)
        {
            _panelInfoDic[name] = panelInfo;
        }

        public UIPanelInfo GetPanelInfo(string name)
        {
            if (!_panelInfoDic.ContainsKey(name))
            {
                Debug.LogError("error!dont config this ui name!");
            }

            return _panelInfoDic[name];
        }


        #endregion

        #region UILoader

        public void LoadUIView<T>(string uiName, Action<IUIView> callback)
            where T : IUIView, new()
        {
            _uiLoader.Get<T>(uiName, callback);
        }

        #endregion

    }
}