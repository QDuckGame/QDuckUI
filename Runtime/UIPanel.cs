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

        protected IUIBehavior _uiBehaviour { get; private set; }

        private string _uiPath;
        private int _index;

        public bool IsActive => State == UIState.Opened;

        public UIPanelInfo Info { get; private set; }

        private List<UIPanel> _children;
        
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
                _context.LoadUIView(Info.Path, (uiBehaviour) =>
                {
                    _uiBehaviour = uiBehaviour;
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
            _uiBehaviour.SetActive(true);
            if(isPlayTween)OnPlayOpenTween();
            if (_children != null)
            {
                foreach (var child in _children)
                {
                    child.OnPlayOpenTween();
                }
                foreach (var child in _children)
                {
                    child.Open(isPlayTween);
                }
            }
            OnOpen();

            State = UIState.Opened;
        }

        internal void Awake()
        {
            SetFullScreenBlock(Info.BlockRaycast);
            if (this is IUIComponentBinder binder)
            {
                binder.BindComponents(_uiBehaviour,this);
            }
            if(_children!=null)
                foreach (var child in _children)child.Awake();
            OnAwake();
            State = UIState.Awaked;
        }

        internal void Close(bool isPlayTween, Action complete = null)
        {
            if(State<UIState.Opened)return;
            State = UIState.Closing;
            OnPlayCloseTween(() =>
            {
                if(_children!=null)
                    foreach (var child in _children)child.Close(isPlayTween);
                OnClose();
                _uiBehaviour.SetActive(false);
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
            if (_uiBehaviour != null)
            {
                _uiBehaviour.Destroy();
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
        
        #region Panel children

        public UIPanel GetChildPanel(IUIBehavior uiBehavior)
        {
            if(_children == null)return null;
            foreach (var child in _children)
            {
                if(child._uiBehaviour == uiBehavior)return child;
            }
            return null;
        }

        public void AddChild(UIPanel child)
        {
            if(GetChildPanel(child._uiBehaviour)!=null)return;
            if(_children == null)_children = new List<UIPanel>();
            _children.Add(child);
        }
        
        public T BindBehaviour<T>(IUIBehavior  uiBehavior)
            where T : UIPanel, new()
        {
            UIPanel child = GetChildPanel(uiBehavior);
            if (child !=null)
            {
                return child as T;
            }
            
            T t = new T();
            t._context = _context;
            t.State = UIState.Awaking;
            t.BeforeCreate();
            t._uiBehaviour = uiBehavior;
            if(_children == null)_children = new List<UIPanel>();
            _children.Add(t);
            if(State >= UIState.Awaked)t.Awake();
            if(State >= UIState.Opened ) t.Open( false);
            if(State >= UIState.Closed) t.Close(false);
            return t;
        }
        
        public void CreateChild<T>(string uiPath,Action<T> callback =null)
            where T : UIPanel, new()
        {
            _context.LoadUIView(uiPath, (uiBehaviour) =>
            {
                callback?.Invoke( BindBehaviour<T>(uiBehaviour));
            });
        }

        #endregion

    }
}