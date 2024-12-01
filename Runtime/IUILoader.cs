using System;
namespace QDuck.UI
{
    public interface IUILoader
    {
        void Get<T>(string uiName, System.Action<IUIView> callback) where T : IUIView, new();
    }
}