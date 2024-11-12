using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorAfterR : MonoBehaviour
{
    public GameObject intText;       // Text yang muncul saat pemain mendekat
    public GameObject lockedText;    // Text yang muncul saat pintu terkunci
    public bool isLocked = true;     // Menentukan apakah pintu terkunci
    public AudioSource audioSource;
    public AudioClip lockedSound;    // Suara saat pintu terkunci

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            intText.SetActive(true);

            // Jika tombol E ditekan dan pintu terkunci, tampilkan pesan terkunci
            if (Input.GetKeyDown(KeyCode.E) && isLocked)
            {
                lockedText.SetActive(true);
                audioSource.PlayOneShot(lockedSound);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            intText.SetActive(false);
            lockedText.SetActive(false);
        }
    }
}
