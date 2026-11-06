
using UnityEngine;
using TMPro;

public class DialogoParqueDepois : MonoBehaviour
{
    public GameObject painelDialogoFinal;
    public TMP_Text nomePersonagem;
    public TMP_Text textoDialogo;

    [Header("Conquista")]
    public AnimacaoConquista animacaoConquista;

    private int falaAtual = 0;
    private bool dialogoFinalizado = false;

    private string[] nomes = {
        "Luna",
        "Theo",
        "Luna"
    };

    private string[] falas = {
        "Theo! Agora consigo enxergar o parque direitinho! Estou me sentindo muito mais segura!",

        "Que bom, Luna! Descobrimos algumas coisas que podem ajudar quando você sente medo!",

        "Sim! Posso usar minha lanterna, abraçar meu ursinho ou pedir ajuda a alguém em quem confio. Obrigada por ficar comigo!"
    };

    void Start()
    {
        falaAtual = 0;
        dialogoFinalizado = false;

        painelDialogoFinal.SetActive(true);
        MostrarFala();
    }

    void MostrarFala()
    {
        nomePersonagem.text = nomes[falaAtual];
        textoDialogo.text = falas[falaAtual];
    }

    public void ProximaFala()
    {
        if (dialogoFinalizado) return;

        falaAtual++;

        if (falaAtual < falas.Length)
        {
            MostrarFala();
        }
        else
        {
            dialogoFinalizado = true;

            // Marca o Parque como restaurado
            PlayerPrefs.SetInt("ParqueConcluido", 1);
            PlayerPrefs.Save();

            // Esconde o diálogo final
            painelDialogoFinal.SetActive(false);

            // Mostra a conquista da figurinha
            if (animacaoConquista != null)
            {
                animacaoConquista.MostrarConquista();
            }
            else
            {
                Debug.LogError(
                    "AnimacaoConquista não configurada no Parque!"
                );
            }
        }
    }
}
