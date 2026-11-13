using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UnityEngine.UI.Image), typeof(UnityEngine.UI.LayoutElement))]
public class CartaAnimacao : MonoBehaviour, IPointerClickHandler
{
    [Header("Imagens da carta")]
    public Sprite spriteFrente;
    public Sprite spriteVerso;

    [Header("Animação")]
    [SerializeField, Min(0.01f)] private float _duracaoVirada = 0.4f;

    private UnityEngine.UI.Image _imagem;
    private UnityEngine.UI.LayoutElement _layout;
    private Vector2 _tamanhoOriginal;
    private bool _animando;
    private bool _mostrandoFrente;

    private void Awake()
    {
        _imagem = GetComponent<UnityEngine.UI.Image>();
        _layout = GetComponent<UnityEngine.UI.LayoutElement>();
        RectTransform rect = (RectTransform)transform;
        _tamanhoOriginal = new Vector2(
            _layout.preferredWidth > 0f ? _layout.preferredWidth : rect.rect.width,
            _layout.preferredHeight > 0f ? _layout.preferredHeight : rect.rect.height);

        // Permite encolher até zero sem um tamanho mínimo impedir a animação.
        _layout.minWidth = 0f;
        _layout.minHeight = 0f;
        _layout.flexibleWidth = 0f;
        _layout.flexibleHeight = 0f;
        DefinirTamanho(_tamanhoOriginal);
        if (spriteVerso != null)
            _imagem.sprite = spriteVerso;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            Virar();
    }

    public void Virar()
    {
        if (!isActiveAndEnabled || _animando || _mostrandoFrente || !SpritesValidos())
            return;
        _animando = true;
        StartCoroutine(AnimarVirada(true));
    }

    public void Desvirar()
    {
        if (!isActiveAndEnabled || _animando || !_mostrandoFrente || !SpritesValidos())
            return;
        _animando = true;
        StartCoroutine(AnimarVirada(false));
    }

    private bool SpritesValidos()
    {
        return spriteFrente != null && spriteVerso != null;
    }

    private IEnumerator AnimarVirada(bool mostrarFrente)
    {
        float meiaDuracao = Mathf.Max(0.01f, _duracaoVirada) * 0.5f;
        Vector2 atual = new Vector2(_layout.preferredWidth, _layout.preferredHeight);
        Vector2 fechado = new Vector2(0f, atual.y);
        Vector2 destino = mostrarFrente ? _tamanhoOriginal * 2f : _tamanhoOriginal;

        yield return AnimarTamanho(atual, fechado, meiaDuracao);
        _imagem.sprite = mostrarFrente ? spriteFrente : spriteVerso;
        _mostrandoFrente = mostrarFrente;
        yield return AnimarTamanho(fechado, destino, meiaDuracao);
        _animando = false;
    }

    private IEnumerator AnimarTamanho(Vector2 inicio, Vector2 fim, float duracao)
    {
        float tempo = 0f;
        while (tempo < duracao)
        {
            tempo += Time.unscaledDeltaTime;
            float progresso = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(tempo / duracao));
            DefinirTamanho(Vector2.Lerp(inicio, fim, progresso));
            yield return null;
        }
        DefinirTamanho(fim);
    }

    private void DefinirTamanho(Vector2 tamanho)
    {
        // Os setters notificam o sistema de layout; não é preciso forçar rebuild.
        _layout.preferredWidth = tamanho.x;
        _layout.preferredHeight = tamanho.y;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        if (_layout != null)
            DefinirTamanho(_tamanhoOriginal);
        if (_imagem != null && spriteVerso != null)
            _imagem.sprite = spriteVerso;
        _mostrandoFrente = false;
        _animando = false;
    }
}
