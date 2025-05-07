using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingView : BaseView
{
    public GameObject musicOn, musicOff;

    public GameObject soundOn, soundOff;

    public GameObject vibrationOn, vibrationOff;

    public string androidGameUrl;

    public string iosGameUrl;

    public string termUrl;

    public string privacyUrl;

    public string contactUrl;

    public override void InitView()
    {
        if (AudioManager.instance.musicState)
        {
            musicOn.SetActive(false);
            musicOff.SetActive(true);
        }
        else
        {
            musicOn.SetActive(true);
            musicOff.SetActive(false);
        }

        if (AudioManager.instance.soundState)
        {
            soundOn.SetActive(false);
            soundOff.SetActive(true);
        }
        else
        {
            soundOn.SetActive(true);
            soundOff.SetActive(false);
        }

        if (AudioManager.instance.hapticState)
        {
            vibrationOn.SetActive(false);
            vibrationOff.SetActive(true);
        }
        else
        {
            vibrationOn.SetActive(true);
            vibrationOff.SetActive(false);
        }
    }

    public override void Start()
    {

    }

    public override void Update()
    {

    }

    public override void ShowView()
    {
        base.ShowView();
        InitView();
    }

    public override void HideView()
    {
        base.HideView();
        AudioManager.instance.clickSound.Play();
    }

    public void ToggleSound()
    {
       
        if (AudioManager.instance.soundState)
        {
            soundOn.SetActive(true);
            soundOff.SetActive(false);
            AudioManager.instance.ToogleSound(true);
        }
        else
        {
            soundOn.SetActive(false);
            soundOff.SetActive(true);
            AudioManager.instance.ToogleSound(false);
        }
    }

    public void ToggleMusic()
    {
       
        if (AudioManager.instance.musicState)
        {
            musicOn.SetActive(true);
            musicOff.SetActive(false);
            AudioManager.instance.ToogleMusic(true);
        }
        else
        {
            musicOn.SetActive(false);
            musicOff.SetActive(true);
            AudioManager.instance.ToogleMusic(false);
        }
    }

    public void ToggleHaptic()
    {

        if (AudioManager.instance.hapticState)
        {
            vibrationOn.SetActive(true);
            vibrationOff.SetActive(false);
            AudioManager.instance.ToogleHaptic(true);
        }
        else
        {
            vibrationOn.SetActive(false);
            vibrationOff.SetActive(true);
            AudioManager.instance.ToogleHaptic(false);
        }
    }

    public void ContactUs()
    {
        AudioManager.instance.clickSound.Play();
        Application.OpenURL(contactUrl);
    }

    public void Rate()
    {
        AudioManager.instance.clickSound.Play();
#if UNITY_IOS
        Application.OpenURL(iosGameUrl);
#elif UNITY_ANDROID
        Application.OpenURL(androidGameUrl);
#endif
        
    }

    public void Term()
    {
        AudioManager.instance.clickSound.Play();
        Application.OpenURL(termUrl);
    }

    public void Policy()
    {
        AudioManager.instance.clickSound.Play();
        Application.OpenURL(privacyUrl);
    }


    public void Restore()
    {
        AudioManager.instance.clickSound.Play();
        //IAPManager.instance.RestorePurchases();
       
    }

    public void QuitGame()
    {
        AudioManager.instance.clickSound.Play();
        Application.Quit();
    }
}
