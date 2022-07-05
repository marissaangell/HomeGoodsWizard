using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FurnitureItemListEntry : DetailsSnippet
{
    [Header("FurnitureItemListEntry Elements")]
    [SerializeField] private Button buttonComponent;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite defaultBackgroundSprite;
    [SerializeField] private Sprite pressedBackgroundSprite;

    public Button GetButtonComponent()
    {
        return buttonComponent;
    }

    public void SetPressedState(bool pressed)
    {
        backgroundImage.sprite = pressed ? pressedBackgroundSprite : defaultBackgroundSprite;
        backgroundImage.color = pressed ? Color.gray : Color.white;
    }

}
