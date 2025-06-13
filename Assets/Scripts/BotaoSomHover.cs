using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class BotaoSomHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public GameObject sliderGO;
    private Coroutine hideCoroutine;

    public void OnPointerEnter(PointerEventData eventData)
    {
        MostrarSlider();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        MostrarSlider();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hideCoroutine != null) StopCoroutine(hideCoroutine);
        hideCoroutine = StartCoroutine(EsconderComDelay());
    }

    private void MostrarSlider()
    {
        if (hideCoroutine != null) StopCoroutine(hideCoroutine);
        sliderGO.SetActive(true);
    }

    private IEnumerator EsconderComDelay()
    {
        yield return new WaitForSeconds(3f);
        sliderGO.SetActive(false);
    }
}
