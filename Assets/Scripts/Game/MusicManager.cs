using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour {
    public static MusicManager Instance { get; private set; }

    private AudioSource audioSource;

    [SerializeField] private AudioClip[] musicClips;
    [SerializeField] private AudioClip gameOverJingle;

    private void Awake() {
        if (Instance != null & Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }

        transform.position = Camera.main.transform.position;

        audioSource = GetComponent<AudioSource>();

        PlayRandomSong();
    }

    public void PlayRandomSong() {
        if (!audioSource) return;

        audioSource.clip = musicClips[Random.Range(0, musicClips.Length)];
        audioSource.Play();
    }

    public void GameOverMusic() {
        if (!audioSource) return;

        audioSource.Stop();
        audioSource.clip = gameOverJingle;
        audioSource.Play();
    }
}
