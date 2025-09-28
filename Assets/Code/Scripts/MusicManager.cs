using UnityEngine;
using UnityEngine.SceneManagement;


public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Background Music")]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip level1Music;
    [SerializeField] private AudioClip loseMusic;
    [SerializeField] private AudioClip winMusic;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
    }
    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        switch (sceneName)
        {
            case "MainMenu":
                PlayMainMenu();
                break;
            case "Level1_1":
                PlayLevel1();
                break;
        }
    }
    public void PlayMainMenu() => PlayClip(mainMenuMusic);
    public void PlayLevel1()   => PlayClip(level1Music);
    public void PlayLose()     => PlayClip(loseMusic);
    public void PlayWin()      => PlayClip(winMusic);

    private void PlayClip(AudioClip clip)
    {
        if (clip == null) return;
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.volume = 0.35f;
        audioSource.Play();
    }

    public void SetVolume(float v)
    {
        audioSource.volume = Mathf.Clamp01(v);
    }
}