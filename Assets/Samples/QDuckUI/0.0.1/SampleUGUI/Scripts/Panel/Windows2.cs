using Game.Core;
using Game.PanelGen;

namespace Game.Panel
{
    public class Windows2:UIWindow<Windows2View>
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            View.CloseBtn.onClick.AddListener(OnClickCloseBtn);
        }

        private void OnClickCloseBtn()
        {
            UIMgr.Close(this);
        }
    }
}