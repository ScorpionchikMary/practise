using UnityEngine;

public class Teleport : MonoBehaviour
{
    [Header("�������� ���������")]
    public string targetSceneName;
    public float teleportDelay = 2f;

    [Header("�����")]
    [Tooltip("���������� ��������� ����, ���� ����� ����")]
    public AudioClip teleportSound; // ���������� ������� � ����������

    private AudioSource _audioSource; // ������������� ��������
    private bool _isPlayerInside;

    private void Awake()
    {
        // ������������� ��������� AudioSource, �� �� ��������� ����
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = true;
            Invoke(nameof(StartTeleport), teleportDelay);

            // ���� ��������� ������ ���� �� ��������� teleportSound
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