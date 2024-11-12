using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class Grabber : MonoBehaviour
{
    [Header("Text")]
    private GameObject selectedObject;
    public GameObject pickupText, dropText;
    public GameObject messageText;

    [Header("Game Object")]
    public GameObject baju1;
    public GameObject baju2;
    public GameObject baju3;
    public GameObject Lantai21;
    public GameObject Lantai22;
    public GameObject Lantai23;
    public GameObject fotoMakanan;
    public Canvas cutsceneCanvas;
    public GameObject successText;

    [Header("Position")]
    public Transform playerHand; // Reference to the player's hand transform
    private Vector3 triggerPosition;
    public Vector3 baju1Position, baju2Position, baju3Position;

    [Header("Bool")]
    public bool isDroppingAtTrigger = false;
    public bool fotoMakananInstantiated = false;

    [Header("Audio")]
    public AudioClip pickupSound; // Add this for the pickup sound effect
    private AudioSource audioSource; // Add this to play the sound effect

    void Start()
    {
        // Initialize the audio source
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("drag") && selectedObject == null)
        {
            Debug.Log("Bisa digrab");
            pickupText.SetActive(true);
            dropText.SetActive(false);

            if (Input.GetKeyDown(KeyCode.E))
            {
                selectedObject = other.gameObject;
                selectedObject.transform.SetParent(playerHand);
                selectedObject.transform.localPosition = Vector3.zero;
                pickupText.SetActive(false);
                dropText.SetActive(false);

                // Play the pickup sound effect
                if (pickupSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(pickupSound);
                }
            }
        }
        else if (selectedObject != null && other.CompareTag("BajuPlacement"))
        {
            Debug.Log("Bisa didrop");
            pickupText.SetActive(false);
            dropText.SetActive(true);
            triggerPosition = other.transform.position;
            isDroppingAtTrigger = true;
        }
        else if (other.CompareTag("lemari"))
        {
            pickupText.SetActive(true);
            dropText.SetActive(false);
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(DisplayMessage("Why is it empty in here", 2));
            }
        }
        else
        {
            pickupText.SetActive(false);
            dropText.SetActive(false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("drag"))
        {
            pickupText.SetActive(false);
            dropText.SetActive(false);
        }

        if (other.CompareTag("lemari"))
        {
            pickupText.SetActive(false);
            dropText.SetActive(false);
        }

        if (selectedObject != null && other.CompareTag("BajuPlacement"))
        {
            pickupText.SetActive(false);
            dropText.SetActive(false);
            isDroppingAtTrigger = false;
        }
    }

    private IEnumerator DisplayMessage(string message, float delay)
    {
        messageText.SetActive(true);
        yield return new WaitForSeconds(delay);
        messageText.SetActive(false);
    }

    private void Update()
    {
        if (selectedObject != null && Input.GetKeyDown(KeyCode.F))
        {
            if (isDroppingAtTrigger)
            {
                DropAtTriggerPosition();
            }
            else
            {
                DropAtCurrentPosition();
            }
        }

        CheckPositions();
    }

    private void DropAtTriggerPosition()
    {
        Debug.Log("Dropping at trigger position.");
        selectedObject.transform.position = triggerPosition;
        DropSelectedObject();
        isDroppingAtTrigger = false;
        dropText.SetActive(false);
        pickupText.SetActive(false);
    }

    private void DropAtCurrentPosition()
    {
        Debug.Log("Dropping at current position.");
        Vector3 currentPosition = selectedObject.transform.position;
        currentPosition.y = 7.8f;
        selectedObject.transform.position = currentPosition;
        DropSelectedObject();
        dropText.SetActive(false);
        pickupText.SetActive(false);
    }

    private void DropSelectedObject()
    {
        if (selectedObject != null)
        {
            selectedObject.transform.rotation = Quaternion.Euler(-85, 133, -8.4f);
            selectedObject.transform.SetParent(null);
            selectedObject = null;
        }
    }

    void HideSuccessText()
    {
        successText.SetActive(false); // Sembunyikan teks berhasil
    }

    private void CheckPositions()
    {
        if (!fotoMakananInstantiated && baju1 != null && baju2 != null && baju3 != null)
        {
            if (Vector3.Distance(baju1.transform.position, baju1Position) < 0.5f &&
                Vector3.Distance(baju2.transform.position, baju2Position) < 0.5f &&
                Vector3.Distance(baju3.transform.position, baju3Position) < 0.5f)
            {
                successText.SetActive(true);
                Invoke("HideSuccessText", 2.0f);
                Debug.Log("All objects are in position. Instantiating new object.");
                Instantiate(fotoMakanan, new Vector3(59.2f, 2.19f, -3f), Quaternion.Euler(-88.077f, -77.45f, -11.8f));
                fotoMakananInstantiated = true;
                Lantai21.SetActive(false);
                Lantai22.SetActive(false);
                Lantai23.SetActive(false);
            }
        }
    }
}
