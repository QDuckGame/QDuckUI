using System;
namespace QDuck.UI
{
    public interface IUIView
    {
        void SetActive(bool active);
        bool IsActive();
        void Destroy();
        IUIView Clone();
        void Bind(IUIBinder gameObject);
        IUIBinder GetBinder();
    }
}