
using UnityEngine;

public class EstadoConstrucoes : MonoBehaviour
{
    [Header("Padaria")]
    public GameObject padariaA;
    public GameObject padariaD;

    [Header("Correios")]
    public GameObject correiosA;
    public GameObject correiosD;

    [Header("Parque")]
    public GameObject parqueA;
    public GameObject parqueD;

    [Header("Plaza")]
    public GameObject plazaA;
    public GameObject plazaD;

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

        // PARQUE
        bool parqueConcluido =
            PlayerPrefs.GetInt("ParqueConcluido", 0) == 1;

        parqueA.SetActive(!parqueConcluido);
        parqueD.SetActive(parqueConcluido);

        // PLAZA
        bool plazaConcluida =
            PlayerPrefs.GetInt("PlazaConcluida", 0) == 1;

        plazaA.SetActive(!plazaConcluida);
        plazaD.SetActive(plazaConcluida);
    }
}
