using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider musicSlider;

    private const string MASTER_VOLUME_PARAM = "MasterVolume";
    private const string SFX_VOLUME_PARAM = "SFXVolume";
    private const string MUSIC_VOLUME_PARAM = "MusicVolume";

    private void Start()
    {
        // Load saved audio settings (if any)
        masterSlider.value = PlayerPrefs.GetFloat(MASTER_VOLUME_PARAM, 1f);
        sfxSlider.value = PlayerPrefs.GetFloat(SFX_VOLUME_PARAM, 1f);
        musicSlider.value = PlayerPrefs.GetFloat(MUSIC_VOLUME_PARAM, 1f);
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat(MASTER_VOLUME_PARAM, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(MASTER_VOLUME_PARAM, volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat(SFX_VOLUME_PARAM, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(SFX_VOLUME_PARAM, volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat(MUSIC_VOLUME_PARAM, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(MUSIC_VOLUME_PARAM, volume);
    }
}
