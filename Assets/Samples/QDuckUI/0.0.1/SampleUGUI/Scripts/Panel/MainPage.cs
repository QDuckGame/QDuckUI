using Game.Core;
using Game.PanelGen;

namespace Game.Panel
{
    public class MainPage:UIPage<MainPageView>
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            View.OpenWin1Btn.onClick.AddListener(OnClickOpenWin1Btn);
            View.OpenGamePage1Btn.onClick.AddListener(OnClickOpenGamePage1Btn);
        }
        
        private void OnClickOpenWin1Btn()
        {
           UIMgr.Open<Windows1>();
        }

        private void OnClickOpenGamePage1Btn()
        {
            UIMgr.Open<GamePage1>();
        }


        
    }
}