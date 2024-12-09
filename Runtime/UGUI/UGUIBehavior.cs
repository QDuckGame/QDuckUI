using System.Collections.Generic;
using UnityEngine;
namespace QDuck.UI
{
    public class UGUIBehavior:MonoBehaviour,IUIBehavior
    {
        public Component[] Components ;
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public bool IsActive()
        {
            return gameObject.activeSelf;
        }

        public void Destroy()
        {
            Object.Destroy(gameObject);
        }

        public IUIBehavior Clone()
        {
            GameObject go = GameObject.Instantiate(gameObject);
            return go.GetComponent<UGUIBehavior>();
        }

        public object GetBindComponent(int index)
        {
            return Components[index];
        }
    }
}