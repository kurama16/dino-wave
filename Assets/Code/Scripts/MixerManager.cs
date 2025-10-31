using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public enum AudioMixerTypeEnum
{
    MasterVolume,
    UIVolume,
    MusicVolume,
    SFXVolume
}

[DisallowMultipleComponent]
public class MixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Slider master;
    [SerializeField] private Slider music;
    [SerializeField] private Slider sfx;
    [SerializeField] private Slider ui;

    private void Awake()
    {
        if (!audioMixer)
            Debug.LogWarning("[MixerManager] AudioMixer no asignado.");
    }

    private void Start()
    {
        ApplySavedVolumesToMixer();
        ApplySavedVolumeToUI();
    }

    public void SliderMaster_OnValueChanged(float value)
    {
        Base_OnValueChanged(AudioMixerTypeEnum.MasterVolume, value);
    }

    public void SliderMusic_OnValueChanged(float value)
    {
        Base_OnValueChanged(AudioMixerTypeEnum.MusicVolume, value);
    }

    public void SliderGame_OnValueChanged(float value)
    {
        Base_OnValueChanged(AudioMixerTypeEnum.SFXVolume, value);
    }

    public void SliderUI_OnValueChanged(float value)
    {
        Base_OnValueChanged(AudioMixerTypeEnum.UIVolume, value);
    }

    #region [Private Methods]

    #region [SFX]
    private void SetSFXVolume(float value)
    {
        SetVolume(AudioMixerTypeEnum.SFXVolume, value);
    }

    private void SaveSFXVolumePlayerPrefs(float value)
    {
        SaveVolume(AudioMixerTypeEnum.SFXVolume, value);
    }

    private float GetSFXVolumePlayerPrefs()
    {
        return GetSavedVolumeOrDefault(AudioMixerTypeEnum.SFXVolume);
    }
    #endregion

    #region [UI]
    private void SetUIVolume(float value)
    {
        SetVolume(AudioMixerTypeEnum.UIVolume, value);
    }

    private void SaveUIVolumePlayerPrefs(float value)
    {
        SaveVolume(AudioMixerTypeEnum.UIVolume, value);
    }

    private float GetUIVolumePlayerPrefs()
    {
        return GetSavedVolumeOrDefault(AudioMixerTypeEnum.UIVolume);
    }
    #endregion

    #region [Music]
    private void SetMusicVolume(float value)
    {
        SetVolume(AudioMixerTypeEnum.MusicVolume, value);
    }

    private void SaveMusicVolumePlayerPrefs(float value)
    {
        SaveVolume(AudioMixerTypeEnum.MusicVolume, value);
    }

    private float GetMusicVolumePlayerPrefs()
    {
        return GetSavedVolumeOrDefault(AudioMixerTypeEnum.MusicVolume);
    }
    #endregion

    #region [Master]
    private void SetMasterVolume(float value)
    {
        SetVolume(AudioMixerTypeEnum.MasterVolume, value);
    }

    private void SaveMasterVolumePlayerPrefs(float value)
    {
        SaveVolume(AudioMixerTypeEnum.MasterVolume, value);
    }

    private float GetMasterVolumePlayerPrefs()
    {
        return GetSavedVolumeOrDefault(AudioMixerTypeEnum.MasterVolume);
    }
    #endregion

    private float FloatToDecibels(float value)
    {
        var dBvalue = value == 0 ? -80 : Mathf.Log10(value) * 20;
        return dBvalue;
    }

    private float GetSavedVolumeOrDefault(AudioMixerTypeEnum param, float def = 1f)
    {
        string key = param.ToString();
        if (!string.IsNullOrEmpty(key))
        {
            var prefValue = PlayerPrefs.GetFloat(key);
            return prefValue;
        }
        return def;
    }

    private void SetVolume(AudioMixerTypeEnum param, float value)
    {
        if (!audioMixer)
            return;

        float valueClamped = Mathf.Clamp01(value);
        float dB = FloatToDecibels(valueClamped);
        bool wasEdited = audioMixer.SetFloat(param.ToString(), dB);

        if (!wasEdited)
        {
            Debug.LogWarning($"[MixerManager] Parámetro '{param}' no encontrado en AudioMixer. ");
        }
    }

    private void SaveVolume(AudioMixerTypeEnum param, float value)
    {
        float v = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat(param.ToString(), v);
    }

    private void Base_OnValueChanged(AudioMixerTypeEnum audioMixerType, float value)
    {
        switch (audioMixerType)
        {
            case AudioMixerTypeEnum.MasterVolume:
                SetMasterVolume(value);
                SaveMasterVolumePlayerPrefs(value);
                break;
            case AudioMixerTypeEnum.MusicVolume:
                SetMusicVolume(value);
                SaveMusicVolumePlayerPrefs(value);
                break;
            case AudioMixerTypeEnum.SFXVolume:
                SetSFXVolume(value);
                SaveSFXVolumePlayerPrefs(value);
                break;
            case AudioMixerTypeEnum.UIVolume:
                SetUIVolume(value);
                SaveUIVolumePlayerPrefs(value);
                break;
        }
    }

    private void ApplySavedVolumeToUI()
    {
        var masterVolume = GetMasterVolumePlayerPrefs();
        var musicVolume = GetMusicVolumePlayerPrefs();
        var sfxVolume = GetSFXVolumePlayerPrefs();
        var uiVolume = GetUIVolumePlayerPrefs();
        master.SetValueWithoutNotify(masterVolume);
        music.SetValueWithoutNotify(musicVolume);
        sfx.SetValueWithoutNotify(sfxVolume);
        ui.SetValueWithoutNotify(uiVolume);
    }

    private void ApplySavedVolumesToMixer()
    {
        SetMasterVolume(GetSavedVolumeOrDefault(AudioMixerTypeEnum.MasterVolume));
        SetMusicVolume(GetSavedVolumeOrDefault(AudioMixerTypeEnum.MusicVolume));
        SetSFXVolume(GetSavedVolumeOrDefault(AudioMixerTypeEnum.SFXVolume));
        SetUIVolume(GetSavedVolumeOrDefault(AudioMixerTypeEnum.UIVolume));
    }
    #endregion
}
