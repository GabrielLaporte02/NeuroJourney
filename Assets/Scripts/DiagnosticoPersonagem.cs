
using UnityEngine;

public class DiagnosticoPersonagens : MonoBehaviour
{
    public GameObject theo;
    public GameObject donaBiscoito;

    void Start()
    {
        InvokeRepeating(nameof(Verificar), 1f, 3f);
    }

    void Verificar()
    {
        Checar("Theo", theo);
        Checar("Dona Biscoito", donaBiscoito);
    }

    void Checar(string nome, GameObject personagem)
    {
        if (personagem == null)
        {
            Debug.LogError(nome + " não está atribuído!");
            return;
        }

        SpriteRenderer sr = personagem.GetComponent<SpriteRenderer>();

        if (sr == null)
        {
            Debug.LogError(nome + " não tem SpriteRenderer!");
            return;
        }

        Debug.Log(
            nome +
            " | Ativo: " + personagem.activeInHierarchy +
            " | Renderer: " + sr.enabled +
            " | Visível: " + sr.isVisible +
            " | Sprite: " + (sr.sprite != null) +
            " | Posição: " + personagem.transform.position +
            " | Escala: " + personagem.transform.lossyScale +
            " | Alpha: " + sr.color.a +
            " | Sorting Layer: " + sr.sortingLayerName +
            " | Order: " + sr.sortingOrder +
            " | Material: " + (sr.sharedMaterial != null
                ? sr.sharedMaterial.name : "NULL") +
            " | ForceRenderingOff: " + sr.forceRenderingOff
        );
    }
}
