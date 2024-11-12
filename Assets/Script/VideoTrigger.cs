using UnityEngine;
using UnityEngine.Video;

public class VideoTrigger : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Assign this in the Inspector
    public AudioSource audioSource; // Assign this in the Inspector

    private Collider triggerCollider; // Reference to the trigger collider

    private void Start()
    {
        // Ensure the audio source is stopped at the start
        audioSource.Stop();

        // Get the collider component attached to this game object
        triggerCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            // Check if the video is currently playing or paused
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Pause(); // Pause video if playing
                audioSource.Pause(); // Pause audio if playing
            }
            else
            {
                videoPlayer.Play(); // Play video if paused
                audioSource.Play(); // Play audio if stopped or paused
            }

            // Disable the collider to prevent re-triggering
            triggerCollider.enabled = false;
        }
    }
}
