
using UnityEngine;
using TMPro;

public class DialogoCorreiosDepois : MonoBehaviour
{
    public GameObject painelDialogoFinal;
    public TMP_Text nomePersonagem;
    public TMP_Text textoDialogo;

    [Header("Conquista")]
    public AnimacaoConquista animacaoConquista;

    private int falaAtual = 0;
    private bool dialogoFinalizado = false;

    private string[] nomes = {
        "Capitão Bigode",
        "Theo",
        "Capitão Bigode"
    };

    private string[] falas = {
        "Ufa! Que alívio, Theo! Agora minha agência está organizada e consigo trabalhar com mais tranquilidade!",

        "Que bom, Capitão Bigode! Descobrimos várias coisas que podem ajudar você quando fica irritado!",

        "É verdade! Quando sinto raiva, posso respirar devagar, tomar meu chá ou olhar fotos dos meus gatinhos. Assim consigo me acalmar e pensar melhor. Obrigado pela ajuda!"
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

            // Marca os Correios como restaurados
            PlayerPrefs.SetInt("CorreiosConcluido", 1);
            PlayerPrefs.Save();

            // Esconde o diálogo
            painelDialogoFinal.SetActive(false);

            // Mostra a conquista da figurinha
            if (animacaoConquista != null)
            {
                animacaoConquista.MostrarConquista();
            }
            else
            {
                Debug.LogError(
                    "AnimacaoConquista não configurada nos Correios!"
                );
            }
        }
    }
}
