
using UnityEngine;
using UnityEngine.UI;

public class CartaMemoria : MonoBehaviour
{
    public Image imagemFrente;
    public Image imagemVerso;

    private int id;
    private JogoMemoria jogoPadaria;
    private JogoMemoriaCorreios jogoCorreios;
    private bool encontrada = false;

    // Configuração para a padaria
    public void Configurar(
        int novoId,
        Sprite imagem,
        JogoMemoria gerenciador)
    {
        id = novoId;
        jogoPadaria = gerenciador;
        jogoCorreios = null;
        imagemFrente.sprite = imagem;
        encontrada = false;
        Esconder();
    }

    // Configuração para os correios
    public void Configurar(
        int novoId,
        Sprite imagem,
        JogoMemoriaCorreios gerenciador)
    {
        id = novoId;
        jogoCorreios = gerenciador;
        jogoPadaria = null;
        imagemFrente.sprite = imagem;
        encontrada = false;
        Esconder();
    }

    public void Clicar()
    {
        if (encontrada) return;

        if (jogoPadaria != null)
        {
            jogoPadaria.SelecionarCarta(this);
        }
        else if (jogoCorreios != null)
        {
            jogoCorreios.SelecionarCarta(this);
        }
        else
        {
            Debug.LogError(
                "Carta sem gerenciador de memória!"
            );
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
    }

    public void Esconder()
    {
        if (encontrada) return;

        imagemFrente.gameObject.SetActive(false);
        imagemVerso.enabled = true;
    }

    public void MarcarEncontrada()
    {
        encontrada = true;
        Revelar();
    }
}
