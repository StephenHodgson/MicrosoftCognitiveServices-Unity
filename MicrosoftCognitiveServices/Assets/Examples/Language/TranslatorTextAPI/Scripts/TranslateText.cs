using Microsoft.Cognitive.Language.TranslatorText;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TranslateText : MonoBehaviour
{
    [SerializeField]
    private string textToSend;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            SendText();
        }
    }

    private async void SendText()
    {
        Debug.Log(textToSend);
        var translatedText = await TranslatorTextApiClient.TranslateAsync(textToSend, TtsLanguageType.English, TtsLanguageType.Japanese);
        Debug.Log(translatedText);
        var language = await TranslatorTextApiClient.DetectLanguageAsync(translatedText);
        Debug.Log(language.ToString());
        audioSource.PlayOneShot(await TranslatorTextApiClient.SpeakAsync(translatedText, TtsLanguageType.Japanese));
    }
}
