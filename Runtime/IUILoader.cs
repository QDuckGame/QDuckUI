using System;
namespace QDuck.UI
{
    public interface IUILoader
    {
        void Get(string uiName, System.Action<IUIBehavior> callback);
    }
}