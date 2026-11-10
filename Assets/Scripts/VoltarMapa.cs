
// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class VoltarMapa : MonoBehaviour
// {
//     public void Voltar()
//     {
//         SceneManager.LoadScene("Mapa");
//     }
// }


using UnityEngine;
using UnityEngine.SceneManagement;

public class VoltarMapa : MonoBehaviour
{
    public void Voltar()
    {
        if (TransicaoNuvens.instancia != null)
        {
            TransicaoNuvens.instancia.MudarCena("Mapa");
        }
        else
        {
            SceneManager.LoadScene("Mapa");
        }
    }
}
