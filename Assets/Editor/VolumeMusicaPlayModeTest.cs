using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Linq;

[InitializeOnLoad]
internal static class VolumeMusicaPlayModeTest
{
    private static string[] _ints = { "PadariaConcluida", "CorreiosConcluido", "ParqueConcluido", "PlazaConcluida", "Figurinha_DonaBiscoito", "Figurinha_CapitaoBigode", "Figurinha_Luna", "Figurinha_Zeca", "Figurinha_HeroiVizinhanca" };
    private static string[] _floats = { "VolumeMusica", "UltimoVolumeMusica" };
    private static Component _ui;
    private static AudioSource _source;
    private static System.Type _audioType;
    private static UnityEngine.UI.Slider _slider;
    private static UnityEngine.UI.Button _sound;
    private static int _stage, _frame;
    private static float _time, _chosen;
    private static double _timeout;
    private static string _errors = "";

    static VolumeMusicaPlayModeTest()
    {
        string state = SessionState.GetString("PlayModeTest.State", "Idle");
        if (state == "WaitingForCompile") EditorApplication.delayCall += () =>
        {
            SessionState.SetString("PlayModeTest.State", "EnteringPlayMode");
            EditorApplication.playModeStateChanged += OnState;
            EditorApplication.isPlaying = true;
        };
        else if (state == "EnteringPlayMode") EditorApplication.playModeStateChanged += OnState;
    }
    private static void OnState(PlayModeStateChange state)
    {
        if (state != PlayModeStateChange.EnteredPlayMode) return;
        EditorApplication.playModeStateChanged -= OnState;
        SessionState.SetString("PlayModeTest.State", "InPlayMode");
        foreach (string key in _ints) { SessionState.SetBool("VolTest.Has." + key, PlayerPrefs.HasKey(key)); SessionState.SetInt("VolTest.Int." + key, PlayerPrefs.GetInt(key)); }
        foreach (string key in _floats) { SessionState.SetBool("VolTest.Has." + key, PlayerPrefs.HasKey(key)); SessionState.SetFloat("VolTest.Float." + key, PlayerPrefs.GetFloat(key)); PlayerPrefs.DeleteKey(key); }
        _timeout = EditorApplication.timeSinceStartup + 25; _time = Time.realtimeSinceStartup;
        _audioType = System.AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetType("GerenciadorAudioPersistente")).First(t => t != null);
        Application.logMessageReceived += OnLog; EditorApplication.update += Tick;
    }
    private static void OnLog(string message, string trace, LogType type)
    {
        if (type == LogType.Error || type == LogType.Exception) _errors += message + "\n";
    }
    private static void Check(bool ok, string label)
    {
        Debug.Log("[Test] " + label + "=" + ok); if (!ok) throw new System.Exception(label);
    }
    private static void OpenUI()
    {
        _ui = Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).Single(c => c.GetType().Name == "PainelConfigProgressoConquistas");
        _ui.GetType().GetMethod("Abrir").Invoke(_ui, null);
        _slider = _ui.GetComponentsInChildren<UnityEngine.UI.Slider>(true).Single(s => s.name == "SliderVolume");
        _sound = _ui.GetComponentsInChildren<UnityEngine.UI.Button>(true).Single(b => b.name == "BotaoSom");
    }
    private static string Numeric()
    {
        return _ui.GetComponentsInChildren<TMPro.TMP_Text>(true).Single(t => t.name == "ValorVolume").text;
    }
    private static void Tick()
    {
        try
        {
            if (EditorApplication.timeSinceStartup > _timeout) throw new System.Exception("Timeout stage=" + _stage);
            float elapsed = Time.realtimeSinceStartup - _time;
            if (_stage == 0 && elapsed > 0.6f)
            {
                _source = GameObject.Find("AudioPersistente").GetComponent<AudioSource>(); OpenUI();
                Check(_slider.value == 1 && Numeric() == "100", "padrão 100 sem preferência salva");
                Time.timeScale = 0; _slider.value = 0.37f;
                Check(Mathf.Abs(_source.volume - 0.561f * 0.37f) < 0.001f && Numeric() == "37", "slider atualiza música e número em tempo real");
                _sound.onClick.Invoke(); Check(_source.volume == 0 && Numeric() == "0", "botão Som silencia");
                _sound.onClick.Invoke(); Check(Mathf.Abs(_slider.value - 0.37f) < 0.001f && Numeric() == "37", "restaura volume anterior");
                Canvas.ForceUpdateCanvases(); var area = (RectTransform)_slider.handleRect.parent; var corners = new Vector3[4]; area.GetWorldCorners(corners);
                var pointer = new PointerEventData(EventSystem.current) { button = PointerEventData.InputButton.Left, position = RectTransformUtility.WorldToScreenPoint(null, Vector3.Lerp(corners[0], corners[3], 0.72f)) };
                ExecuteEvents.Execute(_slider.gameObject, pointer, ExecuteEvents.pointerDownHandler);
                ExecuteEvents.Execute(_slider.gameObject, pointer, ExecuteEvents.dragHandler);
                _chosen = _slider.value;
                Check(Mathf.Abs(_chosen - 0.72f) < 0.02f, "arraste altera Slider horizontal");
                _ui.GetType().GetMethod("Fechar").Invoke(_ui, null); OpenUI();
                Check(Mathf.Abs(_slider.value - _chosen) < 0.001f && Mathf.Abs(PlayerPrefs.GetFloat("VolumeMusica") - _chosen) < 0.001f, "persiste ao fechar e reabrir o painel");
                foreach (string key in _ints) PlayerPrefs.SetInt(key, 1);
                _ui.GetType().GetMethod("Atualizar").Invoke(_ui, null);
                _ui.GetComponentsInChildren<UnityEngine.UI.Button>(true).Single(b => b.name == "Conquista_CapitaoBigode").onClick.Invoke();
                _frame = Time.frameCount; _stage = 1;
            }
            else if (_stage == 1 && Time.frameCount > _frame)
            {
                Canvas.ForceUpdateCanvases(); ScreenCapture.CaptureScreenshot("Assets/PlayModeTest_Volume.png");
                SessionState.SetString("PlayModeTest.ScreenshotPath", "Assets/PlayModeTest_Volume.png");
                _frame = Time.frameCount; _stage = 2;
            }
            else if (_stage == 2 && Time.frameCount > _frame + 2)
            {
                _sound.onClick.Invoke(); SceneManager.LoadScene("PadariaDepois"); _stage = 3; _time = Time.realtimeSinceStartup;
            }
            else if (_stage == 3 && elapsed > 1f && _source.clip.name == "triste")
            {
                Check(_source.volume == 0, "mudo mantido na troca de fase durante pausa");
                OpenUI(); Check(_slider.value == 0 && Numeric() == "0", "Padaria lê o volume salvo");
                _sound.onClick.Invoke(); _stage = 4;
            }
            else if (_stage == 4)
            {
                Check(_source.volume <= 0.561f * _chosen + 0.001f, "fade não ultrapassa volume escolhido");
                if (Mathf.Abs(_source.volume - 0.561f * _chosen) > 0.001f) return;
                Check(Mathf.Abs(_slider.value - _chosen) < 0.001f, "restaurou volume após mudar de cena");
                _slider.value = 0.2f; _ui.GetType().GetMethod("Fechar").Invoke(_ui, null);
                SceneManager.LoadScene("Menu"); _stage = 5;
            }
            else if (_stage == 5 && _source.clip.name == "musica_de_fundo" && Mathf.Abs(_source.volume - 0.561f * 0.2f) < 0.001f)
            {
                Check(Object.FindObjectsByType<AudioSource>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).Length == 1, "Menu sem duplicar fontes");
                SceneManager.LoadScene("Mapa"); _stage = 6; _time = Time.realtimeSinceStartup;
            }
            else if (_stage == 6 && elapsed > 0.5f)
            {
                OpenUI(); Check(Numeric() == "20" && Mathf.Abs(_source.volume - 0.561f * 0.2f) < 0.001f, "Menu/Mapa preservam volume escolhido");
                Object.Destroy(_source.gameObject); _stage = 7; _frame = Time.frameCount;
            }
            else if (_stage == 7 && Time.frameCount > _frame + 1)
            {
                SceneManager.LoadScene("Mapa"); _stage = 8; _time = Time.realtimeSinceStartup;
            }
            else if (_stage == 8 && elapsed > 0.5f)
            {
                _source = GameObject.Find("AudioPersistente").GetComponent<AudioSource>(); OpenUI();
                Check(Numeric() == "20" && Mathf.Abs(_source.volume - 0.561f * 0.2f) < 0.001f, "novo gerenciador recupera volume salvo");
                Finish("PASS: slider/arraste, número, mudo/restauração, pausa, troca de fase, Menu/Mapa e reinicialização verificados.\n" + _errors);
            }
        }
        catch (System.Exception e) { Finish("FAIL: " + e + "\n" + _errors); }
    }
    private static void Finish(string report)
    {
        EditorApplication.update -= Tick; Application.logMessageReceived -= OnLog;
        foreach (string key in _ints)
        {
            if (SessionState.GetBool("VolTest.Has." + key, false)) PlayerPrefs.SetInt(key, SessionState.GetInt("VolTest.Int." + key, 0)); else PlayerPrefs.DeleteKey(key);
            SessionState.EraseInt("VolTest.Int." + key); SessionState.EraseBool("VolTest.Has." + key);
        }
        foreach (string key in _floats)
        {
            if (SessionState.GetBool("VolTest.Has." + key, false)) PlayerPrefs.SetFloat(key, SessionState.GetFloat("VolTest.Float." + key, 0)); else PlayerPrefs.DeleteKey(key);
            SessionState.EraseFloat("VolTest.Float." + key); SessionState.EraseBool("VolTest.Has." + key);
        }
        PlayerPrefs.Save(); Time.timeScale = 1;
        SessionState.SetString("PlayModeTest.Result", report); SessionState.SetString("PlayModeTest.State", "Done"); EditorApplication.isPlaying = false;
    }
}
