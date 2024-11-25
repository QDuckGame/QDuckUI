using UnityEngine;
using Object = System.Object;
namespace QDuck.UI
{
    public class UGUILoader:IUILoader
    {
        public void Get<T>(string uiName, System.Action<IUIView> callback)
            where T:IUIView, new()
        {
            LoadRes(uiName, (obj) =>
            {
                GameObject go = obj as GameObject;
                if (go == null)
                {
                    callback?.Invoke(null);
                    return;
                }
                T view = new T();
                view.Bind(go);
                callback?.Invoke(view);
            });
        }
    
        public virtual void LoadRes(string uiName, System.Action<Object> callback)
        {
            GameObject go = UnityEngine.Resources.Load<GameObject>(uiName);
            callback?.Invoke(go);
        }
    }
}