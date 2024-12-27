using System;
namespace QDuck.UI
{
    public interface IUILoader
    {
        void Get(UIContext context, string uiName, System.Action<IUIBehavior> callback);
    }
}