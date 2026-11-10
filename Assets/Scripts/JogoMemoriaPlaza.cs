
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class JogoMemoriaPlaza : MonoBehaviour
{
    [Header("Cartas")]
    public GameObject prefabCarta;
    public Transform gradeCartas;
    public Sprite[] imagensPares;

    [Header("Interface")]
    public ProgressoEstrelas progressoEstrelas;
    public GameObject painelMensagemPar;
    public TMP_Text textoMensagemPar;

    [Header("Final da fase")]
    public DialogoPlazaAntes dialogoPlaza;

    private CartaMemoria primeiraCarta;
    private CartaMemoria segundaCarta;

    private int paresEncontrados = 0;
    private bool bloqueado = false;

    private string[] mensagens = {
        "Posso escolher um jogo primeiro e depois brincar nos outros! Não preciso fazer tudo ao mesmo tempo!",

        "Quando fico muito agitado, posso fazer uma pausa, beber água e descansar um pouquinho!",

        "Respirar devagar pode me ajudar a diminuir a agitação e escolher o que quero fazer!",

        "Uau, tantas luzes piscando! Às vezes fico tão agitado que não consigo prestar atenção em um jogo só!",

        "Quando todos os jogos fazem barulho ao mesmo tempo, fica difícil me concentrar e escolher onde brincar!"
    };

    public void IniciarJogo()
    {
        StopAllCoroutines();

        primeiraCarta = null;
        segundaCarta = null;
        paresEncontrados = 0;
        bloqueado = false;

        painelMensagemPar.SetActive(false);

        AtualizarProgresso();

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
        if (progressoEstrelas != null)
        {
            progressoEstrelas.Atualizar(paresEncontrados);
        }
    }

    void FinalizarJogo()
    {
        SceneManager.LoadScene("PlazaDepois");
    }
}
