
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class PresenterBait : MonoBehaviour
{
    [SerializeField]  private SpriteRenderer _image;
    public void PresenterImage(Sprite image)
    {
        _image.sprite= image;
    }

}

