using Game.Core;
using Game.Panel;

namespace Game.Panel
{
    public partial class Windows1:UIWindow
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            OpenWin2Btn.onClick.AddListener(OnClickOpenWin2Btn);
            CloseBtn.onClick.AddListener(OnClickCloseBtn);
        }

        private void OnClickOpenWin2Btn()
        {
            UIMgr.Open<Windows2>();
        }

        private void OnClickCloseBtn()
        {
            UIMgr.Close(this);
        }
    }
}