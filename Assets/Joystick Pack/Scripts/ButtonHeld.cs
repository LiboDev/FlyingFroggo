using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool held;
    public bool pressed;
    public bool released;

    public void OnPointerDown(PointerEventData eventData)
    {
        held = true;
        pressed = true;
        StartCoroutine(ResetDown());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        held = false;
        released = true;
        StartCoroutine(ResetUp());
    }

    private IEnumerator ResetDown()
    {
        yield return 0;
        pressed = false;
    }

    private IEnumerator ResetUp()
    {
        yield return 0;
        released = false;
    }
}
