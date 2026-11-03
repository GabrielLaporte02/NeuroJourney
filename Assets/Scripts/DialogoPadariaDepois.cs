
using UnityEngine;
using TMPro;

public class DialogoPadariaDepois : MonoBehaviour
{
    public GameObject painelDialogoFinal;
    public TMP_Text nomePersonagem;
    public TMP_Text textoDialogo;

    private int falaAtual = 0;

    private string[] nomes = {
        "Dona Biscoito",
        "Theo",
        "Dona Biscoito"
    };

    private string[] falas = {
        "Theo! Olha só como minha padaria ficou bonita! E eu já estou me sentindo muito melhor!",

        "Que bom, Dona Biscoito! Descobrimos tantas coisas que deixam você feliz!",

        "É verdade! E aprendi que, mesmo quando fico triste, posso fazer coisas de que gosto para me sentir melhor. Obrigada pela ajuda!"
    };

    void Start()
    {
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
            // Marca a padaria como restaurada
            PlayerPrefs.SetInt("PadariaConcluida", 1);
            PlayerPrefs.Save();

            painelDialogoFinal.SetActive(false);
        }
    }
}
