
using UnityEngine;
using TMPro;

public class DialogoCorreiosAntes : MonoBehaviour
{
    // FASE 2
    private bool dialogoFinalAtivo = false;

    [Header("Fase 2 - Memória")]
    public GameObject painelMemoria;
    public JogoMemoriaCorreios jogoMemoria;

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
        "Capitão Bigode",
        "Theo",
        "Capitão Bigode"
    };

    private string[] falas = {
        "Oi, Capitão Bigode! Nossa, quantas cartas espalhadas!",
        "Argh! Tenho tantas entregas para fazer! Está tudo bagunçado e nada está saindo como planejei!",
        "Você está falando bem alto e parece incomodado. Posso ajudar?",
        "Grrr... Eu queria terminar meu trabalho, mas está tudo tão difícil hoje!"
    };

    void Start()
    {
        falaAtual = 0;
        perguntaRespondida = false;

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

        if (emocao == "Raiva")
        {
            textoFeedback.text =
                "Muito bem! O Capitão Bigode parece estar com raiva. " +
                "Ele está falando alto e demonstrando irritação " +
                "porque as coisas não saíram como esperava.";
        }
        else
        {
            textoFeedback.text =
                "Boa tentativa! Você escolheu " + emocao +
                ". Mas o Capitão Bigode parece estar com raiva. " +
                "Percebeu que ele está falando alto " +
                "e demonstrando irritação com a bagunça?";
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

        painelMemoria.SetActive(true);
        jogoMemoria.IniciarJogo();
    }

    public void IniciarDialogoFinal()
    {
        dialogoFinalAtivo = true;
        painelMemoria.SetActive(false);
        painelDialogo.SetActive(true);

        nomePersonagem.text = "Capitão Bigode";

        textoDialogo.text =
            "Theo, você me ajudou a descobrir " +
            "coisas que podem me acalmar! " +
            "Quando fico irritado, posso respirar devagar, " +
            "tomar meu chá ou olhar fotos dos meus gatinhos. " +
            "Agora consigo pensar com mais tranquilidade!";
    }
}
