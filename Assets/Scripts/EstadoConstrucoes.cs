
using UnityEngine;

public class EstadoConstrucoes : MonoBehaviour
{
    [Header("Padaria")]
    public GameObject padariaA;
    public GameObject padariaD;

    [Header("Correios")]
    public GameObject correiosA;
    public GameObject correiosD;

    void Start()
    {
        // PADARIA
        bool padariaConcluida =
            PlayerPrefs.GetInt("PadariaConcluida", 0) == 1;

        padariaA.SetActive(!padariaConcluida);
        padariaD.SetActive(padariaConcluida);

        // CORREIOS
        bool correiosConcluido =
            PlayerPrefs.GetInt("CorreiosConcluido", 0) == 1;

        correiosA.SetActive(!correiosConcluido);
        correiosD.SetActive(correiosConcluido);
    }
}
