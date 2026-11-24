using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[DisallowMultipleComponent]
public class GerenciadorAudioPersistente : MonoBehaviour
{
    public static GerenciadorAudioPersistente instancia { get; private set; }

    [Header("Música de fundo")]
    [Tooltip("Tema inicial. Menu e Mapa continuam usando o tema original.")]
    public AudioClip musicaDeFundo;

    [Range(0f, 1f)]
    public float volume = 0.5f;

    [Header("Troca entre cenas")]
    [SerializeField, Min(0.02f)] private float _duracaoTroca = 0.6f;

    private AudioSource fonteAudio;
    private AudioClip _destino;
    private Coroutine _transicao;
    private float _ganhoFade = 1f;
    private float _tempoSalvar;
    private static bool _preferenciaPendente;

    public static event System.Action<float> VolumeMusicaAlterado;
    public static float VolumeMusica
    {
        get
        {
            float valor = PlayerPrefs.GetFloat("VolumeMusica", 1f);
            return float.IsNaN(valor) || float.IsInfinity(valor) ? 1f : Mathf.Clamp01(valor);
        }
    }

    public static void DefinirVolumeMusica(float valor)
    {
        if (float.IsNaN(valor) || float.IsInfinity(valor))
            return;
        valor = Mathf.Clamp01(valor);
        PlayerPrefs.SetFloat("VolumeMusica", valor);
        if (valor > 0f)
            PlayerPrefs.SetFloat("UltimoVolumeMusica", valor);
        _preferenciaPendente = true;
        if (instancia != null)
        {
            instancia._tempoSalvar = 0f;
            instancia.AplicarVolume();
        }
        else
            SalvarPreferenciasVolume();
        VolumeMusicaAlterado?.Invoke(valor);
    }

    public static void AlternarMudoMusica()
    {
        if (VolumeMusica > 0f)
        {
            PlayerPrefs.SetFloat("UltimoVolumeMusica", VolumeMusica);
            DefinirVolumeMusica(0f);
        }
        else
        {
            float anterior = PlayerPrefs.GetFloat("UltimoVolumeMusica", 1f);
            DefinirVolumeMusica(float.IsNaN(anterior) || float.IsInfinity(anterior) || anterior <= 0f ? 1f : anterior);
        }
        SalvarPreferenciasVolume();
    }

    public static void SalvarPreferenciasVolume()
    {
        if (!_preferenciaPendente)
            return;
        PlayerPrefs.Save();
        _preferenciaPendente = false;
    }

    private void AplicarVolume()
    {
        if (fonteAudio != null)
            fonteAudio.volume = Mathf.Clamp01(volume) * VolumeMusica * _ganhoFade;
    }

    private void Update()
    {
        if (!_preferenciaPendente)
            return;
        _tempoSalvar += Time.unscaledDeltaTime;
        if (_tempoSalvar >= 0.25f)
            SalvarPreferenciasVolume();
    }

    private void OnApplicationPause(bool pausado)
    {
        if (pausado)
            SalvarPreferenciasVolume();
    }

    private void OnApplicationQuit()
    {
        SalvarPreferenciasVolume();
    }

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
        if (transform.parent != null)
            transform.SetParent(null);
        DontDestroyOnLoad(gameObject);

        fonteAudio.clip = musicaDeFundo;
        fonteAudio.loop = true;
        fonteAudio.spatialBlend = 0f;
        AplicarVolume();
        _destino = musicaDeFundo;

        if (musicaDeFundo != null)
            fonteAudio.Play();
        else
            Debug.LogWarning("Arraste um AudioClip para Música De Fundo no Inspector.", this);
    }

    // Permite iniciar diretamente em uma fase sem depender de passar pelo Menu.
    public static GerenciadorAudioPersistente ObterOuCriar(AudioClip musicaInicial, float volumeInicial)
    {
        if (instancia != null)
            return instancia;

        var objeto = new GameObject("AudioPersistente", typeof(AudioSource));
        objeto.SetActive(false);
        var gerente = objeto.AddComponent<GerenciadorAudioPersistente>();
        gerente.musicaDeFundo = musicaInicial;
        gerente.volume = Mathf.Clamp01(volumeInicial);
        objeto.SetActive(true);
        return gerente;
    }

    public void TrocarMusica(AudioClip novaMusica)
    {
        if (!isActiveAndEnabled || fonteAudio == null)
            return;
        fonteAudio.loop = true;
        fonteAudio.spatialBlend = 0f;

        // Antes/Depois e Menu/Mapa não reiniciam uma faixa que já está tocando.
        if (_destino == novaMusica && (_transicao != null || fonteAudio.isPlaying))
            return;
        _destino = novaMusica;
        if (_transicao != null)
            StopCoroutine(_transicao);

        if (fonteAudio.clip == novaMusica)
        {
            if (novaMusica != null && !fonteAudio.isPlaying)
                fonteAudio.Play();
            _transicao = StartCoroutine(AjustarVolume(1f, Mathf.Max(0.01f, _duracaoTroca * 0.5f)));
        }
        else
            _transicao = StartCoroutine(TrocarFaixa(novaMusica));
    }

    private System.Collections.IEnumerator TrocarFaixa(AudioClip novaMusica)
    {
        float metade = Mathf.Max(0.01f, _duracaoTroca * 0.5f);
        yield return Fade(0f, metade);
        fonteAudio.Stop();
        fonteAudio.clip = novaMusica;
        if (novaMusica != null)
        {
            fonteAudio.Play();
            yield return Fade(1f, metade);
        }
        _transicao = null;
    }

    private System.Collections.IEnumerator AjustarVolume(float destino, float duracao)
    {
        yield return Fade(destino, duracao);
        _transicao = null;
    }

    private System.Collections.IEnumerator Fade(float destino, float duracao)
    {
        float inicial = _ganhoFade;
        float tempo = 0f;
        while (tempo < duracao)
        {
            tempo += Time.unscaledDeltaTime;
            _ganhoFade = Mathf.Lerp(inicial, destino, Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(tempo / duracao)));
            AplicarVolume();
            yield return null;
        }
        _ganhoFade = destino;
        AplicarVolume();
    }

    void OnValidate()
    {
        volume = Mathf.Clamp01(volume);
        AplicarVolume();
    }

    void OnDisable()
    {
        if (_transicao != null)
        {
            StopCoroutine(_transicao);
            _transicao = null;
        }
        _ganhoFade = 1f;
        AplicarVolume();
        SalvarPreferenciasVolume();
    }

    void OnDestroy()
    {
        if (instancia == this)
            instancia = null;
    }
}
