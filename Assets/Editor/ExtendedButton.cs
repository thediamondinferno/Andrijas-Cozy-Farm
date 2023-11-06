using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[AddComponentMenu("UI/Extended Button")] // This makes the script appear in the "Add Component" menu in Unity under "UI" category.
public class ExtendedButton : Button, IPointerUpHandler
{
    [System.Serializable] new public class ButtonClickedEvent : UnityEvent {}

    // Event to be shown in the Inspector
    public ButtonClickedEvent onPointerUp;

    // Overriding the OnPointerUp function from IPointerUpHandler
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData); // Call the base class method

        if (interactable && eventData.button == PointerEventData.InputButton.Left)
        {
            onPointerUp.Invoke(); // Invoke the custom event
        }
    }

    protected override void Awake()
    {
        base.Awake();
        // Ensure there is an event holder
        if (onPointerUp == null)
            onPointerUp = new ButtonClickedEvent();
    }
}
