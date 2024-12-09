using Game.Core;
using Game.Panel;
using UnityEngine;

public class QDuckUIEntry : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIMgr.Init();
        UIMgr.Open<EnterGamePage>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
