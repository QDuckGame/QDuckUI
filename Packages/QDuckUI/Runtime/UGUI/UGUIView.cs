using UnityEngine;
namespace QDuck.UI
{
    public class UGUIView:IUIView
    {
        public GameObject GameObject { get; private set; }
        protected UGUIBinder _binder;
    
        public void Bind(object go)
        {
            GameObject = go as GameObject;
            _binder = GameObject.GetComponent<UGUIBinder>();
            if (_binder == null)
            {
                Debug.LogError("UGUIView.Bind() failed, UGUIBinder not found.");
            }
            OnBind();
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