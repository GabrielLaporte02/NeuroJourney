
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class AbrirCena : MonoBehaviour
{
    [SerializeField] private string nomeDaCena;

    
    private bool ClicouNaInterface(Vector2 posicao)
    {
        if (EventSystem.current == null)
            return false;

        PointerEventData dados = new PointerEventData(EventSystem.current);
        dados.position = posicao;

        List<RaycastResult> resultados = new List<RaycastResult>();
        EventSystem.current.RaycastAll(dados, resultados);

        foreach (RaycastResult resultado in resultados)
        {
            if (resultado.gameObject.GetComponentInParent<UnityEngine.UI.Button>() != null)
            {
                return true;
            }
        }

        return false;
    }


    private void OnMouseDown()
    {
        if (Input.touchCount > 0)
            return;

        if (ClicouNaInterface(Input.mousePosition))
            return;

        Abrir();
    }

    private void Update()
    {
        if (Input.touchCount == 0)
            return;

        Touch toque = Input.GetTouch(0);

        if (toque.phase != TouchPhase.Began)
            return;

        if (ClicouNaInterface(toque.position))
            return;

        Vector3 posicaoMundo =
            Camera.main.ScreenToWorldPoint(toque.position);

        RaycastHit2D hit =
            Physics2D.Raycast(posicaoMundo, Vector2.zero);

        if (hit.collider != null &&
            hit.collider.gameObject == gameObject)
        {
            Abrir();
        }
    }

    private void Abrir()
    {
        SceneManager.LoadScene(nomeDaCena);
    }
}
