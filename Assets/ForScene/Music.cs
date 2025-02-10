using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource; 
    private static MusicManager instance; 
    private bool isMuted = false; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public void ToggleMusic()
    {
        if (musicSource != null)
        {
            isMuted = !isMuted;
            musicSource.mute = isMuted;
            Debug.Log(isMuted ? "Музыка выключена" : "Музыка включена");
        }
        else
        {
            Debug.LogError("AudioSource для музыки не назначен!");
        }
    }

    public void MuteMusic()
    {
        if (musicSource != null && !isMuted)
        {
            isMuted = true;
            musicSource.mute = true;
            Debug.Log("Музыка выключена");
        }
    }

    public void UnmuteMusic()
    {
        if (musicSource != null && isMuted)
        {
            isMuted = false;
            musicSource.mute = false;
            Debug.Log("Музыка включена");
        }
    }
}