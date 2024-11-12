using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class dalammusik : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;
    public GameObject light; // Referensi ke GameObject lampu
    public GameObject textObject; // Referensi ke GameObject teks
    private Collider triggerCollider;

    void Start()
    {
        // Menyimpan referensi ke collider
        triggerCollider = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(StopAudioAfterDelay(2f));
        }
    }

    IEnumerator StopAudioAfterDelay(float delay)
    {
        // Hentikan audio setelah delay
        yield return new WaitForSeconds(delay);

        if (audioSource != null)
        {
            audioSource.Stop();
        }

        // Matikan lampu
        if (light != null)
        {
            light.SetActive(false);
        }

        // Tampilkan teks
        if (textObject != null)
        {
            textObject.SetActive(true);
        }

        // Tunggu selama 5 detik sebelum menghilangkan teks
        yield return new WaitForSeconds(5f);

        // Matikan teks
        if (textObject != null)
        {
            textObject.SetActive(false);
        }

        // Menghapus trigger collider setelah pemain mengenainya
        if (triggerCollider != null)
        {
            triggerCollider.enabled = false;
        }
    }
}
