# MicrosoftCognitiveServices-Unity
This library is inteded to speed up development time to utilize the Microsoft Cognitive Services inside the Unity Editor.

Includes a helpful REST library written by me and the [Unity async/await utilities by Steve Vermeulen](https://github.com/svermeulen/Unity3dAsyncAwaitUtil).

Initially written at the Azure & MR/AI Hackathon March 2018.

The library was mostly generated from the OpenAPI (swagger) endpoints provided in the microsoft docs.  If an API is missing, it's most likely because an API definition endpoint has not been provided by Microsoft for that specific API.

# Requirements

- Unity 2017.x or higher with .NET 4.x support enabled.
- Visual Studio 2017

# Confirmed Supported Platforms

- [x] Windows Standalone
- [x] Windows Universal
- [ ] Android
- [ ] iOS
- [ ] WebGl

# Feature Areas
- [ ] Vision
    - [ ] [Computer Vision API](https://azure.microsoft.com/en-us/services/cognitive-services/computer-vision/)
    - [x] [Face API](https://azure.microsoft.com/en-us/services/cognitive-services/face/)
    - [ ] [Content Moderator](https://azure.microsoft.com/en-us/services/cognitive-services/content-moderator/)
    - [ ] [Emotion API (PREVIEW)](https://azure.microsoft.com/en-us/services/cognitive-services/emotion/)
    - [ ] [Custom Vision Service (PREVIEW)](https://azure.microsoft.com/en-us/services/cognitive-services/custom-vision-service/)
    - [ ] [Video Indexer (PREVIEW)](https://azure.microsoft.com/en-us/services/cognitive-services/video-indexer/)
- [ ] Speech
    - [ ] [Translator Speech API](https://azure.microsoft.com/en-us/services/cognitive-services/translator-speech-api/)
    - [ ] [Bing Speech API](https://azure.microsoft.com/en-us/services/cognitive-services/speech/)
    - [ ] [Speaker Recognition API (PREVIEW)](https://azure.microsoft.com/en-us/services/cognitive-services/speaker-recognition/)
    - [ ] [Custom Speech Service (PREVIEW)](https://azure.microsoft.com/en-us/services/cognitive-services/custom-speech-service/)
- [ ] Language
    - [ ] [Language Understanding (LUIS)](https://azure.microsoft.com/en-us/services/cognitive-services/language-understanding-intelligent-service/)
    - [ ] [Text Analytics API](https://azure.microsoft.com/en-us/services/cognitive-services/text-analytics/)
    - [ ] [Bing Spell Check API](https://azure.microsoft.com/en-us/services/cognitive-services/spell-check/)
    - [x] [Translator Text API](https://azure.microsoft.com/en-us/services/cognitive-services/translator-text-api/)
    - [ ] [Web Language Model API (PREVIEW)](https://azure.microsoft.com/en-us/services/cognitive-services/web-language-model/)
    - [ ] [Linguistic Analysis API (PREVIEW)](https://azure.microsoft.com/en-us/services/cognitive-services/linguistic-analysis-api/)
- [ ] Knowledge
    - [ ] [QnA Maker API (PREVIEW)](https://azure.microsoft.com/en-us/services/cognitive-services/qna-maker/)
    - [ ] [Custom Decision Service (PREVIEW)](https://azure.microsoft.com/en-us/services/cognitive-services/custom-decision-service/)
- [ ] Search
    - [ ] [Bing Autosuggest API](https://azure.microsoft.com/en-us/services/cognitive-services/autosuggest/)
    - [ ] [Bing Image Search API](https://azure.microsoft.com/en-us/services/cognitive-services/bing-image-search-api/)
    - [ ] [Bing News Search API](https://azure.microsoft.com/en-us/services/cognitive-services/bing-news-search-api/)
    - [ ] [Bing Video Search API](https://azure.microsoft.com/en-us/services/cognitive-services/bing-video-search-api/)
    - [ ] [Bing Web Search API](https://azure.microsoft.com/en-us/services/cognitive-services/bing-web-search-api/)
    - [ ] [Bing Custom Search API](https://azure.microsoft.com/en-us/services/cognitive-services/bing-custom-search/)
    - [ ] [Bing Entity Search API](https://azure.microsoft.com/en-us/services/cognitive-services/bing-entity-search-api/)

# Getting Started

To open the dashboard: `Window->Microsoft Cognitive Services`.

Open the FaceApiClient in Visual Studio and paste in your key. (the editor inspector does not work)
