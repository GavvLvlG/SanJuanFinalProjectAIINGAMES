using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GameAudio : MonoBehaviour
{
    private const string MenuSceneName = "Menu";

    public static GameAudio Instance { get; private set; }

    [Header("Background music sources")]
    [SerializeField] private AudioSource menuMusicSource;
    [SerializeField] private AudioSource gameplayMusicSource;

    [Header("Sound effects")]
    [SerializeField] private AudioClip playerPaddleSound;
    [SerializeField] private AudioClip cpuPaddleSound;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioSource effectsSource;
    [SerializeField, Range(0f, 1f)] private float soundEffectsVolume = 1f;

    private AudioSource activeMusicSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        PlayMusicForScene(SceneManager.GetActiveScene().name);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (Instance == this)
            Instance = null;
    }

    /// <summary>Plays the sound associated with the player or CPU paddle.</summary>
    public void PlayPaddleSound(bool isPlayerPaddle)
    {
        AudioClip clip = isPlayerPaddle ? playerPaddleSound : cpuPaddleSound;
        PlayEffect(clip);
    }

    /// <summary>Stops the currently selected background music source.</summary>
    public void StopBackgroundMusic()
    {
        if (activeMusicSource == null)
            return;

        activeMusicSource.Stop();
        activeMusicSource = null;
    }

    /// <summary>Plays the game-over sound effect.</summary>
    public void PlayGameOverSound()
    {
        PlayEffect(gameOverSound);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    private void PlayMusicForScene(string sceneName)
    {
        AudioSource musicSource = sceneName == MenuSceneName
            ? menuMusicSource
            : gameplayMusicSource;

        if (musicSource == null || musicSource.clip == null)
            return;

        if (activeMusicSource != null && activeMusicSource != musicSource)
            activeMusicSource.Stop();

        activeMusicSource = musicSource;
        if (!activeMusicSource.isPlaying)
            activeMusicSource.Play();
    }

    private void PlayEffect(AudioClip clip)
    {
        if (effectsSource == null || clip == null)
            return;

        effectsSource.PlayOneShot(clip, soundEffectsVolume);
    }
}
