
using UnityEngine;
using TMPro;

public class DialogoPadaria : MonoBehaviour
{
    //FASE 2
    private bool dialogoFinalAtivo = false;

    [Header("Fase 2 - Memória")]
    public GameObject painelMemoria;
    public JogoMemoria jogoMemoria;

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
        "Dona Biscoito",
        "Theo",
        "Dona Biscoito"
    };

    private string[] falas = {
        "Oi, Dona Biscoito! O que aconteceu aqui?",
        "Ah, Theo... Hoje nada está dando certo. Passei a manhã fazendo biscoitos, mas eles perderam todas as cores...",
        "Puxa... Você parece diferente hoje. Posso ajudar?",
        "Eu adoraria, querido... Mas agora nem tenho vontade de fazer meus biscoitos favoritos..."
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

        if (emocao == "Tristeza")
        {
            textoFeedback.text =
                "Muito bem! Dona Biscoito parece estar triste. " +
                "Ela está desanimada porque as coisas " +
                "não saíram como esperava.";
        }
        else
        {
            textoFeedback.text =
                "Boa tentativa! Você escolheu " + emocao +
                ". Mas Dona Biscoito parece estar triste. " +
                "Percebeu que ela está desanimada " +
                "e sem vontade de fazer biscoitos?";
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

        nomePersonagem.text = "Dona Biscoito";

        textoDialogo.text =
            "Theo, você me ajudou a lembrar " +
            "de tantas coisas importantes! " +
            "Adoro cozinhar, estar com meus netinhos " +
            "e ouvir minhas músicas favoritas. " +
            "Mesmo quando algo dá errado, " +
            "posso encontrar maneiras de me sentir melhor!";
    }

}
