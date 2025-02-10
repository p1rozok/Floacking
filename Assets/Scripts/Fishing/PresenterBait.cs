using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]


public class PresenterBait : MonoBehaviour

{
    
    public Bait baitData;

    [SerializeField] private SpriteRenderer _image;

    public void PresenterImage(Sprite image)
    {
        _image.sprite = image;
    }
    private void OnEnable()
    {
        Bait bait = Inventory.Instance.CurrentBait();
        PresenterImage(bait.Sprite);
    }
}