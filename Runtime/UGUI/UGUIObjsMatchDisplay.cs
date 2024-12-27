using System;
using UnityEngine;

namespace QDuck.UI
{
    
    public class UGUIObjMatch:MonoBehaviour
    {
        [Serializable]
        public class MatchItem
        {
            public GameObject[] Prefab;
            public string Tag;
            
            public void SetActive(bool active)
            {
                if(Prefab == null) return;
                foreach (var go in Prefab)
                {
                    if(go.activeSelf != active)
                        go.SetActive(active);
                }
            }
        }
        
        [SerializeField]
        private MatchItem[] _matchItems;

        public void Display(string tag)
        {
            foreach (var item in _matchItems)
            {
                item.SetActive(item.Tag == tag);
            }
        }
        
        public void Display(int index)
        {
            for (int i = 0; i < _matchItems.Length; i++)
            {
                _matchItems[i].SetActive(i == index);
            }
        }
    }
}