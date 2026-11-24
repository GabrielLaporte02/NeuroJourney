using UnityEngine;

[DisallowMultipleComponent]
public class EfeitoOndaClique : UnityEngine.UI.MaskableGraphic
{
    [SerializeField, Range(16, 128)] private int _segmentos = 64;
    [SerializeField, Min(0.1f)] private float _espessura = 2.5f;

    private RectTransform _rect;
    private Color _corInicial;
    private float _duracao;
    private float _tempo;
    private float _diametroInicial;
    private float _diametroFinal;

    public bool EstaAtiva { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        _rect = rectTransform;
        raycastTarget = false;
        maskable = false;
    }

    public void Mostrar(Vector2 posicao, float duracao, float diametroInicial,
        float diametroFinal, Color cor)
    {
        _duracao = Mathf.Max(0.01f, duracao);
        _diametroInicial = diametroInicial;
        _diametroFinal = diametroFinal;
        _corInicial = cor;
        _tempo = 0f;
        EstaAtiva = true;
        gameObject.SetActive(true);
        _rect.anchoredPosition = posicao;
        _rect.sizeDelta = Vector2.one * diametroInicial;
        color = cor;
        raycastTarget = false;
        SetVerticesDirty();
    }

    public void Avancar(float deltaTime)
    {
        if (!EstaAtiva)
            return;

        _tempo += deltaTime;
        float progresso = Mathf.Clamp01(_tempo / _duracao);
        float suave = Mathf.SmoothStep(0f, 1f, progresso);
        _rect.sizeDelta = Vector2.one * Mathf.Lerp(_diametroInicial, _diametroFinal, suave);
        Color cor = _corInicial;
        cor.a *= 1f - progresso;
        color = cor;
        SetVerticesDirty();

        if (progresso >= 1f)
        {
            EstaAtiva = false;
            gameObject.SetActive(false);
        }
    }

    protected override void OnPopulateMesh(UnityEngine.UI.VertexHelper mesh)
    {
        mesh.Clear();
        Rect rect = rectTransform.rect;
        Vector2 centro = rect.center;
        float externo = Mathf.Min(rect.width, rect.height) * 0.5f;
        float interno = Mathf.Max(0f, externo - _espessura);
        int segmentos = Mathf.Max(16, _segmentos);
        for (int i = 0; i <= segmentos; i++)
        {
            float angulo = i * Mathf.PI * 2f / segmentos;
            Vector2 direcao = new Vector2(Mathf.Cos(angulo), Mathf.Sin(angulo));
            mesh.AddVert(centro + direcao * externo, color, Vector2.zero);
            mesh.AddVert(centro + direcao * interno, color, Vector2.zero);
            if (i == 0)
                continue;
            int atual = i * 2;
            mesh.AddTriangle(atual - 2, atual, atual - 1);
            mesh.AddTriangle(atual, atual + 1, atual - 1);
        }
    }

    protected override void OnDisable()
    {
        EstaAtiva = false;
        base.OnDisable();
    }
}
