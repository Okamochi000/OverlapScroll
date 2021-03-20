using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// �X�N���[���̈悪�d�Ȃ�Ƃ�ScrollRect����
/// </summary>
public class OverlapScrollRect : ScrollRect
{
    private enum ScrollType
    {
        NotDrag,   // �h���b�O����Ă��Ȃ�
        Self,      // ���g
        Back       // ��
    }

    private ScrollRect backScrollRect_ = null;
    private ScrollType scrollType_ = ScrollType.NotDrag;

    protected override void OnDisable()
    {
        base.OnDisable();

        // ����ScrollRect�������I�Ƀh���b�O��������
        if (scrollType_ == ScrollType.Back)
        {
            PointerEventData eventData = new PointerEventData(null);
            eventData.button = PointerEventData.InputButton.Left;
            backScrollRect_.OnEndDrag(eventData);
            scrollType_ = ScrollType.NotDrag;
        }
    }

    /// <summary>
    /// ���ɂ���ScrollRect�̐ݒ�
    /// </summary>
    public void SetBackScrollRect(ScrollRect scrollRect)
    {
        backScrollRect_ = scrollRect;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        // �X�N���[���Ώۊm�F
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

        // �����X�N���[������ꍇ��ScrollRect�̒l���R�s�[
        if (prevScrollType == ScrollType.NotDrag && scrollType_ == ScrollType.Back)
        {
            // ����ScrollRect�������I�Ƀh���b�O�J�n����
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

        // ����ScrollRect�������I�Ƀh���b�O��������
        if (scrollType_ == ScrollType.Back) { backScrollRect_.OnEndDrag(eventData); }

        scrollType_ = ScrollType.NotDrag;
    }
}
