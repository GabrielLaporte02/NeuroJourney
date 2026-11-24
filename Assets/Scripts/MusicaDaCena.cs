using UnityEngine;

[DisallowMultipleComponent]
public class MusicaDaCena : MonoBehaviour
{
    [Header("Tema desta cena")]
    [SerializeField] private AudioClip _musica;
    [Tooltip("Volume do Menu, usado apenas quando a cena é iniciada sem gerenciador persistente.")]
    [SerializeField, Range(0f, 1f)] private float _volumeInicial = 0.561f;

    private void Start()
    {
        if (_musica == null)
        {
            Debug.LogWarning("MusicaDaCena: nenhuma faixa configurada; música atual preservada.", this);
            return;
        }
        GerenciadorAudioPersistente gerente =
            GerenciadorAudioPersistente.ObterOuCriar(_musica, _volumeInicial);
        gerente.TrocarMusica(_musica);
    }
}
