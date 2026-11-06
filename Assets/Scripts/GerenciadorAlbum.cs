
using UnityEngine;

public class GerenciadorAlbum : MonoBehaviour
{
    [Header("Figurinhas")]
    public GameObject figurinhaDona;
    public GameObject figurinhaCapitao;
    public GameObject figurinhaLuna;
    public GameObject figurinhaZeca;

    void OnEnable()
    {
        AtualizarAlbum();
    }

    public void AtualizarAlbum()
    {
        figurinhaDona.SetActive(
            SistemaFigurinhas.EstaDesbloqueada("DonaBiscoito")
        );

        figurinhaCapitao.SetActive(
            SistemaFigurinhas.EstaDesbloqueada("CapitaoBigode")
        );

        figurinhaLuna.SetActive(
            SistemaFigurinhas.EstaDesbloqueada("Luna")
        );

        figurinhaZeca.SetActive(
            SistemaFigurinhas.EstaDesbloqueada("Zeca")
        );
    }
}
