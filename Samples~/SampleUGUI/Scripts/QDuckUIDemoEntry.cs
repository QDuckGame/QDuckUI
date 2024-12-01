using System;
using Game.Core;
using Game.Panel;
using UnityEngine;

public class QDuckUIDemoEntry : MonoBehaviour
{
    private void Awake()
    {
        UIMgr.Init();
        UIMgr.Open<EnterGamePage>();
    }
}
