using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    private bool isMuted = false;

    private void Start()
    {
        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0 : volumeSlider.value; 
    }

    public void SetVolume(float value)
    {
        if (!isMuted)
        {
            AudioListener.volume = value;
        }
    }
}