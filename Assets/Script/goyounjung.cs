using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goyounjung : MonoBehaviour
{
    public GameObject intText;
    public bool interactable, toggle;
    public Animator doorAnim;
    public AudioSource audioSource;
    public AudioClip openSound;

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
            doorAnim.SetTrigger("open");
            audioSource.PlayOneShot(openSound);
            intText.SetActive(false);
            interactable = false;
        }
    }
}
}