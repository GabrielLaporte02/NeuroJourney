
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimacaoConquista : MonoBehaviour
{
    [Header("Interface")]
    public GameObject painelConquista;
    public RectTransform imagemFigurinha;
    public Button botaoContinuar;

    [Header("Configuração")]
    public string personagem = "DonaBiscoito";
    public float duracao = 0.6f;

    private Vector3 escalaFinal;

    void Awake()
    {
        // Guarda a escala configurada no Inspector
        escalaFinal = imagemFigurinha.localScale;
    }

    public void MostrarConquista()
    {
        SistemaFigurinhas.Desbloquear(personagem);

        painelConquista.SetActive(true);
        botaoContinuar.interactable = false;

        StartCoroutine(Animar());
    }

    IEnumerator Animar()
    {
        float tempo = 0f;

        // Começa invisível (escala zero)
        imagemFigurinha.localScale = Vector3.zero;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;

            float progresso = Mathf.Clamp01(tempo / duracao);

            float escala = Mathf.SmoothStep(
                0f, 1f, progresso
            );

            // Cresce até a escala original
            imagemFigurinha.localScale =
                escalaFinal * escala;

            yield return null;
        }

        // Garante o tamanho final correto
        imagemFigurinha.localScale = escalaFinal;

        botaoContinuar.interactable = true;
    }

    public void FecharConquista()
    {
        painelConquista.SetActive(false);
    }
}
