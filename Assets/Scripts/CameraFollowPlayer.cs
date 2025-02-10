using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
   [SerializeField] private Transform target;
   [SerializeField] private Vector3 offset;
   [SerializeField] [Range(0f, 1f)] private float followSpeed;



   private void Update()
   {
      var lerpPos = Vector2.Lerp(target.position + offset, transform.position, followSpeed);
      transform.position = new Vector3(lerpPos.x, lerpPos.y, transform.position.z);
   }
}
