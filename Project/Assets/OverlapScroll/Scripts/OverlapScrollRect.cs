using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// スクロール領域が重なるときScrollRect制御
/// </summary>
public class OverlapScrollRect : ScrollRect
{
    private enum ScrollType
    {
        NotDrag,   // ドラッグされていない
        Self,      // 自身
        Back       // 裏
    }

    private ScrollRect backScrollRect_ = null;
    private ScrollType scrollType_ = ScrollType.NotDrag;

    protected override void OnDisable()
    {
        base.OnDisable();

        // 裏のScrollRectを強制的にドラッグ解除する
        if (scrollType_ == ScrollType.Back)
        {
            PointerEventData eventData = new PointerEventData(null);
            eventData.button = PointerEventData.InputButton.Left;
            backScrollRect_.OnEndDrag(eventData);
            scrollType_ = ScrollType.NotDrag;
        }
    }

    /// <summary>
    /// 裏にあるScrollRectの設定
    /// </summary>
    public void SetBackScrollRect(ScrollRect scrollRect)
    {
        backScrollRect_ = scrollRect;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        // スクロール対象確認
        ScrollType prevScrollType = scrollType_;
        if (backScrollRect_ == null) { scrollType_ = ScrollType.Self; }
        if (scrollType_ == ScrollType.NotDrag)
        {
            if (vertical == horizontal)
            {
                scrollType_ = ScrollType.Self;
            }
            else if (vertical)
            {
                if (Mathf.Abs(eventData.delta.x) < Mathf.Abs(eventData.delta.y)) { scrollType_ = ScrollType.Self; }
                else { scrollType_ = ScrollType.Back; }
            }
            else if (horizontal)
            {
                if (Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y)) { scrollType_ = ScrollType.Self; }
                else { scrollType_ = ScrollType.Back; }
            }
        }

        // 裏をスクロールする場合にScrollRectの値をコピー
        if (prevScrollType == ScrollType.NotDrag && scrollType_ == ScrollType.Back)
        {
            // 裏のScrollRectを強制的にドラッグ開始する
            backScrollRect_.OnBeginDrag(eventData);
        }
        else
        {
            if (scrollType_ == ScrollType.Back) { backScrollRect_.OnDrag(eventData); }
            else { base.OnDrag(eventData); }
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        // 裏のScrollRectを強制的にドラッグ解除する
        if (scrollType_ == ScrollType.Back) { backScrollRect_.OnEndDrag(eventData); }

        scrollType_ = ScrollType.NotDrag;
    }
}
