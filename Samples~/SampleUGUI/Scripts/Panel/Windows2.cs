using Game.Core;
using Game.Panel;

namespace Game.Panel
{
    public partial class Windows2:UIWindow
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            CloseBtn.onClick.AddListener(OnClickCloseBtn);
        }

        private void OnClickCloseBtn()
        {
            UIMgr.Close(this);
        }
    }
}