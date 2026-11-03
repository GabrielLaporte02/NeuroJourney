
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class JogoMemoria : MonoBehaviour
{
    [Header("Cartas")]
    public GameObject prefabCarta;
    public Transform gradeCartas;
    public Sprite[] imagensPares;

    [Header("Interface")]
    public TMP_Text textoProgresso;
    public GameObject painelMensagemPar;
    public TMP_Text textoMensagemPar;

    [Header("Final da fase")]
    public DialogoPadaria dialogoPadaria;

    private CartaMemoria primeiraCarta;
    private CartaMemoria segundaCarta;

    private int paresEncontrados = 0;
    private bool bloqueado = false;

    private string[] mensagens = {
        "Ah, meus ingredientes! Adoro misturar tudo e inventar receitas novas. Isso me deixa tão animada!",

        "Meus netinhos! Quando eles vêm me visitar, minha padaria fica cheia de alegria!",

        "Essa é minha música favorita! Quando estou desanimada, ouvir música me ajuda a relaxar.",

        "Ah, meus biscoitos queimados... Fiquei tão chateada! Passei muito tempo preparando a receita.",

        "Essa era minha tigela favorita... Fiquei triste quando ela quebrou, porque era muito especial para mim."
    };

    public void IniciarJogo()
    {
        StopAllCoroutines();

        primeiraCarta = null;
        segundaCarta = null;
        paresEncontrados = 0;
        bloqueado = false;

        painelMensagemPar.SetActive(false);

        // Apaga cartas de uma partida anterior
        foreach (Transform filho in gradeCartas)
        {
            Destroy(filho.gameObject);
        }

        // Dois exemplares de cada par
        List<int> ids = new List<int>();

        for (int i = 0; i < 5; i++)
        {
            ids.Add(i);
            ids.Add(i);
        }

        // Embaralhamento Fisher-Yates
        for (int i = ids.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);

            int temp = ids[i];
            ids[i] = ids[j];
            ids[j] = temp;
        }

        // Cria as dez cartas
        foreach (int id in ids)
        {
            GameObject objeto = Instantiate(
                prefabCarta,
                gradeCartas
            );

            CartaMemoria carta =
                objeto.GetComponent<CartaMemoria>();

            carta.Configurar(
                id,
                imagensPares[id],
                this
            );
        }

        AtualizarProgresso();
    }

    public void SelecionarCarta(CartaMemoria carta)
    {
        if (bloqueado) return;
        if (carta == primeiraCarta) return;

        carta.Revelar();

        if (primeiraCarta == null)
        {
            primeiraCarta = carta;
            return;
        }

        segundaCarta = carta;
        bloqueado = true;

        StartCoroutine(VerificarPar());
    }

    IEnumerator VerificarPar()
    {
        // Dá tempo para observar as imagens
        yield return new WaitForSeconds(0.8f);

        if (primeiraCarta.ObterId() ==
            segundaCarta.ObterId())
        {
            primeiraCarta.MarcarEncontrada();
            segundaCarta.MarcarEncontrada();

            paresEncontrados++;
            AtualizarProgresso();

            int id = primeiraCarta.ObterId();

            textoMensagemPar.text = mensagens[id];
            painelMensagemPar.SetActive(true);

            // Espera o jogador clicar em Continuar
        }
        else
        {
            primeiraCarta.Esconder();
            segundaCarta.Esconder();

            LimparSelecao();
        }
    }

    public void ContinuarDepoisDoPar()
    {
        painelMensagemPar.SetActive(false);

        LimparSelecao();

        if (paresEncontrados == 5)
        {
            FinalizarJogo();
        }
    }

    void LimparSelecao()
    {
        primeiraCarta = null;
        segundaCarta = null;
        bloqueado = false;
    }

    void AtualizarProgresso()
    {
        textoProgresso.text =
            "Pares encontrados: " +
            paresEncontrados + "/5";
    }

    void FinalizarJogo()
    {
        SceneManager.LoadScene("PadariaDepois");
    }

    // void Start()
    // {
    //     IniciarJogo();
    // }
}
