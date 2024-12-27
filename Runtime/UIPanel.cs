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
        public virtual string UIPath => $"{UIName}.prefab";
        public virtual bool NeedRetain => false;
        public virtual bool BlockRaycast => true;
        public virtual int SortOrder => 0;
        
        public string UIName { get; private set; }
        protected UIContext _context;
        protected object _data;

        public int UIIndex { get; internal set; }
        public UIState State { get; private set; }
        public virtual UIPanelStackInfo StackInfo { get; }
        public virtual int CacheCount => UISetting.DefaultCacheCount;
        public IUIBehavior UIBehaviour { get; private set; }
        private int _index;
        public bool IsActive => State == UIState.Opened;
        
        private UIPool _childPool;
        private List<UIPanel> _children;
        
        internal void Init( UIContext context)
        {
            _context = context;
            UIName = GetType().Name;
        }
        
        internal void Create(Action<UIPanel> complete)
        {
            if (State == UIState.None)
            {
                State = UIState.Awaking;
                BeforeCreate();
                LoadRes<GameObject>(UIPath, (prefab) =>
                {
                    GameObject go = GameObject.Instantiate(prefab);
                    if (go == null)
                    {
                        complete?.Invoke(null);
                        return;
                    }
                    go.transform.SetParent(_context.transform, false);
                    UIBehaviour = go.GetComponent<IUIBehavior>();
                    Awake();
                    complete?.Invoke(this);
                });
            }
            else
            {
                complete?.Invoke(this);
            }
        }
        
        protected virtual void LoadRes<T>(string path, Action<T> callback)
            where T : UnityEngine.Object
        {
            
        }
        
        protected virtual void ReleaseRes()
        {
            
        }

        public void Open(object data, bool isPlayTween = true)
        {
            if(State<UIState.Awaked)return;
            if(data!=null)_data = data;
            State = UIState.Opening;
            UIBehaviour.SetActive(true);
            if(isPlayTween)OnPlayOpenTween();
            if (_children != null)
            {
                foreach (var child in _children)
                {
                    child.OnPlayOpenTween();
                }
                foreach (var child in _children)
                {
                    child.Open(data,isPlayTween);
                }
            }
            OnOpen();

            State = UIState.Opened;
        }

        internal void Awake()
        {
            SetFullScreenBlock(BlockRaycast);
            if (this is IUIComponentBinder binder)
            {
                binder.BindComponents(UIBehaviour,this);
            }
            if(_children!=null)
                foreach (var child in _children)child.Awake();
            OnAwake();
            State = UIState.Awaked;
        }
        
        public void Refresh()
        {
            if (State == UIState.Opened)
            {
                OnRefresh();
            }
        }

        internal void Close(bool isPlayTween, Action complete = null)
        {
            if(State<UIState.Opened)return;
            _data = null;
            State = UIState.Closing;
            OnPlayCloseTween(() =>
            {
                if(_children!=null)
                    foreach (var child in _children)child.Close(isPlayTween);
                OnClose();
                UIBehaviour.SetActive(false);
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
            if (UIBehaviour != null)
            {
                OnDestroy();
                ReleaseRes();
                UIBehaviour.Destroy();
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
                ForceDestroy();
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

        protected virtual void OnRefresh()
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
                if(child.UIBehaviour == uiBehavior)return child;
            }
            return null;
        }

        public void AddChild(UIPanel child)
        {
            if(GetChildPanel(child.UIBehaviour)!=null)return;
            if(_children == null)_children = new List<UIPanel>();
            _children.Add(child);
        }
        
        public void RemoveChild(UIPanel child)
        {
            if (_children != null)
            {
                _children.Remove(child);
            }
            TryRecycle(child);
        }
        
        public T BindBehaviour<T>(IUIBehavior  uiBehavior)
        where T : UIPanel,new()
        {
            return BindBehaviour(typeof(T), uiBehavior) as T;
        }
        
        public UIPanel BindBehaviour(Type type, IUIBehavior  uiBehavior)
        {
            UIPanel child = GetChildPanel(uiBehavior);
            if (child !=null)
            {
                return child;
            }
            
            UIPanel p = Activator.CreateInstance(type) as UIPanel;
            p._context = _context;
            p.State = UIState.Awaking;
            p.BeforeCreate();
            p.UIBehaviour = uiBehavior;
            if(_children == null)_children = new List<UIPanel>();
            _children.Add(p);
            if(State >= UIState.Awaked)p.Awake();
            if(State >= UIState.Opened ) p.Open( null,false);
            if(State >= UIState.Closed) p.Close(false);
            return p;
        }
        
        public void CreateChild(Type type, Action<UIPanel> callback =null)
        {
            GetPanelFormPool(type).Create((panel) =>
            {
                callback?.Invoke(panel);
            });
        }

        public UIPanel GetPanelFormPool(Type type)
        {
            if(_childPool == null)_childPool = new UIPool(_context);
            return _childPool.Get(type);
        }

        public bool ContainsInPool(Type type)
        {
            if (_childPool == null) return false;
           return _childPool.Contains(type);
        }

        public bool TryRecycle(UIPanel uiPanel)
        {
            if(_childPool == null)_childPool = new UIPool(_context);
            return _childPool.TryRecycle(uiPanel);
        }

        #endregion

    }
}