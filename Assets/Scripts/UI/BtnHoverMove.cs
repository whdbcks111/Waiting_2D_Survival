using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnHoverMove : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool isHover = false;
    private RectTransform rectTransform;

    void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        var pos = rectTransform.anchoredPosition;
        if(isHover) {
            
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
         isHover = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
         isHover = false;
    }
}
