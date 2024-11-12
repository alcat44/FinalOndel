using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateObjective : MonoBehaviour
{
    public TextMeshProUGUI objectiveText;
    public GameObject pickupText;
    public GameObject bahan;
    public GameObject icon;
    public GameObject music;
    public AudioSource audioSource; // Referensi untuk AudioSource
    public AudioClip updateSound;   // Referensi untuk AudioClip
    public bool interactable;

    public void UpdateObjectiveText(string newObjective, bool playSound)
    {
        objectiveText.text = newObjective;
        if (playSound && audioSource != null && updateSound != null) // Cek apakah AudioSource dan AudioClip sudah diatur
        {
            audioSource.PlayOneShot(updateSound); // Mainkan efek suara
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("8Icon"))
        {
            Destroy(icon);
            UpdateObjectiveText("⊙ Don't get caught by the Ondel-ondel\n⊙ Mission 1: Match the traditional clothes in the traditional clothes gallery with the information on the front", true);
        }
        else if (other.CompareTag("Bahan"))
        {
            Debug.Log("Bahan detected and will be destroyed."); // Add this line to check if the code runs
            Destroy(bahan);
            UpdateObjectiveText("⊙ Mission 2: Find the lost ingredients of the kerak telor on the 2nd floor", true);
        }
        else if (other.CompareTag("Music"))
        {
            Destroy(music);
            UpdateObjectiveText("⊙ Mission 3: Find the right melody", true);
        }
    }

    public GameObject inspectCanvas;
    private bool isActive;

    private void Start()
    {
        interactable = false;
        isActive = false;
        if (inspectCanvas != null)
        {
            inspectCanvas.SetActive(false);
        }
        if (pickupText != null)
        {
            pickupText.SetActive(false);
        }
    }

    private void Update()
    {
        if (interactable && Input.GetKeyDown(KeyCode.E))
        {
            ToggleInspectCanvas();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Inspect"))
        {
            interactable = true;
            pickupText.SetActive(true);  // Teks muncul saat collider menyentuh objek
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Inspect"))
        {
            pickupText.SetActive(false);
            interactable = false;
            if (inspectCanvas != null && inspectCanvas.activeSelf)
            {
                inspectCanvas.SetActive(false);
                isActive = false;
            }
        }
    }

    private void ToggleInspectCanvas()
    {
        isActive = !isActive;
        if (inspectCanvas != null)
        {
            inspectCanvas.SetActive(isActive);
        }
        if (pickupText != null)
        {
            pickupText.SetActive(!isActive);  // Teks hilang setelah inspeksi dilakukan
        }
        if (bahan != null && isActive)
        {
            bahan.SetActive(true);
        }
    }
}
