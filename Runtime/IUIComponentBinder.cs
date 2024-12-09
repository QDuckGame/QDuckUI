using System;

namespace QDuck.UI
{
    public interface IUIComponentBinder
    {
       void BindComponents( IUIBehavior uiBehavior,UIPanel panel);
    }
}