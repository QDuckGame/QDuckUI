using Game.Core;
using Game.PanelGen;

namespace Game.Panel
{
    public class MainPage:UIPage<MainPageView>
    {
        private ContenParts _contenParts;
        protected override void OnAwake()
        {
            base.OnAwake();
            _contenParts = BindParts<ContenParts>(View.ContenParts);
            View.OpenWin1Btn.onClick.AddListener(OnClickOpenWin1Btn);
            View.OpenGamePage1Btn.onClick.AddListener(OnClickOpenGamePage1Btn);
            View.ContenParts.Content.text = "Hello World";
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