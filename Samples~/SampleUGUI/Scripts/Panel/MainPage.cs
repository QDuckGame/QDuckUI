using Game.Core;
using Game.Panel;

namespace Game.Panel
{
    public partial class MainPage:UIPage
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            OpenWin1Btn.onClick.AddListener(OnClickOpenWin1Btn);
            OpenGamePage1Btn.onClick.AddListener(OnClickOpenGamePage1Btn);
            ContenParts.Content.text = "Hello World";
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