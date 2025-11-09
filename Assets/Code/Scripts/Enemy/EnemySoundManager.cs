using UnityEngine;

public class EnemySoundManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
public static AudioManager Instance;
 
    [SerializeField] private AudioClip awakeClip;
    [SerializeField] private AudioClip moveClip;
    [SerializeField] private AudioClip attackClip;
    [SerializeField] private AudioClip deathClip;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAwake() => PlayWithPitch(awakeClip);
    public void PlayMove() => PlayWithPitch(moveClip);
    public void PlayAttack() => PlayWithPitch(attackClip);
    public void PlayDeath() => PlayWithPitch(deathClip);


    public void PlayWithPitch(AudioClip source)
    {
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(source);

    }
}
