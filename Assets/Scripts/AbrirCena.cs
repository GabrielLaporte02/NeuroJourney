
using UnityEngine;
using UnityEngine.SceneManagement;

public class AbrirCena : MonoBehaviour
{
    [SerializeField] private string nomeDaCena;

    private void OnMouseDown()
    {
        SceneManager.LoadScene(nomeDaCena);
    }
}
