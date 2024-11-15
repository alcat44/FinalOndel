using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class AINavigationall : MonoBehaviour
{
    public NavMeshAgent ai;
    public List<Transform> destinations;
    public Animator aiAnim;
    public float walkSpeed, chaseSpeed, minIdleTime, maxIdleTime, idleTime, sightDistance, catchDistance, chaseTime, minChaseTime, maxChaseTime, jumpscareTime;
    public bool walking, chasing, roaring;
    public Transform player;
    public Camera mainCamera; // Referensi ke kamera utama
    public Camera jumpscareCamera; // Referensi ke kamera jumpscare
    private Vector3 initialPlayerPosition; // Menyimpan posisi awal pemain
    Transform currentDest;
    Vector3 dest;
    int randNum;
    public string deathScene;
    public float aiDistance;
    public Vector3 rayCastOffset;

    [SerializeField] private Animator myAniationController;
    
    public SoundEffectsPlayer1 Audio;

    public List<Transform> checkpoints;
    private int currentCheckpointIndex = -1;

    private AudioSource audioSource;

    private string previousAnimationState;

    // Waktu maksimal untuk mencari destinasi
    public float maxSearchTime = 10f; // waktu maksimal dalam detik, bisa diubah di Inspector
    private float timeSpentSearching = 0f; // waktu yang dihabiskan untuk mencapai destinasi

    void Start()
    {
        walking = true;
        randNum = Random.Range(0, destinations.Count);
        currentDest = destinations[randNum];
        initialPlayerPosition = player.position; // Simpan posisi awal pemain
        if (jumpscareCamera != null)
        {
            jumpscareCamera.gameObject.SetActive(false); // Nonaktifkan kamera jumpscare pada awalnya
        }

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        RaycastHit hit;
        aiDistance = Vector3.Distance(player.position, this.transform.position);

        // Menghitung waktu yang dihabiskan untuk mencari destinasi
        if (walking && ai.remainingDistance > ai.stoppingDistance)
        {
            timeSpentSearching += Time.deltaTime;
            if (timeSpentSearching >= maxSearchTime)
            {
                // Waktu habis, berhenti dan cari destinasi baru
                SetIdle();
                randNum = Random.Range(0, destinations.Count);
                currentDest = destinations[randNum];
                timeSpentSearching = 0f; // Reset waktu pencarian
            }
        }
        else
        {
            // Reset waktu jika mencapai destinasi atau dalam keadaan idle
            timeSpentSearching = 0f;
        }

        if (Physics.Raycast(transform.position + rayCastOffset, direction, out hit, sightDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (!chasing)
                {
                    chasing = true;
                    walking = false;
                    StopCoroutine("stayIdle");
                    StopCoroutine("chaseRoutine");
                    StartCoroutine("chaseRoutine");

                    Audio.bgm();
                }
            }
        }

        if (roaring)
        {
            aiAnim.ResetTrigger("sprint");
            aiAnim.ResetTrigger("idle");
            aiAnim.ResetTrigger("walk");
            aiAnim.SetTrigger("roar");

            dest = currentDest.position;
            ai.destination = dest;
            ai.speed = walkSpeed;
        }

        if (chasing)
        {
            dest = player.position;
            ai.destination = dest;
            ai.speed = chaseSpeed;
            aiAnim.ResetTrigger("walk");
            aiAnim.ResetTrigger("idle");
            aiAnim.ResetTrigger("roar");
            aiAnim.SetTrigger("sprint");

            if (previousAnimationState != "sprint")
            {
                Audio.sfx2();
                Audio.sfx5();
                previousAnimationState = "sprint";
            }

            float distance = Vector3.Distance(player.position, ai.transform.position);
            if (aiDistance <= catchDistance)
            {
                player.gameObject.SetActive(false);
                aiAnim.ResetTrigger("walk");
                aiAnim.ResetTrigger("idle");
                aiAnim.ResetTrigger("sprint");
                aiAnim.ResetTrigger("roar");
                StartCoroutine(deathRoutine());
                chasing = false;
            }
            else if (distance > 30)
            {
                chasing = false;
                walking = true;
                randNum = Random.Range(0, destinations.Count);
                currentDest = destinations[randNum];

                Audio.Chasemusicbg.Stop();
            }
        }
        else if (walking)
        {
            dest = currentDest.position;
            ai.destination = dest;
            ai.speed = walkSpeed;
            aiAnim.ResetTrigger("sprint");
            aiAnim.ResetTrigger("idle");
            aiAnim.ResetTrigger("roar");
            aiAnim.SetTrigger("walk");

            if (previousAnimationState != "walk")
            {
                Audio.sfx1();
                previousAnimationState = "walk";
            }

            if (ai.remainingDistance <= ai.stoppingDistance)
            {
                SetIdle();
            }
        }
    }

    void SetIdle()
    {
        aiAnim.ResetTrigger("sprint");
        aiAnim.ResetTrigger("walk");
        aiAnim.ResetTrigger("roar");
        aiAnim.SetTrigger("idle");
        ai.speed = 0;
        walking = false;

        if (previousAnimationState != "idle")
        {
            previousAnimationState = "idle";
            Audio.SFXSource.Stop();
            Audio.sfx4();
        }

        StopCoroutine("stayIdle");
        StartCoroutine("stayIdle");
    }

    public void stopChase()
    {
        walking = true;
        chasing = false;
        StopCoroutine("chaseRoutine");
        randNum = Random.Range(0, destinations.Count);
        currentDest = destinations[randNum];
        Audio.Chasemusicbg.Stop();
    }

    IEnumerator stayIdle()
    {
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        yield return new WaitForSeconds(idleTime);
        walking = true;
        randNum = Random.Range(0, destinations.Count);
        currentDest = destinations[randNum];
        Audio.Chasemusicbg.Stop();
    }

    IEnumerator chaseRoutine()
    {
        chaseTime = Random.Range(minChaseTime, maxChaseTime);
        yield return new WaitForSeconds(chaseTime);
        walking = true;
        chasing = false;
        randNum = Random.Range(0, destinations.Count);
        currentDest = destinations[randNum];
        Audio.Chasemusicbg.Stop();
    }

    IEnumerator deathRoutine()
    {
        if (jumpscareCamera != null && mainCamera != null)
        {
            Audio.SFXSource.Stop();
            Audio.sfx3();
            chasing = false;
            mainCamera.gameObject.SetActive(false); // Nonaktifkan kamera utama
            jumpscareCamera.gameObject.SetActive(true); // Aktifkan kamera jumpscare
            roaring = true;
        }
        yield return new WaitForSeconds(jumpscareTime);
        if (jumpscareCamera != null && mainCamera != null)
        {
            roaring = false;
            jumpscareCamera.gameObject.SetActive(false); // Nonaktifkan kamera jumpscare
            mainCamera.gameObject.SetActive(true); // Aktifkan kembali kamera utama
        }
        if (currentCheckpointIndex >= 0 && currentCheckpointIndex < checkpoints.Count)
        {
            player.position = checkpoints[currentCheckpointIndex].position;
            Debug.Log("Player moved to checkpoint: " + currentCheckpointIndex);
        }
        else
        {
            player.position = initialPlayerPosition;
            Debug.Log("Player moved to initial position.");
        }
        Audio.Chasemusicbg.Stop();
        player.gameObject.SetActive(true);
        walking = true;
        randNum = Random.Range(0, destinations.Count);
        currentDest = destinations[randNum];
        ai.destination = currentDest.position;
        ai.speed = walkSpeed;
        Audio.SFXSource.Stop();
    }

    public void SetCheckpoint(int checkpointIndex)
    {
        if (checkpointIndex >= 0 && checkpointIndex < checkpoints.Count)
        {
            currentCheckpointIndex = checkpointIndex;
            Debug.Log("Checkpoint set to: " + checkpointIndex);
        }
    }
}
