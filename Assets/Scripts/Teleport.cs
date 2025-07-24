using UnityEngine;

public class Teleport : MonoBehaviour
{
    [Header("Основные настройки")]
    public string targetSceneName;
    public float teleportDelay = 2f;

    [Header("Аудио")]
    [Tooltip("Перетащите аудиофайл сюда, если нужен звук")]
    public AudioClip teleportSound; // Назначаете ВРУЧНУЮ в инспекторе

    private AudioSource _audioSource; // Автоматически создаётся
    private bool _isPlayerInside;

    private void Awake()
    {
        // Автоматически добавляем AudioSource, но не назначаем звук
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = true;
            Invoke(nameof(StartTeleport), teleportDelay);

            // Звук сработает ТОЛЬКО если вы назначили teleportSound
            if (teleportSound != null)
            {
                _audioSource.PlayOneShot(teleportSound);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = false;
            CancelInvoke(nameof(StartTeleport));
        }
    }

    private void StartTeleport()
    {
        if (_isPlayerInside && FadeTransition.Instance != null)
        {
            FadeTransition.Instance.StartCoroutine(
                FadeTransition.Instance.FadeAndLoadScene(targetSceneName)
            );
        }
    }
}