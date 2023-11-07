using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[AddComponentMenu("UI/Extended Button")]
public class ExtendedButton : Button, IPointerUpHandler
{
    [System.Serializable] new public class ButtonClickedEvent : UnityEvent {}

    public ButtonClickedEvent onPointerUp;

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        if (interactable && eventData.button == PointerEventData.InputButton.Left)
        {
            onPointerUp.Invoke();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        if (onPointerUp == null)
            onPointerUp = new ButtonClickedEvent();
    }
}
