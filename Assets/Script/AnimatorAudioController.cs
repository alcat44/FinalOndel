using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorAudioController : MonoBehaviour
{
    public Animator animator;
    public AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ItemDrop.Instance.anidio == true)
        {
            animator.enabled = false;
            audio.enabled = false;

        }
    }
}
