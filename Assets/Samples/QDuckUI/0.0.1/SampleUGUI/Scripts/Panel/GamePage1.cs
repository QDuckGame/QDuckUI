using Game.Core;
using Game.PanelGen;

namespace Game.Panel
{
    public class GamePage1:UIPage<GamePage1View>
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            View.CloseBtnBtn.onClick.AddListener(OnClickCloseBtnBtn);
            View.OpenWin2Btn.onClick.AddListener(OnClickOpenWin2Btn);
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