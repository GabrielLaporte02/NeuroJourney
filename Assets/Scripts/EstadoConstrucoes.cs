using UnityEngine;

public class EstadoConstrucoes : MonoBehaviour
{
    [Header("Padaria")]
    public GameObject padariaA;
    public GameObject padariaD;

    void Start()
    {
        bool padariaConcluida =
            PlayerPrefs.GetInt("PadariaConcluida", 0) == 1;

        padariaA.SetActive(!padariaConcluida);
        padariaD.SetActive(padariaConcluida);
    }
}
