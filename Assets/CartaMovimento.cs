using System.Collections;
using UnityEngine;

public class CartaMovimento : MonoBehaviour
{
    public Sprite frenteCarta;
    public Sprite versoCarta;
    
    private SpriteRenderer renderizador;
    private bool estaVirada = false;

    void Start()
    {
        renderizador = GetComponent<SpriteRenderer>();
        renderizador.sprite = versoCarta; // Inicia mostrando as costas da carta
    }

    void OnMouseDown()
    {
        // Só anima se a carta ainda não estiver virada
        if (!estaVirada)
        {
            StartCoroutine(AnimacaoGiro());
        }
    }

    IEnumerator AnimacaoGiro()
    {
        estaVirada = true;
        Vector3 escalaOriginal = transform.localScale;
        float tempoGiro = 0.15f; // Quão rápido é o movimento

        // Parte 1: Encolhe a largura (X) até 0 e aumenta a altura (Y) para dar o efeito de pulo
        for (float t = 0; t < tempoGiro; t += Time.deltaTime)
        {
            float progresso = t / tempoGiro;
            float novoX = Mathf.Lerp(escalaOriginal.x, 0f, progresso);
            float novoY = Mathf.Lerp(escalaOriginal.y, escalaOriginal.y * 1.15f, progresso);
            
            transform.localScale = new Vector3(novoX, novoY, escalaOriginal.z);
            yield return null;
        }

        // Troca a imagem no exato momento em que a carta está "invisível" de lado
        renderizador.sprite = frenteCarta;

        // Parte 2: Cresce a largura (X) de volta ao normal e reduz a altura (Y) para encaixar
        for (float t = 0; t < tempoGiro; t += Time.deltaTime)
        {
            float progresso = t / tempoGiro;
            float novoX = Mathf.Lerp(0f, escalaOriginal.x, progresso);
            float novoY = Mathf.Lerp(escalaOriginal.y * 1.15f, escalaOriginal.y, progresso);
            
            transform.localScale = new Vector3(novoX, novoY, escalaOriginal.z);
            yield return null;
        }

        // Garante que termine exatamente com o tamanho perfeito
        transform.localScale = escalaOriginal;
    }
}