using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ChangerButtonImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    [SerializeField]
    private Sprite noActiveSprite;

    [SerializeField]
    private Sprite activeSprite;

    private Image buttonImage;

    public void Awake()
    {
        buttonImage = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.sprite = activeSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.sprite = noActiveSprite;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonImage.sprite = noActiveSprite;
    }
}
