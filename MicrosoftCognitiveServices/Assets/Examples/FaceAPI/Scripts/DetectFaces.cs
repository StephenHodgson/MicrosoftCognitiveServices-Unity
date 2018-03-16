using System.Threading.Tasks;
using Microsoft.Cognitive.DataStructures.Face;
using Microsoft.Cognitive.Face;
using UnityEngine;

#if UNITY_WSA
using System.Linq;
using UnityEngine.XR.WSA.Input;
using UnityEngine.XR.WSA.WebCam;
#endif

public class DetectFaces : MonoBehaviour
{
    [SerializeField]
    private TextMesh labelText = null;

    private bool isBusy = false;
    private Texture2D lastCapturedPhoto;
#if UNITY_WSA
    private PhotoCapture photoCaptureObject = null;
    private GestureRecognizer gestureRecognizer;
#else
    private WebCamTexture webCamTexture;
#endif

    private void Awake()
    {
#if UNITY_WSA
        gestureRecognizer = new GestureRecognizer();
        gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap);
        gestureRecognizer.Tapped += OnTap;
#endif
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Clicked");
            ExecuteImageCaptureAndAnalysis();
        }

        for (int i = 0; i < Input.touchCount; i++)
        {
            Debug.Log("Touched");
            if (Input.GetTouch(i).phase == TouchPhase.Ended)
            {
                ExecuteImageCaptureAndAnalysis();
            }
        }
    }

#if UNITY_WSA
    private void OnTap(TappedEventArgs tapArgs)
    {
        ExecuteImageCaptureAndAnalysis();
    }
#endif

    private void ExecuteImageCaptureAndAnalysis()
    {
        if (isBusy) { return; }
        isBusy = true;
#if UNITY_WSA
        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending(res => res.width * res.height).First();
        lastCapturedPhoto = new Texture2D(cameraResolution.width, cameraResolution.height);
        PhotoCapture.CreateAsync(false, OnPhotoCaptureCallback);
#else
        webCamTexture = new WebCamTexture();
        webCamTexture.Play();
        TakePhotoAsync();
#endif
    }

#if UNITY_WSA
    private void OnPhotoCaptureCallback(PhotoCapture captureObject)
    {
        photoCaptureObject = captureObject;
        var cameraParameters = new CameraParameters
        {
            hologramOpacity = 0.0f,
            cameraResolutionWidth = lastCapturedPhoto.width,
            cameraResolutionHeight = lastCapturedPhoto.height,
            pixelFormat = CapturePixelFormat.BGRA32
        };

        captureObject.StartPhotoModeAsync(cameraParameters, OnPhotoModeStartedCallback);
    }

    private void OnPhotoModeStartedCallback(PhotoCapture.PhotoCaptureResult result)
    {
        photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemoryCallback);
    }

    private void OnCapturedPhotoToMemoryCallback(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        photoCaptureFrame.UploadImageDataToTexture(lastCapturedPhoto);
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
    }

    private void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
        DetectFacesAsync();
    }

    private async void DetectFacesAsync()
    {
        await GetPersonFromImageAsync(lastCapturedPhoto.EncodeToJPG());
        isBusy = false;
    }
#else
    private async void TakePhotoAsync()
    {
        // Give the camera some time to auto adjust for light conditions.
        await new WaitForSeconds(0.5f);

        // Make sure we wait until the end of the frame to grab the texture.
        await new WaitForEndOfFrame();

        lastCapturedPhoto = new Texture2D(webCamTexture.width, webCamTexture.height);
        lastCapturedPhoto.SetPixels(webCamTexture.GetPixels());
        lastCapturedPhoto.Apply();
        webCamTexture.Stop();
        await GetPersonFromImageAsync(lastCapturedPhoto.EncodeToJPG());

        isBusy = false;
    }
#endif

    private async Task GetPersonFromImageAsync(byte[] imageBytes)
    {
        var personGroups = await PersonGroup.GetGroupListAsync();
        for (var i = 0; i < personGroups.Length; i++)
        {
            labelText.text = "Searching Group: " + personGroups[i].name;

            // try to detect the faces in the image.
            FaceInfo[] faces = await Face.DetectAsync(imageBytes);

            if (faces != null)
            {
                if (faces.Length == 0)
                {
                    await new WaitForUpdate();
                    labelText.text = "No Faces Found!";
                    return;
                }

                // if faces are found, assign a GUID
                var faceIds = new string[faces.Length];
                for (int j = 0; j < faces.Length; j++)
                {
                    faceIds[j] = faces[j].faceId;
                }

                // try to identify the face found in the image by
                // retrieving a series of candidates form the queried group.
                var idResults = await Face.IdentifyAsync(faceIds, personGroups[i].personGroupId);

                for (var j = 0; j < idResults.Length; j++)
                {
                    double bestConfidence = 0f;
                    string personId = null;

                    // try to match the candidate to the face found
                    // in the image using a confidence value.
                    for (var k = 0; k < idResults[j].candidates.Length; k++)
                    {
                        var candidate = idResults[j].candidates[k];

                        if (bestConfidence > candidate.confidence) { continue; }

                        bestConfidence = candidate.confidence;
                        personId = candidate.personId;
                    }

                    if (string.IsNullOrEmpty(personId))
                    {
                        await new WaitForUpdate();
                        labelText.text = "No Faces Found!";
                        continue;
                    }

                    // display the candidate with the highest confidence
                    var person = await Person.GetPersonAsync(personGroups[i].personGroupId, personId);
                    labelText.text = person?.name;
                    return;
                }
            }
        }

        await new WaitForUpdate();
        labelText.text = "No Faces Found!";
    }
}
