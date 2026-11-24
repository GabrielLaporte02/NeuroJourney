using UnityEngine;

[DisallowMultipleComponent]
public class PainelConfigProgressoConquistas : MonoBehaviour
{
    [System.Serializable]
    public class Conquista
    {
        [SerializeField] private string _personagem;
        [SerializeField] private string _titulo;
        [SerializeField, TextArea(2, 5)] private string _descricao;
        [SerializeField] private Sprite _icone;
        [SerializeField] private UnityEngine.UI.Button _botao;
        [SerializeField] private TMPro.TMP_Text _rotulo;

        public string Personagem => _personagem;
        public string Titulo => _titulo;
        public string Descricao => _descricao;
        public Sprite Icone => _icone;
        public UnityEngine.UI.Button Botao => _botao;
        public TMPro.TMP_Text Rotulo => _rotulo;
        public RectTransform Rect { get; set; }
    }

    [Header("Interface")]
    [SerializeField] private GameObject _painel;
    [SerializeField] private UnityEngine.UI.Slider _barraProgresso;
    [SerializeField] private TMPro.TMP_Text _porcentagem;
    [SerializeField] private TMPro.TMP_Text _resumoProgresso;
    [SerializeField] private TMPro.TMP_Text _quantidadeConquistas;
    [SerializeField] private GameObject _estadoVazio;

    [Header("Detalhes da conquista")]
    [SerializeField] private GameObject _detalhes;
    [SerializeField] private UnityEngine.UI.Image _imagemDetalhe;
    [SerializeField] private TMPro.TMP_Text _tituloDetalhe;
    [SerializeField] private TMPro.TMP_Text _descricaoDetalhe;

    [Header("Dados existentes do jogo")]
    [Tooltip("Mesmas chaves lidas pelo EstadoConstrucoes. Não cria nem modifica o salvamento.")]
    [SerializeField] private string[] _chavesProgresso =
    {
        "PadariaConcluida", "CorreiosConcluido", "ParqueConcluido", "PlazaConcluida"
    };
    [SerializeField] private Conquista[] _conquistas;

    [Header("Atualização e disposição")]
    [SerializeField, Min(0.1f)] private float _intervaloAtualizacao = 0.5f;
    [SerializeField, Min(1f)] private float _passoLinha = 76f;

    private UnityEngine.Events.UnityAction[] _acoes;
    private float _tempo;
    private int _assinatura = int.MinValue;
    private int _selecionada = -1;

    public int QuantidadeConquistas { get; private set; }
    public bool EstaAberto => _painel != null && _painel.activeSelf;
    public float ProgressoNormalizado => _chavesProgresso == null || _chavesProgresso.Length == 0
        ? 0f : (float)ContarLugaresConcluidos() / _chavesProgresso.Length;

    private void Awake()
    {
        int tamanho = _conquistas == null ? 0 : _conquistas.Length;
        _acoes = new UnityEngine.Events.UnityAction[tamanho];
        for (int i = 0; i < tamanho; i++)
        {
            Conquista conquista = _conquistas[i];
            if (conquista == null || conquista.Botao == null)
                continue;
            conquista.Rect = (RectTransform)conquista.Botao.transform;
            int indice = i;
            _acoes[i] = () => MostrarDetalhes(indice);
            conquista.Botao.onClick.AddListener(_acoes[i]);
        }
        Fechar();
    }

    private void Update()
    {
        if (!EstaAberto)
            return;
        _tempo += Time.unscaledDeltaTime;
        if (_tempo < _intervaloAtualizacao)
            return;
        _tempo = 0f;
        if (CalcularAssinatura() != _assinatura)
            Atualizar();
    }

    public void Alternar()
    {
        if (EstaAberto)
            Fechar();
        else
            Abrir();
    }

    public void Abrir()
    {
        if (_painel == null)
            return;
        _painel.SetActive(true);
        _selecionada = -1;
        if (_detalhes != null)
            _detalhes.SetActive(false);
        _tempo = 0f;
        Atualizar();
    }

    public void Fechar()
    {
        if (_painel != null)
            _painel.SetActive(false);
        if (_detalhes != null)
            _detalhes.SetActive(false);
        _selecionada = -1;
    }

    public void Atualizar()
    {
        int total = _chavesProgresso == null ? 0 : _chavesProgresso.Length;
        int concluidos = ContarLugaresConcluidos();
        float progresso = total == 0 ? 0f : (float)concluidos / total;
        if (_barraProgresso != null)
            _barraProgresso.SetValueWithoutNotify(progresso);
        if (_porcentagem != null)
            _porcentagem.SetText("{0}%", Mathf.RoundToInt(progresso * 100f));
        if (_resumoProgresso != null)
            _resumoProgresso.SetText("{0} de {1} lugares restaurados", concluidos, total);

        int visiveis = 0;
        if (_conquistas != null)
            for (int i = 0; i < _conquistas.Length; i++)
            {
                Conquista conquista = _conquistas[i];
                if (conquista == null)
                    continue;
                bool desbloqueada = EstaDesbloqueada(conquista);
                if (conquista.Botao != null)
                {
                    conquista.Botao.gameObject.SetActive(desbloqueada);
                    if (desbloqueada && conquista.Rect != null)
                        conquista.Rect.anchoredPosition = new Vector2(0f, -visiveis * _passoLinha);
                }
                if (conquista.Rotulo != null)
                    conquista.Rotulo.SetText(conquista.Titulo);
                if (desbloqueada)
                    visiveis++;
                if (i == _selecionada && !desbloqueada)
                {
                    _selecionada = -1;
                    if (_detalhes != null)
                        _detalhes.SetActive(false);
                }
            }
        QuantidadeConquistas = visiveis;
        if (_quantidadeConquistas != null)
            _quantidadeConquistas.SetText("{0}/{1}", visiveis, _conquistas == null ? 0 : _conquistas.Length);
        if (_estadoVazio != null)
            _estadoVazio.SetActive(visiveis == 0);
        _assinatura = CalcularAssinatura();
    }

    public void MostrarDetalhes(int indice)
    {
        if (!EstaAberto || _conquistas == null || indice < 0 || indice >= _conquistas.Length)
            return;
        Conquista conquista = _conquistas[indice];
        if (conquista == null || !EstaDesbloqueada(conquista))
            return;
        _selecionada = indice;
        if (_tituloDetalhe != null)
            _tituloDetalhe.SetText(conquista.Titulo);
        if (_descricaoDetalhe != null)
            _descricaoDetalhe.SetText(conquista.Descricao);
        if (_imagemDetalhe != null)
        {
            _imagemDetalhe.sprite = conquista.Icone;
            _imagemDetalhe.gameObject.SetActive(conquista.Icone != null);
        }
        if (_detalhes != null)
            _detalhes.SetActive(true);
    }

    private int ContarLugaresConcluidos()
    {
        int count = 0;
        if (_chavesProgresso != null)
            for (int i = 0; i < _chavesProgresso.Length; i++)
                if (!string.IsNullOrEmpty(_chavesProgresso[i]) && PlayerPrefs.GetInt(_chavesProgresso[i], 0) == 1)
                    count++;
        return count;
    }

    private static bool EstaDesbloqueada(Conquista conquista)
    {
        // Reaproveita o mesmo acesso ao salvamento usado pelo álbum original.
        return !string.IsNullOrEmpty(conquista.Personagem) &&
            SistemaFigurinhas.EstaDesbloqueada(conquista.Personagem);
    }

    private int CalcularAssinatura()
    {
        int hash = ContarLugaresConcluidos();
        if (_conquistas != null)
            for (int i = 0; i < _conquistas.Length; i++)
                hash = hash * 31 + (_conquistas[i] != null && EstaDesbloqueada(_conquistas[i]) ? 1 : 0);
        return hash;
    }

    private void OnDestroy()
    {
        if (_acoes == null || _conquistas == null)
            return;
        for (int i = 0; i < _acoes.Length && i < _conquistas.Length; i++)
            if (_conquistas[i] != null && _conquistas[i].Botao != null && _acoes[i] != null)
                _conquistas[i].Botao.onClick.RemoveListener(_acoes[i]);
    }
}
