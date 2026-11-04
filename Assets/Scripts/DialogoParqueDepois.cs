
using UnityEngine;
using TMPro;

public class DialogoParqueDepois : MonoBehaviour
{
    public GameObject painelDialogoFinal;
    public TMP_Text nomePersonagem;
    public TMP_Text textoDialogo;

    private int falaAtual = 0;

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
        falaAtual++;

        if (falaAtual < falas.Length)
        {
            MostrarFala();
        }
        else
        {
            // Marca o parque como restaurado
            PlayerPrefs.SetInt("ParqueConcluido", 1);
            PlayerPrefs.Save();

            painelDialogoFinal.SetActive(false);
        }
    }
}
