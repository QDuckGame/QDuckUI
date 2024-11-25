using System;
using Demo.Core;
using UnityEngine;
using Object = System.Object;
namespace QDuck.UI
{
    public enum UIState
    {
        Ready,
        Created,
        Opened,
        Closed,
        Recycle,
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
        private bool _isInit = false;
        private int _index;

        public bool IsActive => State == UIState.Opened;

        public UIPanelInfo Info { get; private set; }

        internal void Init(UIPanelInfo info, UIContext context)
        {
            _context = context;
            Info = info;
        }

        internal void Create(Action<IUIView> complete)
        {
            if (!_isInit)
            {
                State = UIState.Ready;
                BeforeCreate();

                LoadUIView(Info.Path, (IUIView iview) =>
                {
                    _isInit = true;
                    _iView = iview;
                    Awake();
                    complete?.Invoke(iview);
                });
            }
            else
            {
                complete?.Invoke(_iView);
            }
        }

        internal void Open(UIContext context)
        {
            Create((iview) =>
            {
                iview.SetActive(true);
                OnOpen();
                State = UIState.Opened;
            });
        }

        internal void Awake()
        {

            OnAwake();
            State = UIState.Created;
        }

        internal void Close()
        {
            OnClose();
            _iView.SetActive(false);
            State = UIState.Closed;
        }

        internal void Recycle()
        {
            OnRecycle();
            State = UIState.Recycle;
        }

        internal void Destroy()
        {
            OnAwake();
            OnDestroy();
            State = UIState.Destroyed;
        }

        protected virtual void BeforeCreate()
        {
        }

        protected virtual void OnAwake()
        {
        }

        protected virtual void OnOpen()
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

        protected virtual void LoadUIView(string uiName, Action<IUIView> callback)
        {
        }

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