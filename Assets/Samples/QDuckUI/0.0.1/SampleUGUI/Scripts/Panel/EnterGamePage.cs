using Game.Core;
using Game.PanelGen;

namespace Game.Panel
{
    public class EnterGamePage:UIPage<EnterGamePageView>
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            View.EnterBtn.onClick.AddListener(OnClickEnterBtn);
        }

        private void OnClickEnterBtn()
        {
            UIMgr.Open<MainPage>();
        }
    }
}