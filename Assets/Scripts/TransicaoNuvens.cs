
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransicaoNuvens : MonoBehaviour
{
    public static TransicaoNuvens instancia;

    [Header("Elementos")]
    public RectTransform nuvensEsquerda;
    public RectTransform nuvensDireita;
    public Image fundoNuvens;

    [Header("Animação")]
    public float duracao = 1f;

    [Range(0f, 1f)]
    public float sobreposicao = 0.25f;

    [Header("Botão Voltar")]
    public float duracaoFadeBotao = 1.2f;

    private bool emTransicao = false;

    private RectTransform canvasRect;
    private Vector2 esquerdaFora;
    private Vector2 direitaFora;
    private Vector2 esquerdaCentro;
    private Vector2 direitaCentro;

    void Awake()
    {
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        instancia = this;
        DontDestroyOnLoad(gameObject);

        canvasRect = nuvensEsquerda.parent as RectTransform;

        // Impede que o CanvasGroup antigo bloqueie cliques.
        CanvasGroup grupoNuvens =
            GetComponentInChildren<CanvasGroup>();

        if (grupoNuvens != null)
        {
            grupoNuvens.alpha = 1f;
            grupoNuvens.blocksRaycasts = false;
            grupoNuvens.interactable = false;
        }

        fundoNuvens.raycastTarget = false;

        CalcularPosicoes();

        nuvensEsquerda.anchoredPosition = esquerdaFora;
        nuvensDireita.anchoredPosition = direitaFora;

        DefinirAlphaFundo(0f);
    }

    void CalcularPosicoes()
    {
        float larguraTela = canvasRect.rect.width;

        float larguraEsquerda =
            nuvensEsquerda.rect.width * nuvensEsquerda.localScale.x;

        float larguraDireita =
            nuvensDireita.rect.width * nuvensDireita.localScale.x;

        nuvensEsquerda.anchorMin = new Vector2(0.5f, 0.5f);
        nuvensEsquerda.anchorMax = new Vector2(0.5f, 0.5f);

        nuvensDireita.anchorMin = new Vector2(0.5f, 0.5f);
        nuvensDireita.anchorMax = new Vector2(0.5f, 0.5f);

        nuvensEsquerda.pivot = new Vector2(0.5f, 0.5f);
        nuvensDireita.pivot = new Vector2(0.5f, 0.5f);

        float avancoEsquerda = larguraEsquerda * sobreposicao;
        float avancoDireita = larguraDireita * sobreposicao;

        esquerdaCentro = new Vector2(
            -larguraEsquerda / 2f + avancoEsquerda, 0f
        );

        direitaCentro = new Vector2(
            larguraDireita / 2f - avancoDireita, 0f
        );

        esquerdaFora = new Vector2(
            -larguraTela / 2f - larguraEsquerda / 2f - 10f, 0f
        );

        direitaFora = new Vector2(
            larguraTela / 2f + larguraDireita / 2f + 10f, 0f
        );
    }

    public void MudarCena(string nomeCena)
    {
        if (!emTransicao)
            StartCoroutine(AnimarTransicao(nomeCena));
    }

    IEnumerator AnimarTransicao(string nomeCena)
    {
        emTransicao = true;

        CalcularPosicoes();

        nuvensEsquerda.anchoredPosition = esquerdaFora;
        nuvensDireita.anchoredPosition = direitaFora;

        // Nuvens entram.
        yield return MoverNuvens(
            esquerdaFora, esquerdaCentro,
            direitaFora, direitaCentro,
            true
        );

        DefinirAlphaFundo(1f);

        yield return SceneManager.LoadSceneAsync(nomeCena);

        CanvasGroup grupoBotao = null;

        if (nomeCena == "Mapa")
        {
            GameObject botaoVoltar =
                GameObject.Find("BotaoVoltar");

            if (botaoVoltar != null)
            {
                grupoBotao =
                    botaoVoltar.GetComponent<CanvasGroup>();

                if (grupoBotao == null)
                    grupoBotao =
                        botaoVoltar.AddComponent<CanvasGroup>();

                // Esconde sem desativar o botão.
                grupoBotao.alpha = 0f;
                grupoBotao.interactable = false;
                grupoBotao.blocksRaycasts = false;
            }
        }

        yield return new WaitForSecondsRealtime(0.25f);

        // Nuvens saem.
        yield return MoverNuvens(
            esquerdaCentro, esquerdaFora,
            direitaCentro, direitaFora,
            false
        );

        DefinirAlphaFundo(0f);

        // Botão aparece suavemente.
        if (grupoBotao != null)
        {
            grupoBotao.alpha = 1f;
            grupoBotao.interactable = true;
            grupoBotao.blocksRaycasts = true;
        }

        emTransicao = false;
    }

    IEnumerator FadeBotao(CanvasGroup grupoBotao)
    {
        float tempo = 0f;

        while (tempo < duracaoFadeBotao)
        {
            tempo += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(
                tempo / Mathf.Max(0.01f, duracaoFadeBotao)
            );

            // Curva suave de entrada e saída.
            grupoBotao.alpha = t;

            yield return null;
        }

        grupoBotao.alpha = 1f;
        grupoBotao.interactable = true;
        grupoBotao.blocksRaycasts = true;
    }

    IEnumerator MoverNuvens(
        Vector2 inicioEsquerda,
        Vector2 fimEsquerda,
        Vector2 inicioDireita,
        Vector2 fimDireita,
        bool entrando)
    {
        float tempo = 0f;

        while (tempo < duracao)
        {
            tempo += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(tempo / duracao);
            t = Mathf.SmoothStep(0f, 1f, t);

            nuvensEsquerda.anchoredPosition =
                Vector2.Lerp(inicioEsquerda, fimEsquerda, t);

            nuvensDireita.anchoredPosition =
                Vector2.Lerp(inicioDireita, fimDireita, t);

            // float alpha = entrando
            //     ? Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(0.5f, 1f, t))
            //     : 1f - Mathf.SmoothStep(0f, 1f, t);

            float alpha = entrando
    ? Mathf.InverseLerp(0.5f, 1f, t)
    : 1f - t;

            DefinirAlphaFundo(alpha);

            yield return null;
        }

        nuvensEsquerda.anchoredPosition = fimEsquerda;
        nuvensDireita.anchoredPosition = fimDireita;
    }

    void DefinirAlphaFundo(float alpha)
    {
        Color cor = fundoNuvens.color;
        cor.a = alpha;
        fundoNuvens.color = cor;
    }
}
