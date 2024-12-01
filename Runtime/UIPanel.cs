using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;
namespace QDuck.UI
{
    public enum UIState
    {
        None = 0,
        Awaking,
        Awaked,
        Opening,
        Opened,
        Closing,
        Closed,
        Recycled,
        Destroyed
    }

    public abstract class UIPanel
    {
        protected UIContext _context;

        public int UIIndex { get; internal set; }
        public UIState State { get; private set; }
        public virtual UIPanelStackInfo StackInfo { get; }

        public virtual int CacheCount => UISetting.DefaultCacheCount;

        protected IUIView _iView { get; private set; }

        private string _uiPath;
        private int _index;

        public bool IsActive => State == UIState.Opened;

        public UIPanelInfo Info { get; private set; }

        private Dictionary<IUIView,UIPanel> _panelPartsDic;

        private Dictionary<IUIView, UIPanel> PanelPartsDic
        {
            get
            {
                if(_panelPartsDic ==  null)
                    _panelPartsDic = new Dictionary<IUIView, UIPanel>();
                return _panelPartsDic;
            }
        }
        
        internal void Init(UIPanelInfo info, UIContext context)
        {
            _context = context;
            Info = info;
        }
        
        internal void Create(Action complete)
        {
            if (State == UIState.None)
            {
                State = UIState.Awaking;
                BeforeCreate();

                LoadUIView(Info.Path, (IUIView iview) =>
                {
                    _iView = iview;
                    Awake();
                    complete?.Invoke();
                });
            }
            else
            {
                complete?.Invoke();
            }
        }

        internal void Open(bool isPlayTween = true)
        {
            if(State<UIState.Awaked)return;
            State = UIState.Opening;
            _iView.SetActive(true);
            if(isPlayTween)OnPlayOpenTween();
            OnOpen();
            if (_panelPartsDic != null)
            {
                foreach (var part in _panelPartsDic)
                {
                    part.Value.OnPlayOpenTween();
                    part.Value.Open(isPlayTween);
                }
            }
            State = UIState.Opened;
        }

        internal void Awake()
        {
            SetFullScreenBlock(Info.BlockRaycast);
            OnAwake();
            if(_panelPartsDic!=null)
                foreach (var part in _panelPartsDic)part.Value.Awake();
            State = UIState.Awaked;
        }

        internal void Close(bool isPlayTween, Action complete = null)
        {
            if(State<UIState.Opened)return;
            State = UIState.Closing;
            OnPlayCloseTween(() =>
            {
                OnClose();
                _iView.SetActive(false);
                if(_panelPartsDic!=null)
                    foreach (var part in _panelPartsDic)part.Value.Close(isPlayTween);
                State = UIState.Closed;
                complete?.Invoke();
            });
        }

        internal void Destroy()
        {
            if (State < UIState.Closed)
            {
                Close(true, HandRecycleOrDestroy);
            }
            else
            {
                ForceDestroy();
            }
        }
        
        private void ForceDestroy()
        {
            if (_iView != null)
            {
                _iView.Destroy();
                OnDestroy();
            }
            State = UIState.Destroyed;
        }
        
        private void HandRecycleOrDestroy()
        {
            if (_context.Pool.TryRecycle(this))
            {
                OnRecycle();
                State = UIState.Recycled;
            }
            else
            {
                OnDestroy();
                State = UIState.Destroyed;
            }
        }

        protected virtual void BeforeCreate()
        {
        }

        protected virtual void OnPlayOpenTween( Action onFinish =null)
        {
            onFinish?.Invoke();
        }

        protected virtual void OnPlayCloseTween(Action onFinish =null)
        {
            onFinish?.Invoke();
        }
        
        protected virtual void OnAwake()
        {
        }
        
        protected virtual void OnOpen()
        {
        }
        
        protected virtual void AfterOpenTween()
        {
            
        }

        protected virtual void OnClose()
        {
        }

        protected virtual void OnRecycle()
        {
        }

        protected virtual void OnDestroy()
        {
        }

        protected virtual void SetFullScreenBlock( bool isBlock)
        {
            
        }

        protected virtual void LoadUIView(string uiName, Action<IUIView> callback)
        {
        }
        
        #region PanelParts

        protected T BindParts<T>(IUIView view)
            where T : UIPanel, new()
        {
            T target;
            if (PanelPartsDic.TryGetValue(view, out var value))
            {
                return value as T;
            }
            else
            {
                target = new T();
                target._context = _context;
                target.State = UIState.Awaking;
                target.BeforeCreate();
                target._iView = view;
                PanelPartsDic.Add(view, target);
                if(State >= UIState.Awaked)target.Awake();
                if(State >= UIState.Opened ) target.Open( false);
                if(State >= UIState.Closed) target.Close(false);
               
            }
            return target;
        }

        protected T CloneAndBindParts<T>(IUIView view)
            where T : UIPanel, new()
        {
            var cloneView = view.Clone();
            return BindParts<T>(cloneView);
        }
        
        protected void CreateAndBindParts<T,K>(string uiPath,Action<T> callback)
            where T : UIPanel<K>, new()
            where K:  IUIView, new()
        {
            _context.LoadUIView<K>(uiPath, (uiview) =>
            {
                callback?.Invoke( BindParts<T>(uiview));
            });
        }

        #endregion

    }

    public class UIPanel<T> : UIPanel
        where T : IUIView, new()
    {
        private T _view;

        protected T View
        {
            get
            {
                if (_view == null)
                {
                    _view = (T)_iView;
                }

                return _view;
            }
        }

        protected sealed override void LoadUIView(string uiName, Action<IUIView> callback)
        {
            _context.LoadUIView<T>(uiName, callback);
        }

    }
    
    
    
    
    
}