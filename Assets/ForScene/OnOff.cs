using UnityEngine;
public class ToggleGameObject : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    private bool isPaused = false;

    public void Toggle()
    {

        bool isActive = !targetObject.activeSelf;
        targetObject.SetActive(isActive);
        
        if (isActive)
        {
            PauseTime();
        }
        else
        {
            ResumeTime();
        }
      
    }
    
    private void PauseTime()
    {
        Time.timeScale = 0.00001f;
        isPaused = true;
    }

    private void ResumeTime()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }

    private void Awake()
    {
        Time.timeScale = 1f;
    }
    
    
}
