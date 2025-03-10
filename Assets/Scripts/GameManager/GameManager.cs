using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public ManagementData managementData;
    public Animator OpenCloseScene;
    public void Start(){
        managementData.SetAudioMixerData();
    }
    public void ChangeSceneSelector(TypeScene typeScene)
    {
        switch (typeScene)
        {
            case TypeScene.HomeScene:
                OpenCloseScene.SetBool("Out", true);
                OpenCloseScene.Play("Out");
                StartCoroutine(FadeOut());
                StartCoroutine(ChangeScene(typeScene));
                break;
            case TypeScene.OptionsScene:
                SceneManager.LoadScene("OptionsScene", LoadSceneMode.Additive);
                break;
            case TypeScene.GameScene:
                OpenCloseScene.SetBool("Out", true);
                OpenCloseScene.Play("Out");
                StartCoroutine(FadeOut());
                StartCoroutine(ChangeScene(typeScene));
                break;
            case TypeScene.Exit:
                OpenCloseScene.SetBool("Out", true);
                OpenCloseScene.Play("Out");
                StartCoroutine(FadeOut());
                StartCoroutine(ChangeScene(typeScene));
                break;
        }
    }
    public IEnumerator FadeIn()
    {        
        float decibelsMaster = 20 * Mathf.Log10(ManagementData.saveData.configurationsInfo.soundConfiguration.MASTERValue / 100);
        float currentVolumen = 0;
        float volume = 0;
        if (ManagementData.audioMixer.GetFloat(ManagementOptions.TypeSound.Master.ToString(), out volume)){
            currentVolumen = volume;
        }
        else{
            currentVolumen = -80f;
        }
        while (currentVolumen < decibelsMaster)
        {
            if (ManagementData.saveData.configurationsInfo.soundConfiguration.isMute) break;
            currentVolumen++;
            ManagementData.audioMixer.SetFloat(ManagementOptions.TypeSound.Master.ToString(), currentVolumen);
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }
    public IEnumerator ChangeScene(TypeScene typeScene){
        Time.timeScale = 1;
        yield return new WaitForSecondsRealtime(2);
        if (typeScene != TypeScene.Exit)
        {
            SceneManager.LoadScene(typeScene.ToString());
        }
        else
        {
            Application.Quit();
        }
        OpenCloseScene.SetBool("Out", false);
        StartCoroutine(FadeIn());
    }
    public IEnumerator FadeOut()
    {
        float decibelsMaster = 20 * Mathf.Log10(ManagementData.saveData.configurationsInfo.soundConfiguration.MASTERValue / 100);
        while (decibelsMaster > -80)
        {
            if (ManagementData.saveData.configurationsInfo.soundConfiguration.isMute) break;
            decibelsMaster -= 1;
            ManagementData.audioMixer.SetFloat(ManagementOptions.TypeSound.Master.ToString(), decibelsMaster);
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }
    public void PlayASound(AudioClip audioClip)
    {
        AudioSource audioBox = Instantiate(Resources.Load<GameObject>("Prefabs/AudioBox/AudioBox")).GetComponent<AudioSource>();
        audioBox.clip = audioClip;
        audioBox.Play();
        Destroy(audioBox.gameObject, audioBox.clip.length);
    }
    public void PlayASound(AudioClip audioClip, float initialRandomPitch)
    {
        AudioSource audioBox = Instantiate(Resources.Load<GameObject>("Prefabs/AudioBox/AudioBox")).GetComponent<AudioSource>();
        audioBox.clip = audioClip;
        audioBox.pitch = Random.Range(initialRandomPitch - 0.1f, initialRandomPitch + 0.1f);
        audioBox.Play();
        Destroy(audioBox.gameObject, audioBox.clip.length);
    }

    internal void SetAudioMixerData()
    {
        managementData.SetAudioMixerData();
    }

    public enum TypeScene
    {
        HomeScene = 0,
        OptionsScene = 1,
        GameScene = 2,
        Exit = 3
    }
}
