using UnityEngine;
namespace QDuck.UI
{
    public class UGUIView:IUIView
    {
        public GameObject GameObject { get; private set; }
        protected UGUIBinder _binder;
        
    
        public void Bind(IUIBinder go)
        {
            _binder = go as UGUIBinder;
            this.GameObject = _binder.gameObject;
            if (_binder == null)
            {
                Debug.LogError("UGUIView.Bind() failed, UGUIBinder not found.");
            }
            OnBind();
        }

        public IUIBinder GetBinder()
        {
            return _binder;
        }

        public IUIView Clone()
        {
            GameObject go = GameObject.Instantiate(this.GameObject);
            UGUIView view = new UGUIView();
            view.Bind(go.GetComponent<UGUIBinder>());
            return view;
        }

        protected virtual void OnBind() { }
    
        public void SetActive(bool active)
        {
            this.GameObject.SetActive(active);
        }

        public bool IsActive()
        {
            return this.GameObject.activeSelf;
        }

        public void Destroy()
        {
            GameObject.Destroy(this.GameObject);
        }
        
        
    }
}