using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// UI事件监听帮助类2020.11.01
/// </summary>
public delegate void PointerHandler(PointerEventData data);
public class UIEventListener : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public event PointerHandler OnClick;
    public event PointerHandler PointerDown;
    public event PointerHandler PointerEnter;
    public event PointerHandler PointerExit;
    public event PointerHandler PointerUp;
    public event PointerHandler BeginDrag;
    public event PointerHandler Drag;
    public event PointerHandler EndDrag;
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (BeginDrag == null) return;
        BeginDrag(eventData);
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (Drag == null) return;
        Drag(eventData);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (EndDrag == null) return;
        EndDrag(eventData);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClick == null) return;
        OnClick(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (PointerDown == null) return;
        PointerDown(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (PointerEnter == null) return;
        PointerEnter(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (PointerExit == null) return;
        PointerExit(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (PointerUp == null) return;
        PointerUp(eventData);
    }
}

