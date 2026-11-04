
using UnityEngine;
using TMPro;

public class DialogoParqueAntes : MonoBehaviour
{
    // FASE 2
    private bool dialogoFinalAtivo = false;

    [Header("Fase 2 - Memória")]
    public GameObject painelMemoria;
    public JogoMemoriaParque jogoMemoria;

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
        "Luna",
        "Theo",
        "Luna"
    };

    private string[] falas = {
        "Oi, Luna! Por que você está aqui tão quietinha?",
        "Theo... Está tudo tão escuro aqui. Ouvi uns barulhos estranhos e meu coração começou a bater bem rápido...",
        "Percebi que você está tremendo um pouquinho. Quer me contar o que aconteceu?",
        "Eu queria brincar no parque, mas não consigo chegar perto dos brinquedos..."
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

        if (emocao == "Medo")
        {
            textoFeedback.text =
                "Muito bem! Luna parece estar com medo. " +
                "Ela está tremendo, seu coração está batendo rápido " +
                "e ela não consegue se aproximar dos brinquedos.";
        }
        else
        {
            textoFeedback.text =
                "Boa tentativa! Você escolheu " + emocao +
                ". Mas Luna parece estar com medo. " +
                "Percebeu que ela está tremendo e que seu coração " +
                "está batendo rápido por causa dos barulhos e da escuridão?";
        }
    }

    public void ContinuarAposFeedback()
    {
        painelFeedback.SetActive(false);
        painelDialogo.SetActive(true);

        falaAtual++;
        MostrarFala();
    }

    public void IniciarFaseMemoria()
    {
        painelDialogo.SetActive(false);
        painelPergunta.SetActive(false);
        painelFeedback.SetActive(false);

        painelMemoria.SetActive(true);
        jogoMemoria.IniciarJogo();
    }

    public void IniciarDialogoFinal()
    {
        dialogoFinalAtivo = true;

        painelMemoria.SetActive(false);
        painelDialogo.SetActive(true);

        nomePersonagem.text = "Luna";

        textoDialogo.text =
            "Theo, você me ajudou a descobrir coisas " +
            "que podem me fazer sentir mais segura! " +
            "Quando sinto medo, posso usar minha lanterna, " +
            "abraçar meu ursinho ou pedir ajuda " +
            "a alguém em quem confio. " +
            "Agora sei que não preciso enfrentar tudo sozinha!";
    }
}
