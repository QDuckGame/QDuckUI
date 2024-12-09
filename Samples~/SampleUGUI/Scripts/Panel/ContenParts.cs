using Game.Core;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Panel
{
    public partial class ContenParts:UIWidget
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            TestBtn.onClick.AddListener(OnTestClick);
        }

        private void OnTestClick()
        {
            Content.text = $"number.{Random.Range(0, 100)}";
        }
    }
}