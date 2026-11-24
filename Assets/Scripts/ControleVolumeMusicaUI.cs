using UnityEngine;

[DisallowMultipleComponent]
public class ControleVolumeMusicaUI : MonoBehaviour
{
    [Header("Volume da música")]
    [SerializeField] private UnityEngine.UI.Slider _slider;
    [SerializeField] private UnityEngine.UI.Button _botaoSom;
    [SerializeField] private TMPro.TMP_Text _rotuloSom;
    [SerializeField] private TMPro.TMP_Text _valorVolume;

    private void OnEnable()
    {
        if (_slider != null)
            _slider.onValueChanged.AddListener(AoAlterarSlider);
        if (_botaoSom != null)
            _botaoSom.onClick.AddListener(AlternarMudo);
        GerenciadorAudioPersistente.VolumeMusicaAlterado += AtualizarVisual;
        AtualizarVisual(GerenciadorAudioPersistente.VolumeMusica);
    }

    private void AoAlterarSlider(float valor)
    {
        GerenciadorAudioPersistente.DefinirVolumeMusica(valor);
    }

    public void AlternarMudo()
    {
        GerenciadorAudioPersistente.AlternarMudoMusica();
    }

    private void AtualizarVisual(float volume)
    {
        if (_slider != null)
            _slider.SetValueWithoutNotify(volume);
        if (_valorVolume != null)
            _valorVolume.SetText("{0}", Mathf.RoundToInt(volume * 100f));
        if (_rotuloSom != null)
            _rotuloSom.SetText(volume <= 0f ? "Mudo" : "Som");
    }

    private void OnDisable()
    {
        if (_slider != null)
            _slider.onValueChanged.RemoveListener(AoAlterarSlider);
        if (_botaoSom != null)
            _botaoSom.onClick.RemoveListener(AlternarMudo);
        GerenciadorAudioPersistente.VolumeMusicaAlterado -= AtualizarVisual;
        GerenciadorAudioPersistente.SalvarPreferenciasVolume();
    }
}
