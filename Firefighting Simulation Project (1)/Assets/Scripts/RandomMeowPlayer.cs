using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class RandomMeowPlayer : MonoBehaviour
{
    [System.Serializable]
    public class WeightedClip
    {
        public AudioClip clip;
        [Tooltip("Higher = more likely to play. Relative to other clips.")]
        public float weight = 1f;
    }

    [Header("Clips")]
    public WeightedClip[] clips;

    [Header("Timing")]
    [Tooltip("Base interval between meows (seconds).")]
    public float interval = 4f;
    public float intervalJitter = 0.5f;

    [Header("Variation (optional)")]
    [Range(0f, 0.5f)] public float pitchJitter = 0.1f;
    [Range(0f, 0.3f)] public float volumeJitter = 0.1f;
    public bool avoidRepeats = true;

    private AudioSource audioSource;
    private int lastIndex = -1;
    private float totalWeight;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        RecalculateWeights();
    }

    void OnValidate()
    {
        RecalculateWeights();
    }

    void RecalculateWeights()
    {
        totalWeight = 0f;
        if (clips == null) return;
        foreach (var c in clips)
            if (c != null && c.clip != null)
                totalWeight += Mathf.Max(0f, c.weight);
    }

    void Start()
    {
        StartCoroutine(MeowLoop());
    }

    IEnumerator MeowLoop()
    {
        while (true)
        {
            float wait = interval + Random.Range(-intervalJitter, intervalJitter);
            yield return new WaitForSeconds(Mathf.Max(0.1f, wait));
            PlayRandomMeow();
        }
    }

    void PlayRandomMeow()
    {
        if (clips == null || clips.Length == 0 || totalWeight <= 0f) return;

        int index = PickWeightedIndex();
        if (index < 0) return;

        lastIndex = index;
        var chosen = clips[index].clip;

        audioSource.pitch = 1f + Random.Range(-pitchJitter, pitchJitter);
        float volume = Mathf.Clamp01(1f + Random.Range(-volumeJitter, volumeJitter));
        audioSource.PlayOneShot(chosen, volume);
    }

    int PickWeightedIndex()
    {
        int attempts = (avoidRepeats && clips.Length > 1) ? 5 : 1;
        for (int a = 0; a < attempts; a++)
        {
            float roll = Random.Range(0f, totalWeight);
            float cumulative = 0f;
            for (int i = 0; i < clips.Length; i++)
            {
                if (clips[i] == null || clips[i].clip == null) continue;
                cumulative += Mathf.Max(0f, clips[i].weight);
                if (roll <= cumulative)
                {
                    if (avoidRepeats && i == lastIndex && a < attempts - 1) break;
                    return i;
                }
            }
        }
        return -1;
    }
}
