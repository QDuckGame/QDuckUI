using Game.Core;

namespace Game.Panel
{
    public partial class EnterGamePage:UIPage
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            EnterBtn.onClick.AddListener(OnClickEnterBtn);
        }

        private void OnClickEnterBtn()
        {
            UIMgr.Open<MainPage>();
        }
    }
}