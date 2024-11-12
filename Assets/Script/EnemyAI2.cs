using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI2 : MonoBehaviour
{
    public NavMeshAgent ai;
    public float chaseSpeed, catchDistance, jumpscareTime;
    public Transform player;
    public Camera mainCamera; // Referensi ke kamera utama
    public Camera jumpscareCamera; // Referensi ke kamera jumpscare
    private Vector3 initialPlayerPosition; // Menyimpan posisi awal pemain
    public Vector3 rayCastOffset;
    public float maxChaseDistance; // Batas maksimum jarak pengejaran
    public Animator aiAnim; // Referensi ke Animator
    public SoundEffectsPlayer1 soundEffects; // Referensi ke SoundEffectsPlayer1

    void Start()
    {
        initialPlayerPosition = player.position; // Simpan posisi awal pemain

        if (jumpscareCamera != null)
        {
            jumpscareCamera.gameObject.SetActive(false); // Nonaktifkan kamera jumpscare pada awalnya
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, ai.transform.position);

        // Hentikan pengejaran jika jarak melebihi batas yang ditentukan
        if (distance > maxChaseDistance)
        {
            StopChasing();
            return; // Keluar dari update jika di luar jarak maksimum
        }
        
        // Lanjutkan pengejaran jika jarak di dalam batas maksimum
        ai.destination = player.position;
        ai.speed = chaseSpeed;

        // Memastikan NavMeshAgent mengikuti jalur
        if (ai.pathPending || ai.pathStatus == NavMeshPathStatus.PathPartial)
        {
            return; // Jika path belum siap atau tidak lengkap, keluar
        }

        if (distance <= catchDistance)
        {
            Debug.Log("Player caught!");
            player.gameObject.SetActive(false);
            StartCoroutine(deathRoutine());
        }

        // Mainkan sound effect saat musuh mulai mengejar
        if (!soundEffects.Chasemusicbg.isPlaying)
        {
            soundEffects.bgm2(); // Gunakan sound effect dari SoundEffectsPlayer1
            soundEffects.sfx2(); // Mainkan sound effect pengejaran
        }
    }

    void StopChasing()
    {
        ai.destination = transform.position; // Set destination ke posisi musuh untuk berhenti
        ai.speed = 0;

        if (soundEffects.Chasemusicbg.isPlaying)
        {
            soundEffects.Chasemusicbg.Stop(); // Hentikan sound effect
        }
    }

    IEnumerator deathRoutine()
    {
        // Ubah animasi ke "roar"
        aiAnim.SetTrigger("roar");
        soundEffects.SFXSource.Stop();
        soundEffects.sfx3();

        if (jumpscareCamera != null && mainCamera != null)
        {
            mainCamera.gameObject.SetActive(false); // Nonaktifkan kamera utama
            jumpscareCamera.gameObject.SetActive(true); // Aktifkan kamera jumpscare
        }

        yield return new WaitForSeconds(jumpscareTime);

        if (jumpscareCamera != null && mainCamera != null)
        {
            jumpscareCamera.gameObject.SetActive(false); // Nonaktifkan kamera jumpscare
            mainCamera.gameObject.SetActive(true); // Aktifkan kembali kamera utama
        }

        // Kembalikan posisi pemain ke posisi awal
        player.position = initialPlayerPosition;
        player.gameObject.SetActive(true); // Aktifkan kembali pemain
        
        // Hentikan pengejaran dan reset posisi musuh
        StopChasing();

        // Ubah animasi kembali ke "run" setelah jumpscare selesai
        aiAnim.ResetTrigger("roar");
        aiAnim.SetTrigger("sprint");
    }
}