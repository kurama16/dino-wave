using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip buildClip;
    [SerializeField] private AudioClip enemyHitClip;
    [SerializeField] private AudioClip playerHitClip;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();

    }

    public void PlayShoot() => PlayWithPitch(shootClip);
    public void PlayBuild() => audioSource.PlayOneShot(buildClip);
    public void PlayEnemyHit() => PlayWithPitch(enemyHitClip);
    public void PlayPlayerHit() => PlayWithPitch(playerHitClip);


    public void PlayWithPitch(AudioClip source)
    {
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(source);

    }
}