using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// スクロール初期設定
/// </summary>
public class ScrollSetting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ScrollRect scrollRect = this.GetComponent<ScrollRect>();
        OverlapScrollRect[] overlapScrollRects = this.GetComponentsInChildren<OverlapScrollRect>();
        foreach (OverlapScrollRect overlapScrollRect in overlapScrollRects) { overlapScrollRect.SetBackScrollRect(scrollRect); }
    }
}
