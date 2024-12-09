using System;
namespace QDuck.UI
{
    public interface IUIBehavior
    {
        void SetActive(bool active);
        bool IsActive();
        void Destroy();
        IUIBehavior Clone();
        Object GetBindComponent(int index);
    }
}