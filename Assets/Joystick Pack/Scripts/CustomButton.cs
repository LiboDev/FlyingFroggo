using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ButtonHeld : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool held;
    public bool pressed;
    public bool released;

    public void OnPointerDown(PointerEventData eventData)
    {
        held = true;
        pressed = true;
        StartCoroutine(Reset());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        held = false;
        released = true;
        StartCoroutine(Reset());
    }

    private IEnumerator Reset()
    {
        yield return 0;
        released = false;
        pressed = false;
    }
}
