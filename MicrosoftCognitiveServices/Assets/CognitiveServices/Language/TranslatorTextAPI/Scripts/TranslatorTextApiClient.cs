using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Microsoft.Cognitive.Language.TranslatorText
{
    public static class TranslatorTextApiClient
    {
        private const string RequestHeaderKey = "Ocp-Apim-Subscription-Key";
        public static string ApiKey { get; set; } = "===>API Key Goes Here<===";
        public static Dictionary<string, string> ApiKeyHeader
        {
            get
            {
                if (ApiKey != currentFaceApiKeyHeader[RequestHeaderKey])
                {
                    currentFaceApiKeyHeader = new Dictionary<string, string> { { RequestHeaderKey, ApiKey } };
                }

                return currentFaceApiKeyHeader;
            }
        }

        private static Dictionary<string, string> currentFaceApiKeyHeader = new Dictionary<string, string> { { RequestHeaderKey, ApiKey } };

        private static readonly string[] languages =
        {
            "af","ar","bg","bn","bs","ca","cs","cy","da","de","el","en","es","et","fa","fi","fil","fj","fr","he","hi","hr","ht",
            "hu","id","it","ja","ko","lt","lv","mg","ms","mt","mww","nb","nl","otq","pl","pt","ro","ru","sk","sl","sm","sr-Cyrl",
            "sr-Latn","sv","sw","ta","th","tlh","to","tr","ty","uk","ur","vi","yua","yue","zh-Hans","zh-Hant"
        };

        private const string BaseEndpoint = @"https://api.microsofttranslator.com/V2/Http.svc";

        /// <summary>
        /// Translates a text string from one language to another.
        /// </summary>
        /// <param name="text">Text to translate</param>
        /// <param name="fromLanguage">Language to translate from.</param>
        /// <param name="toLanguage">Language to translate to.</param>
        /// <returns>The translated text.</returns>
        public static async Task<string> TranslateAsync(string text, TtsLanguageType fromLanguage, TtsLanguageType toLanguage)
        {
            var query = $"{BaseEndpoint}/Translate?text={text}&from={languages[(int)fromLanguage]}&to={languages[(int)toLanguage]}";
            var response = await Rest.GetAsync(query, ApiKeyHeader);

            if (response.Successful)
            {
                return response.ResponseBody.StripXml();
            }

            return string.Empty;
        }

        /// <summary>
        /// Identify the language of a selected piece of text.
        /// </summary>
        /// <param name="text"></param>
        /// <returns>The the language.</returns>
        public static async Task<TtsLanguageType> DetectLanguageAsync(string text)
        {
            var query = $"{BaseEndpoint}/Detect?text={text}";
            var response = await Rest.GetAsync(query, ApiKeyHeader);

            for (int i = 0; i < languages.Length; i++)
            {
                if (languages[i].Equals(response.ResponseBody.StripXml()))
                {
                    return (TtsLanguageType)i;
                }
            }

            throw new Exception($"Unable to detect the language. {response.ResponseBody}");
        }

        /// <summary>
        /// Gets the AudioClip of the passed-in text being spoken in the desired language.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="language"></param>
        /// <returns>Audio Clip of the text to be spoken.</returns>
        public static async Task<AudioClip> SpeakAsync(string text, TtsLanguageType language)
        {
            var query = $"{BaseEndpoint}/Speak?text={text}&language={languages[(int)language]}";
            return await Rest.GetAudioClipAsync(query, AudioType.WAV, ApiKeyHeader);
        }

        private static string StripXml(this string text)
        {
            text = text.Replace("<string xmlns=\"http://schemas.microsoft.com/2003/10/Serialization/\">", string.Empty);
            text = text.Replace("</string>", string.Empty);
            return text;
        }
    }
}
