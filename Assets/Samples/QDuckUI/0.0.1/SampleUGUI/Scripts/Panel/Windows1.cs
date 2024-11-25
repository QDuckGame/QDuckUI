using Game.Core;
using Game.PanelGen;

namespace Game.Panel
{
    public class Windows1:UIWindow<Windows1View>
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            View.OpenWin2Btn.onClick.AddListener(OnClickOpenWin2Btn);
            View.CloseBtn.onClick.AddListener(OnClickCloseBtn);
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