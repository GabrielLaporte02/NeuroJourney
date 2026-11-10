
using UnityEngine;

public class AlbumInterativo : MonoBehaviour
{
    public GameObject painelAlbum;
    public AnimacaoConquista animacaoConquistaHeroi;

    void OnMouseDown()
    {
        if (painelAlbum != null)
        {
            painelAlbum.SetActive(true);
            VerificarHeroi();
        }
    }

    void VerificarHeroi()
    {
        bool todasConquistadas =
            SistemaFigurinhas.EstaDesbloqueada("DonaBiscoito") &&
            SistemaFigurinhas.EstaDesbloqueada("CapitaoBigode") &&
            SistemaFigurinhas.EstaDesbloqueada("Luna") &&
            SistemaFigurinhas.EstaDesbloqueada("Zeca");

        if (todasConquistadas &&
            !SistemaFigurinhas.EstaDesbloqueada("HeroiVizinhanca"))
        {
            if (animacaoConquistaHeroi != null)
            {
                animacaoConquistaHeroi.MostrarConquista();

                painelAlbum.GetComponent<GerenciadorAlbum>().AtualizarAlbum();
            }
            else
            {
                Debug.LogError("Animação do Herói não configurada!");
}
        }
    }

    public void FecharAlbum()
    {
        painelAlbum.SetActive(false);
    }
}
