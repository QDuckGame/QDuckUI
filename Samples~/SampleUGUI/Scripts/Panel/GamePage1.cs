using Game.Core;
using Game.Panel;

namespace Game.Panel
{
    public partial class GamePage1:UIPage
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            CloseBtnBtn.onClick.AddListener(OnClickCloseBtnBtn);
            OpenWin2Btn.onClick.AddListener(OnClickOpenWin2Btn);
        }
        
        private void OnClickCloseBtnBtn()
        {
            UIMgr.Close(this);
        }
        
        private void OnClickOpenWin2Btn()
        {
            UIMgr.Open<Windows2>();
        }
    }
}