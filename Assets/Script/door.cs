using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject intText;
    public bool interactable, toggle;
    public Animator doorAnim;
    public AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound;

    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("MainCamera"))
        {
            intText.SetActive(true);
            interactable = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("MainCamera"))
        {
            intText.SetActive(false);
            interactable = false;
        }
    }

    void Update()
    {
        if(interactable == true)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                toggle = !toggle;
                if(toggle == true)
                {
                    doorAnim.ResetTrigger("close");
                    doorAnim.SetTrigger("open");
                    audioSource.PlayOneShot(openSound);
                }
                if(toggle == false)
                {
                    doorAnim.ResetTrigger("open");
                    doorAnim.SetTrigger("close");
                    audioSource.PlayOneShot(closeSound);
                }
                intText.SetActive(false);
                interactable = false;
            }
        }
    }
}
