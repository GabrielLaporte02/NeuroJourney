using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class NavegacaoJogo : MonoBehaviour
{
    [Header("Cenas verificadas no projeto")]
    [SerializeField] private string _cenaMenu = "Menu";
    [SerializeField] private string _cenaMapa = "Mapa";

    [Header("Continuar (para novas instâncias do prefab)")]
    [Tooltip("Conecte o avanço do diálogo. Nas cenas existentes, os eventos originais do Button são preservados.")]
    [SerializeField] private UnityEvent _aoContinuar = new UnityEvent();

    private static bool _carregando;
    private static string _cenaAnterior;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Inicializar()
    {
        _carregando = false;
        _cenaAnterior = null;
        SceneManager.sceneLoaded -= AoCarregarCena;
        SceneManager.sceneLoaded += AoCarregarCena;
    }

    public void Jogar()
    {
        CarregarCena(_cenaMapa);
    }

    public void VoltarMenu()
    {
        CarregarCena(_cenaMenu);
    }

    public void VoltarMapa()
    {
        CarregarCena(_cenaMapa);
    }

    public void VoltarCenaAnterior()
    {
        CarregarCena(string.IsNullOrEmpty(_cenaAnterior) ? _cenaMenu : _cenaAnterior);
    }

    public void Continuar()
    {
        _aoContinuar.Invoke();
    }

    public static void CarregarCena(string nomeCena)
    {
        if (_carregando || SceneManager.GetActiveScene().name == nomeCena)
            return;

        if (string.IsNullOrWhiteSpace(nomeCena) || !Application.CanStreamedLevelBeLoaded(nomeCena))
        {
            Debug.LogError("NavegacaoJogo: cena não disponível no Build Profile: " + nomeCena);
            return;
        }

        _cenaAnterior = SceneManager.GetActiveScene().name;
        _carregando = true;
        try
        {
            if (TransicaoNuvens.instancia != null && TransicaoNuvens.instancia.isActiveAndEnabled)
                TransicaoNuvens.instancia.MudarCena(nomeCena);
            else
                SceneManager.LoadScene(nomeCena);
        }
        catch (System.Exception)
        {
            _carregando = false;
            throw;
        }
    }

    private static void AoCarregarCena(Scene cena, LoadSceneMode modo)
    {
        if (modo == LoadSceneMode.Single)
            _carregando = false;
    }
}
