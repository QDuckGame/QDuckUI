using System;
namespace Duck.UI
{
    public interface IUIView
    {
        void Bind(Object gameObject);
        void SetActive(bool active);
        bool IsActive();
        void Destroy();
    }
}