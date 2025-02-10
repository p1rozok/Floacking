using UnityEngine;

public class returnPosition : MonoBehaviour
{
    private Transform _baitTransform;

    public void SetBaitTransform(Transform baitTransform)
    {
        _baitTransform = baitTransform;
    }

    public Transform ReturnTransformPositionBait()
    {
        return _baitTransform;
    }
}
