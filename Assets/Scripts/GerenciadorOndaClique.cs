using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class GerenciadorOndaClique : MonoBehaviour
{
    [Header("Ondas")]
    [SerializeField, Range(0.35f, 0.45f)] private float _duracao = 0.4f;
    [SerializeField, Min(1)] private int _tamanhoPool = 16;
    [SerializeField, Min(1f)] private float _diametroInicial = 14f;
    [SerializeField, Min(1f)] private float _diametroFinal = 120f;
    [SerializeField] private Color _cor = new Color(1f, 0.9f, 0.5f, 0.85f);

    private static GerenciadorOndaClique _instancia;
    private UnityEngine.InputSystem.InputAction _clique;
    private RectTransform _canvasRect;
    private EfeitoOndaClique[] _pool;
    private int _proximaOnda;
    private Vector2 _ultimaPosicaoEntrada;
    private double _ultimoTempoEntrada = -1d;

    public int CapacidadePool => _pool == null ? 0 : _pool.Length;
    public int OndasAtivas
    {
        get
        {
            int count = 0;
            if (_pool != null)
                for (int i = 0; i < _pool.Length; i++)
                    if (_pool[i].EstaAtiva)
                        count++;
            return count;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ReiniciarEstado()
    {
        _instancia = null;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void CriarAutomaticamente()
    {
        if (_instancia == null)
            new GameObject("GerenciadorOndaClique").AddComponent<GerenciadorOndaClique>();
    }

    private void Awake()
    {
        if (_instancia != null && _instancia != this)
        {
            Destroy(this);
            return;
        }
        _instancia = this;
        DontDestroyOnLoad(gameObject);

        // O próprio objeto persistente é o Canvas raiz de overlay.
        var canvas = GetComponent<Canvas>();
        if (canvas == null)
            canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = short.MaxValue;
        _canvasRect = (RectTransform)transform;

        // Não existe GraphicRaycaster neste Canvas. As ondas também ignoram raycasts.
        var grupo = GetComponent<CanvasGroup>();
        if (grupo == null)
            grupo = gameObject.AddComponent<CanvasGroup>();
        grupo.interactable = false;
        grupo.blocksRaycasts = false;

        _pool = new EfeitoOndaClique[Mathf.Max(1, _tamanhoPool)];
        for (int i = 0; i < _pool.Length; i++)
        {
            var objeto = new GameObject("OndaClique_" + i, typeof(RectTransform), typeof(CanvasRenderer));
            objeto.transform.SetParent(_canvasRect, false);
            var rect = (RectTransform)objeto.transform;
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            _pool[i] = objeto.AddComponent<EfeitoOndaClique>();
            objeto.SetActive(false);
        }

        _clique = new UnityEngine.InputSystem.InputAction("FeedbackOndaClique",
            UnityEngine.InputSystem.InputActionType.PassThrough);
        UnityEngine.InputSystem.InputActionSetupExtensions.AddBinding(_clique, "<Mouse>/leftButton");
        UnityEngine.InputSystem.InputActionSetupExtensions.AddBinding(_clique, "<Touchscreen>/touch*/press");
        _clique.performed += AoPressionar;
    }

    private void OnEnable()
    {
        if (_instancia == this && _clique != null)
            _clique.Enable();
    }

    private void Update()
    {
        if (_pool == null)
            return;
        float deltaTime = Time.unscaledDeltaTime;
        for (int i = 0; i < _pool.Length; i++)
            if (_pool[i].EstaAtiva)
                _pool[i].Avancar(deltaTime);
    }

    private void AoPressionar(UnityEngine.InputSystem.InputAction.CallbackContext contexto)
    {
        if (contexto.ReadValue<float>() < 0.5f)
            return;

        Vector2 posicao;
        var toque = contexto.control.parent as UnityEngine.InputSystem.Controls.TouchControl;
        if (toque != null)
            posicao = toque.position.ReadValue();
        else
        {
            var mouse = contexto.control.device as UnityEngine.InputSystem.Mouse;
            if (mouse == null)
                return;
            posicao = mouse.position.ReadValue();
        }

        // Evita duplicar a onda quando um mesmo toque também produz entrada de mouse.
        double tempo = contexto.time;
        if (System.Math.Abs(tempo - _ultimoTempoEntrada) < 0.001d &&
            (posicao - _ultimaPosicaoEntrada).sqrMagnitude < 1f)
            return;
        _ultimoTempoEntrada = tempo;
        _ultimaPosicaoEntrada = posicao;
        MostrarOnda(posicao);
    }

    public void MostrarOnda(Vector2 posicaoTela)
    {
        if (!isActiveAndEnabled || _pool == null || _canvasRect == null)
            return;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasRect, posicaoTela, null, out Vector2 posicaoLocal))
            return;

        int indice = -1;
        for (int i = 0; i < _pool.Length; i++)
        {
            int candidato = (_proximaOnda + i) % _pool.Length;
            if (!_pool[candidato].EstaAtiva)
            {
                indice = candidato;
                break;
            }
        }
        // Pool cheio: recicla a onda mais antiga sem criar novos objetos.
        if (indice < 0)
            indice = _proximaOnda;
        _proximaOnda = (indice + 1) % _pool.Length;
        _pool[indice].Mostrar(posicaoLocal, _duracao, _diametroInicial, _diametroFinal, _cor);
    }

    private void OnDisable()
    {
        if (_clique != null)
            _clique.Disable();
        if (_pool != null)
            for (int i = 0; i < _pool.Length; i++)
                _pool[i].gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (_clique != null)
        {
            _clique.performed -= AoPressionar;
            _clique.Dispose();
        }
        if (_instancia == this)
            _instancia = null;
    }
}
