using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class DetailsSnippet : MonoBehaviour
{
    [Header("Detail UI Elements (Inherited)")]
    [SerializeField] protected Image thumbnailImage;
    [SerializeField] protected TextMeshProUGUI nameText;

    public virtual void SetThumbnail(Sprite thumbnailSprite)
    {
        if (thumbnailSprite != null)
        {
            thumbnailImage.sprite = thumbnailSprite;
        }
    }

    public virtual void SetName(string name)
    {
        if (name != null)
            nameText.text = name;
    }

    public virtual void InitializeDetails(string name, Sprite thumbnailSprite)
    {
        SetName(name);
        SetThumbnail(thumbnailSprite);
    }
}
