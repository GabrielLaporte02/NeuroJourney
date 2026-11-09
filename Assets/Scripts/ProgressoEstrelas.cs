
using UnityEngine;
using UnityEngine.UI;

public class ProgressoEstrelas : MonoBehaviour
{
    public Image[] estrelas;

    public void Atualizar(int paresEncontrados)
    {
        for (int i = 0; i < estrelas.Length; i++)
        {
            Color cor = estrelas[i].color;
            cor.a = i < paresEncontrados ? 1f : 80f / 255f;
            estrelas[i].color = cor;
        }
    }

    void Start()
    {
        Atualizar(0);
    }
}
