using Game.Core;
using Game.PanelGen;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Panel
{
    public class ContenParts:UIPage<ContenPartsView>
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            View.TestBtn.onClick.AddListener(OnTestClick);
        }

        private void OnTestClick()
        {
            View.Content.text = $"number.{Random.Range(0, 100)}";
        }
    }
}