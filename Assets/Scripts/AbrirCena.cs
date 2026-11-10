
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class AbrirCena : MonoBehaviour
{
    [SerializeField] private string nomeDaCena;

    private void OnMouseDown()
    {
        // Ignora cliques sobre a interface
        if (EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // Evita duplicar o toque em dispositivos móveis
        if (Input.touchCount > 0)
        {
            return;
        }

        Abrir();
    }

    private void Update()
    {
        // Detecta o toque no iPhone
        if (Input.touchCount > 0 &&
            Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Touch toque = Input.GetTouch(0);

            // Ignora toques sobre botões da interface
            if (EventSystem.current != null &&
                EventSystem.current.IsPointerOverGameObject(
                    toque.fingerId))
            {
                return;
            }

            Vector2 posicaoToque = toque.position;

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
    // if (GerenciadorTransicao.instancia != null)
    // {
    //     GerenciadorTransicao.instancia.MudarCena(nomeDaCena);
    // }
    // if (TransicaoNuvens.instancia != null)
    // {
    //     TransicaoNuvens.instancia.MudarCena(nomeDaCena);
    // }
    // else
    // {
        SceneManager.LoadScene(nomeDaCena);
    //}
}
}
