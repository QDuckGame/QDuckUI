using UnityEngine;
using Object = System.Object;
namespace QDuck.UI
{
    public class UGUILoader:IUILoader
    {
        public void Get(string uiName, System.Action<IUIBehavior> callback)
        {
            LoadRes(uiName, (obj) =>
            {
                GameObject go = obj as GameObject;
                if (go == null)
                {
                    callback?.Invoke(null);
                    return;
                }
                callback?.Invoke(go.GetComponent<UGUIBehavior>());
            });
        }
    
        public virtual void LoadRes(string uiName, System.Action<Object> callback)
        {
            GameObject go = UnityEngine.Resources.Load<GameObject>(uiName);
            callback?.Invoke(go);
        }
    }
}