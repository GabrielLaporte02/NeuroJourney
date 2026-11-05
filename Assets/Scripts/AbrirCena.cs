
using UnityEngine;
using UnityEngine.SceneManagement;

public class AbrirCena : MonoBehaviour
{
    [SerializeField] private string nomeDaCena;

    private void OnMouseDown()
    {
        Abrir();
    }

    private void Update()
    {
        // Detecta o toque no iPhone
        if (Input.touchCount > 0 &&
            Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 posicaoToque = Input.GetTouch(0).position;

            Vector3 posicaoMundo =
                Camera.main.ScreenToWorldPoint(posicaoToque);

            RaycastHit2D hit =
                Physics2D.Raycast(posicaoMundo, Vector2.zero);

            if (hit.collider != null &&
                hit.collider.gameObject == gameObject)
            {
                Abrir();
            }
        }
    }

    private void Abrir()
    {
        SceneManager.LoadScene(nomeDaCena);
    }
}
