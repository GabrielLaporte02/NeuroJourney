
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
        NavegacaoJogo.CarregarCena("Mapa");
    }
}
