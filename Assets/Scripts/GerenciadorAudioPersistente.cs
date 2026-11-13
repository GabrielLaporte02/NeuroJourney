using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[DisallowMultipleComponent]
public class GerenciadorAudioPersistente : MonoBehaviour
{
    public static GerenciadorAudioPersistente instancia { get; private set; }

    [Header("Música de fundo")]
    [Tooltip("Arraste aqui a música que será reproduzida entre as cenas.")]
    public AudioClip musicaDeFundo;

    [Range(0f, 1f)]
    public float volume = 0.5f;

    private AudioSource fonteAudio;

    void Awake()
    {
        fonteAudio = GetComponent<AudioSource>();
        fonteAudio.playOnAwake = false;

        if (instancia != null && instancia != this)
        {
            fonteAudio.Stop();
            Destroy(gameObject);
            return;
        }

        instancia = this;

        // DontDestroyOnLoad exige um objeto na raiz da hierarquia.
        if (transform.parent != null)
            transform.SetParent(null);

        DontDestroyOnLoad(gameObject);

        fonteAudio.clip = musicaDeFundo;
        fonteAudio.loop = true;
        fonteAudio.spatialBlend = 0f;
        fonteAudio.volume = Mathf.Clamp01(volume);

        if (musicaDeFundo != null)
            fonteAudio.Play();
        else
            Debug.LogWarning("Arraste um AudioClip para Música De Fundo no Inspector.", this);
    }

    void OnValidate()
    {
        volume = Mathf.Clamp01(volume);

        // Permite ajustar o volume no Inspector durante a execução.
        if (fonteAudio != null)
            fonteAudio.volume = volume;
    }

    void OnDestroy()
    {
        if (instancia == this)
            instancia = null;
    }
}
