
using UnityEngine;
using TMPro;

public class DialogoPlazaDepois : MonoBehaviour
{
    public GameObject painelDialogoFinal;
    public TMP_Text nomePersonagem;
    public TMP_Text textoDialogo;

    [Header("Conquista")]
    public AnimacaoConquista animacaoConquista;

    private int falaAtual = 0;
    private bool dialogoFinalizado = false;

    private string[] nomes = {
        "Zeca",
        "Theo",
        "Zeca"
    };

    private string[] falas = {
        "Theo! Olha só! A praça está muito mais organizada! E eu ainda estou superfeliz!",

        "Que bom, Zeca! Agora você consegue aproveitar os jogos sem precisar fazer tudo de uma vez!",

        "É verdade! Posso escolher um jogo, fazer pausas e respirar devagar quando precisar. Assim consigo brincar e me divertir do meu jeito!"
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

            // Marca a Plaza como restaurada
            PlayerPrefs.SetInt("PlazaConcluida", 1);
            PlayerPrefs.Save();

            // Esconde o diálogo final
            painelDialogoFinal.SetActive(false);

            // Mostra a conquista somente na primeira vez
            if (!SistemaFigurinhas.EstaDesbloqueada("Zeca"))
            {
                if (animacaoConquista != null)
                {
                    animacaoConquista.MostrarConquista();
                }
                else
                {
                    Debug.LogError(
                        "AnimacaoConquista não configurada na Plaza!"
                    );
                }
            }
        }
    }
}
