
using UnityEngine;
using UnityEngine.UI;

public class CartaMemoria : MonoBehaviour
{
    public Image imagemFrente;
    public Image imagemVerso;

    private int id;

    private JogoMemoria jogoPadaria;
    private JogoMemoriaCorreios jogoCorreios;
    private JogoMemoriaParque jogoParque;
    private JogoMemoriaPlaza jogoPlaza;

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
        jogoParque = null;
        jogoPlaza = null;

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
        jogoParque = null;
        jogoPlaza = null;

        imagemFrente.sprite = imagem;
        encontrada = false;
        Esconder();
    }

    // Configuração para o parque da Luna
    public void Configurar(
        int novoId,
        Sprite imagem,
        JogoMemoriaParque gerenciador)
    {
        id = novoId;
        jogoParque = gerenciador;
        jogoPadaria = null;
        jogoCorreios = null;
        jogoPlaza = null;

        imagemFrente.sprite = imagem;
        encontrada = false;
        Esconder();
    }

    // Configuração para a Plaza do Zeca
    public void Configurar(
        int novoId,
        Sprite imagem,
        JogoMemoriaPlaza gerenciador)
    {
        id = novoId;
        jogoPlaza = gerenciador;
        jogoPadaria = null;
        jogoCorreios = null;
        jogoParque = null;

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
        else if (jogoParque != null)
        {
            jogoParque.SelecionarCarta(this);
        }
        else if (jogoPlaza != null)
        {
            jogoPlaza.SelecionarCarta(this);
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
