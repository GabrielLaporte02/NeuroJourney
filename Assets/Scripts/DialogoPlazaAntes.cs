
using UnityEngine;
using TMPro;

public class DialogoPlazaAntes : MonoBehaviour
{
    // FASE 2
    private bool dialogoFinalAtivo = false;

    [Header("Fase 2 - Memória")]
    public GameObject painelMemoria;
    public JogoMemoriaPlaza jogoMemoria;

    [Header("Painéis")]
    public GameObject painelDialogo;
    public GameObject painelPergunta;
    public GameObject painelFeedback;

    [Header("Textos do diálogo")]
    public TMP_Text nomePersonagem;
    public TMP_Text textoDialogo;

    [Header("Texto do feedback")]
    public TMP_Text textoFeedback;

    private int falaAtual = 0;
    private bool perguntaRespondida = false;

    private string[] nomes = {
        "Theo",
        "Zeca",
        "Theo",
        "Zeca"
    };

    private string[] falas = {
        "Oi, Zeca! Nossa, quantos jogos! O que está acontecendo aqui?",

        "THEO! OLHA ISSO! Tem jogo de corrida, de pular, de acertar alvos! Quero jogar TODOS! AGORA!",

        "Uau! Você está falando muito rápido e pulando de um lado para o outro!",

        "É QUE TEM TANTA COISA LEGAL! Nem sei por onde começar! Meu corpo parece que não consegue ficar parado!"
    };

    void Start()
    {
        falaAtual = 0;
        perguntaRespondida = false;
        dialogoFinalAtivo = false;

        painelPergunta.SetActive(false);
        painelFeedback.SetActive(false);
        painelDialogo.SetActive(true);

        MostrarFala();
    }

    void MostrarFala()
    {
        nomePersonagem.text = nomes[falaAtual];
        textoDialogo.text = falas[falaAtual];
    }

    public void ProximaFala()
    {
        if (dialogoFinalAtivo)
        {
            painelDialogo.SetActive(false);
            return;
        }

        if (falaAtual == 2 && !perguntaRespondida)
        {
            MostrarPergunta();
            return;
        }

        falaAtual++;

        if (falaAtual < falas.Length)
        {
            MostrarFala();
        }
        else
        {
            painelDialogo.SetActive(false);
            IniciarFaseMemoria();
        }
    }

    void MostrarPergunta()
    {
        painelDialogo.SetActive(false);
        painelPergunta.SetActive(true);
    }

    public void EscolherEmocao(string emocao)
    {
        painelPergunta.SetActive(false);
        painelFeedback.SetActive(true);

        perguntaRespondida = true;

        if (emocao == "Alegria")
        {
            textoFeedback.text =
                "Muito bem! Zeca parece estar muito alegre " +
                "e empolgado! Ele quer brincar em todos os jogos " +
                "ao mesmo tempo e está com tanta energia " +
                "que não sabe por onde começar!";
        }
        else
        {
            textoFeedback.text =
                "Boa tentativa! Você escolheu " + emocao +
                ". Mas Zeca parece estar muito alegre " +
                "e empolgado! Percebeu como ele fala rápido " +
                "e quer brincar em tudo ao mesmo tempo?";
        }
    }

    public void ContinuarAposFeedback()
    {
        painelFeedback.SetActive(false);
        painelDialogo.SetActive(true);

        // Avança para a fala seguinte
        falaAtual++;
        MostrarFala();
    }

    public void IniciarFaseMemoria()
    {
        painelDialogo.SetActive(false);
        painelPergunta.SetActive(false);
        painelFeedback.SetActive(false);

        if (painelMemoria == null)
        {
            Debug.LogError(
                "PainelMemoria não configurado no DialogoPlazaAntes!"
            );
            return;
        }

        if (jogoMemoria == null)
        {
            Debug.LogError(
                "JogoMemoriaPlaza não configurado no DialogoPlazaAntes!"
            );
            return;
        }

        painelMemoria.SetActive(true);
        jogoMemoria.IniciarJogo();
    }

    public void IniciarDialogoFinal()
    {
        dialogoFinalAtivo = true;

        painelMemoria.SetActive(false);
        painelDialogo.SetActive(true);

        nomePersonagem.text = "Zeca";

        textoDialogo.text =
            "Theo, você me ajudou a descobrir " +
            "que posso continuar me divertindo " +
            "sem precisar jogar tudo ao mesmo tempo! " +
            "Posso escolher um jogo, fazer pausas " +
            "e respirar devagar quando precisar. " +
            "Assim consigo aproveitar melhor a praça!";
    }
}
