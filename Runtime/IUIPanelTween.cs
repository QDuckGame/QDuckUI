using System;
namespace QDuck.UI
{
    public interface IUIPanelTween
    {
        void Play(Action callback);
    }
}