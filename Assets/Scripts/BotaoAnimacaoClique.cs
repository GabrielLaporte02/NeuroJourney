using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class BotaoAnimacaoClique : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, ICancelHandler
{
    [Header("Animação")]
    [SerializeField, Range(0.01f, 1f)] private float _escalaPressionado = 0.92f;
    [SerializeField, Min(0.001f)] private float _duracao = 0.08f;

    private Transform _transform;
    private UnityEngine.UI.Button _botao;
    private Vector3 _escalaOriginal;
    private Vector3 _escalaInicio;
    private Vector3 _escalaDestino;
    private float _tempo;
    private bool _animando;
    private bool _pressionado;
    private int _ponteiroId;

    private void Awake()
    {
        _transform = transform;
        _botao = GetComponent<UnityEngine.UI.Button>();
        _escalaOriginal = _transform.localScale;
    }

    private void Update()
    {
        if (_pressionado && _botao != null &&
            (!_botao.isActiveAndEnabled || !_botao.IsInteractable()))
            CancelarClique();

        if (!_animando)
            return;

        _tempo += Time.unscaledDeltaTime;
        float progresso = Mathf.Clamp01(_tempo / Mathf.Max(0.001f, _duracao));
        _transform.localScale = Vector3.Lerp(
            _escalaInicio, _escalaDestino, Mathf.SmoothStep(0f, 1f, progresso));

        if (progresso >= 1f)
        {
            _transform.localScale = _escalaDestino;
            _animando = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isActiveAndEnabled || _pressionado ||
            eventData.button != PointerEventData.InputButton.Left)
            return;

        if (_botao != null && (!_botao.isActiveAndEnabled || !_botao.IsInteractable()))
            return;

        _pressionado = true;
        _ponteiroId = eventData.pointerId;
        AnimarPara(_escalaOriginal * _escalaPressionado);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_pressionado && eventData.pointerId == _ponteiroId)
            CancelarClique();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_pressionado && eventData.pointerId == _ponteiroId)
            CancelarClique();
    }

    public void OnCancel(BaseEventData eventData)
    {
        CancelarClique();
    }

    public void CancelarClique()
    {
        if (!isActiveAndEnabled || !_pressionado)
            return;

        _pressionado = false;
        AnimarPara(_escalaOriginal);
    }

    private void AnimarPara(Vector3 destino)
    {
        // Uma nova transição sempre começa na escala atual, sem saltos.
        _escalaInicio = _transform.localScale;
        _escalaDestino = destino;
        _tempo = 0f;
        _animando = true;
    }

    private void OnApplicationFocus(bool temFoco)
    {
        if (!temFoco)
            CancelarClique();
    }

    private void OnApplicationPause(bool pausado)
    {
        if (pausado)
            CancelarClique();
    }

    private void OnDisable()
    {
        _pressionado = false;
        _animando = false;
        _tempo = 0f;

        // Um objeto desativado não executa Update: restaura imediatamente.
        if (_transform != null)
            _transform.localScale = _escalaOriginal;
    }
}
