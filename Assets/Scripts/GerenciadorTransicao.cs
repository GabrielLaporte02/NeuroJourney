
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GerenciadorTransicao : MonoBehaviour
{
    public static GerenciadorTransicao instancia;

    [Header("Transição")]
    public Image imagemFade;
    public float duracao = 0.4f;

    private bool emTransicao = false;

    void Awake()
    {
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        instancia = this;
        DontDestroyOnLoad(gameObject);

        // Começa com a tela escura para clarear ao entrar.
        DefinirAlpha(1f);
    }

    IEnumerator Start()
    {
        yield return Fade(1f, 0f);
    }

    public void MudarCena(string nomeCena)
    {
        if (!emTransicao)
            StartCoroutine(Transicao(nomeCena));
    }

    IEnumerator Transicao(string nomeCena)
    {
        emTransicao = true;

        // Escurece a cena atual.
        yield return Fade(0f, 1f);

        // Carrega a próxima cena.
        yield return SceneManager.LoadSceneAsync(nomeCena);

        // Clareia a nova cena.
        yield return Fade(1f, 0f);

        emTransicao = false;
    }

    IEnumerator Fade(float inicio, float fim)
    {
        float tempo = 0f;

        while (tempo < duracao)
        {
            tempo += Time.unscaledDeltaTime;

            float progresso = Mathf.Clamp01(tempo / duracao);
            progresso = Mathf.SmoothStep(0f, 1f, progresso);

            DefinirAlpha(Mathf.Lerp(inicio, fim, progresso));

            yield return null;
        }

        DefinirAlpha(fim);
    }

    void DefinirAlpha(float alpha)
    {
        Color cor = imagemFade.color;
        cor.a = alpha;
        imagemFade.color = cor;

        // Impede cliques durante a transição.
        imagemFade.raycastTarget = alpha > 0.01f;
    }
}
