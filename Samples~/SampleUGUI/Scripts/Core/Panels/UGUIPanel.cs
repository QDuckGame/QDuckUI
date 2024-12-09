using System;
using UnityEngine;
using UnityEngine.Playables;

namespace QDuck.UI
{
    public class UGUIPanel:UIPanel
    {
        private Animator _rootAnimator;
        
        private PlayableDirector _openPlayable;
        private PlayableDirector _closePlayable;

        private UGUIBehavior _uguiBehavior;

        protected UGUIBehavior UGUIBehavior
        {
            get
            {
                if (_uguiBehavior == null)
                {
                    _uguiBehavior = _uiBehaviour as UGUIBehavior;
                }
                return _uguiBehavior;
            }
        }

        protected GameObject Go => UGUIBehavior.gameObject;
        
        protected override void OnPlayOpenTween(Action onFinish = null)
        {
            StopPlayables();
            if(_openPlayable==null)
                _openPlayable = Go.transform.Find("OpenTimeline")?.GetComponent<PlayableDirector>();
            if(_openPlayable == null)
            {
                onFinish?.Invoke();
                return;
            }
            Action<PlayableDirector> onStopped = null;
            onStopped = director =>
            {
                onFinish?.Invoke();
                _openPlayable.stopped -= onStopped;
            };
            _openPlayable.stopped += onStopped;
            _openPlayable.Play();

        }

        protected override void OnPlayCloseTween(Action onFinish = null)
        {
            StopPlayables();
            if(_closePlayable == null)
                _closePlayable = Go.transform.Find("CloseTimeline")?.GetComponent<PlayableDirector>();
            if(_closePlayable == null)
            {
                onFinish?.Invoke();
                return;
            }
            Action<PlayableDirector> onStopped = null;
            onStopped = director =>
            {
                onFinish?.Invoke();
                _closePlayable.stopped -= onStopped;
            };
            _closePlayable.stopped += onStopped;
            _closePlayable.Play();

        }
        
        private void StopPlayables()
        {
            _openPlayable?.Stop();
            _closePlayable?.Stop();
        }
        
        private GameObject blocker;

        protected override void SetFullScreenBlock(bool isBlock)
        {

            if (!isBlock)
            {
                if(blocker!=null)
                    blocker.SetActive(false);
                return;
            }
            if (blocker == null)
            {
                // 创建一个新的 GameObject
                blocker = new GameObject("FullScreenBlocker");
                blocker.transform.SetParent(Go.transform, false);
                blocker.transform.SetAsFirstSibling();

                // 设置 RectTransform 以覆盖整个屏幕
                RectTransform rectTransform = blocker.AddComponent<RectTransform>();
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.sizeDelta = Vector2.zero;

                // 添加 CanvasGroup 组件并启用阻挡射线
                blocker.AddComponent<CanvasRenderer>();
                blocker.AddComponent<UGUIEmptyRaycast>();
            }
            if(blocker.activeSelf!=isBlock) blocker.SetActive(isBlock);

        }
    }
}