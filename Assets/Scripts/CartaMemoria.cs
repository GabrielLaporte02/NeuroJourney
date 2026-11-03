using UnityEngine;
using UnityEngine.UI;

public class CartaMemoria : MonoBehaviour
{
    public Image imagemFrente;
    public Image imagemVerso;

    private int id;
    private JogoMemoria jogo;
    private bool encontrada = false;

    public void Configurar(
        int novoId,
        Sprite imagem,
        JogoMemoria gerenciador)
    {
        id = novoId;
        jogo = gerenciador;

        imagemFrente.sprite = imagem;
        encontrada = false;

        Esconder();
    }

    public void Clicar()
    {
        if (!encontrada)
        {
            jogo.SelecionarCarta(this);
        }
    }

    public int ObterId()
    {
        return id;
    }

    public void Revelar()
    {
        imagemFrente.gameObject.SetActive(true);
        imagemVerso.enabled = false;
             // imagemVerso.gameObject.SetActive(false);
    }

    public void Esconder()
    {
        if (encontrada) return;

        imagemFrente.gameObject.SetActive(false);
        imagemVerso.enabled = true;
        //imagemVerso.gameObject.SetActive(true);
    }

    public void MarcarEncontrada()
    {
        encontrada = true;
        Revelar();
    }
}
