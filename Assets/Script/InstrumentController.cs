using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentController : MonoBehaviour
{
    public GameObject inttext; // Teks instruksi interaksi
    public GameObject successText; // Teks yang muncul jika misi berhasil
    public GameObject failText; // Teks yang muncul jika misi gagal
    public GameObject rewardObject; // Objek yang muncul sebagai reward
    public AudioSource rewardSound; // Suara yang dimainkan sebagai reward
    public Animator rewardAnimator; // Animator untuk animasi reward
    public bool interactable; // Menandakan apakah objek dapat diinteraksi
    public AudioSource[] instrumentSounds; // Array untuk menyimpan AudioSource untuk setiap alat musik
    public Timelinetiga jumpscare; // Skrip Timelinetiga yang berhubungan dengan jumpscare
    public GameObject light; // Lampu yang akan dihidupkan kembali
    public Renderer lightBulb;
    public Material onlight;
    public GameObject cutscene3trigger;
    public GameObject ondel1;

    private int currentInstrumentIndex = -1; // Indeks alat musik saat ini yang berada dalam jangkauan
    private List<int> playedInstruments = new List<int>(); // Daftar untuk menyimpan urutan instrumen yang dimainkan
    private bool isPlayingInstrument = false; // Menandakan apakah ada instrumen yang sedang dimainkan

    void Start()
    {
        successText.SetActive(false); // Pastikan teks berhasil tidak muncul di awal
        failText.SetActive(false); // Pastikan teks salah tidak muncul di awal
        rewardObject.SetActive(false); // Pastikan objek reward tidak muncul di awal
        TurnOnLight(); // Pastikan lampu menyala di awal
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            inttext.SetActive(true);
            interactable = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            inttext.SetActive(false);
            interactable = false;
            currentInstrumentIndex = -1; // Reset indeks alat musik saat keluar dari jangkauan
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            // Deteksi alat musik saat ini berdasarkan jarak terdekat
            inttext.SetActive(true);
            interactable = true;
            float minDistance = float.MaxValue;
            for (int i = 0; i < instrumentSounds.Length; i++)
            {
                float distance = Vector3.Distance(other.transform.position, instrumentSounds[i].transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    currentInstrumentIndex = i;
                }
            }
        }
    }

    void Update()
    {
        if (interactable && Input.GetKeyDown(KeyCode.E))
        {
            if (currentInstrumentIndex != -1 && !isPlayingInstrument)
            {
                PlayInstrument(currentInstrumentIndex);
            }
        }
    }

    void PlayInstrument(int index)
    {
        if (index >= 0 && index < instrumentSounds.Length)
        {
            if (instrumentSounds[index] != null && !instrumentSounds[index].isPlaying)
            {
                instrumentSounds[index].Play();
                playedInstruments.Add(index);
                isPlayingInstrument = true;
                StartCoroutine(WaitForInstrumentToFinish(instrumentSounds[index]));
                CheckSequence();
            }
        }
    }

    IEnumerator WaitForInstrumentToFinish(AudioSource audioSource)
    {
        yield return new WaitWhile(() => audioSource.isPlaying);
        isPlayingInstrument = false;
    }

    void CheckSequence()
    {
        // Urutan instrumen yang diinginkan
        int[] correctSequence = { 0, 1, 2, 3 };

        if (playedInstruments.Count == correctSequence.Length)
        {
            for (int i = 0; i < correctSequence.Length; i++)
            {
                if (playedInstruments[i] != correctSequence[i])
                {
                    // Jika urutan salah, tampilkan teks salah, reset daftar, dan keluar dari fungsi
                    failText.SetActive(true);
                    Invoke("ResetSequence", 2.0f); // Reset setelah 2 detik
                    return;
                }
            }
            // Jika urutan benar, tampilkan teks berhasil dan reward
            successText.SetActive(true);
            Invoke("HideSuccessText", 2.0f); // Sembunyikan teks berhasil setelah 2 detik
            Invoke("ShowReward", 5.0f); // Tampilkan reward setelah 5 detik
        }
        else
        {
            // Periksa apakah ada kesalahan di tengah urutan
            for (int i = 0; i < playedInstruments.Count; i++)
            {
                if (playedInstruments[i] != correctSequence[i])
                {
                    // Jika urutan salah, tampilkan teks salah, reset daftar, dan keluar dari fungsi
                    failText.SetActive(true);
                    Invoke("ResetSequence", 2.0f); // Reset setelah 2 detik
                    return;
                }
            }
        }
    }

    void ResetSequence()
    {
        playedInstruments.Clear(); // Reset urutan instrumen yang dimainkan
        failText.SetActive(false); // Sembunyikan teks salah
    }

    void HideSuccessText()
    {
        successText.SetActive(false); // Sembunyikan teks berhasil
    }

    void ShowReward()
    {
        rewardObject.SetActive(true); // Tampilkan objek reward
        if (rewardAnimator != null)
        {
            rewardAnimator.SetTrigger("Appear"); // Trigger animasi muncul
        }
        if (rewardSound != null)
        {
            rewardSound.Play(); // Mainkan suara reward
        }
        Invoke("TurnOnLightAfterReward", 2.0f); // Nyalakan lampu setelah 2 detik animasi reward
    }

    void TurnOnLightAfterReward()
    {
        TurnOnLight(); // Nyalakan lampu setelah 2 detik
        RenderSettings.fog = true;
        cutscene3trigger.SetActive(true);
        ondel1.SetActive(false);
    }

    void TurnOnLight()
    {
        if (light != null)
        {
            light.SetActive(true);
            if (lightBulb != null && onlight != null)
            {
                lightBulb.material = onlight;
            }
        }
    }
}
