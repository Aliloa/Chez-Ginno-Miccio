using UnityEngine;

public class RadioScript : MonoBehaviour
{
    [SerializeField] private AudioSource radioAudioSource;
    private bool isPlaying = true;

    public void ToggleRadio()
    {
        isPlaying = !isPlaying;

        if (isPlaying)
            radioAudioSource.Play();
        else
            radioAudioSource.Pause();
    }
}