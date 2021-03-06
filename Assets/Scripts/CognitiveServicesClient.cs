﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR.WSA.WebCam;

public class CognitiveServicesClient : MonoBehaviour
{
    public Text TagsTextbox;
    public Text ImageCaptionTextbox;
    public Text FaceApiCaptionTextbox;

    private PhotoCapture photoCapture = null;

    private const string FILE_NAME = @"cognitive_analysis.jpg";

    // This method request to create a PhotoCapture object.
    // When its finish created, call the OnPhotoCreated method.
    private void Analyze()
    {
        PhotoCapture.CreateAsync(false, this.OnPhotoCreated);
    }

    // This method store the PhotoCapture object just created and retrieve the high quality
    // available for the camera and then request to start capturing the photo with the
    // given camera parameters.
    private void OnPhotoCreated(PhotoCapture captureObject)
    {
        this.photoCapture = captureObject;

        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();

        CameraParameters c = new CameraParameters()
        {
            hologramOpacity = 0.0f,
            cameraResolutionWidth = cameraResolution.width,
            cameraResolutionHeight = cameraResolution.height,
            pixelFormat = CapturePixelFormat.BGRA32
        };
        captureObject.StartPhotoModeAsync(c, this.OnPhotoModeStarted);
    }

    // This method is called when we have access to the camera and can take photo with it.
    // We request to take the photo and store it in the storage.
    private void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result)
    {
        if (result.success)
        {
            string filename = string.Format(FILE_NAME);
            string filePath = Path.Combine(Application.persistentDataPath, filename);
            this.photoCapture.TakePhotoAsync(filePath, PhotoCaptureFileOutputFormat.JPG, this.OnCapturedPhotoToDisk);
        }
        else
        {
            Debug.LogError("Unable to start photo mode.");
        }
    }

    // This method is called when the photo is finish taked (or not, so check the succes property)
    // We can read the file from disk and do anything we need with it.
    // Finally, we request to stop the photo mode to free the resource.
    private void OnCapturedPhotoToDisk(PhotoCapture.PhotoCaptureResult result)
    {
        if (result.success)
        {
            string filename = string.Format(FILE_NAME);
            string filePath = Path.Combine(Application.persistentDataPath, filename);

            byte[] image = File.ReadAllBytes(filePath);

            // We have the photo taken.
            GetComputerVisionTags(image);
            GetFaceAPITags(image);
        }
        else
        {
            Debug.LogError("Failed to save Photo to disk.");
        }
        this.photoCapture.StopPhotoModeAsync(this.OnStoppedPhotoMode);
    }

    // This method is called when the photo mode is stopped and we can dispose the resources allocated.
    private void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        this.photoCapture.Dispose();
        this.photoCapture = null;
    }

    private void GetFaceAPITags(byte[] image)
    {
        StartCoroutine(RunFaceAPIAnalysis(image));
    }

    private const string FaceAPIUriBase = "https://westcentralus.api.cognitive.microsoft.com/face/v1.0";
    private const string FaceAPIKey = "3a80dc522cfe4e2e8c9765eb0db0e94e";

    private IEnumerator RunFaceAPIAnalysis(byte[] image)
    {
        var headers = new Dictionary<string, string>() {
            { "Ocp-Apim-Subscription-Key", FaceAPIKey },
            { "Content-Type", "application/octet-stream" }
        };

        string FaceAPIURL = FaceAPIUriBase + "/detect?returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses,emotion,hair,accessories";
        WWW httpClient = new WWW(FaceAPIURL, image, headers);
        yield return httpClient;

        //When return ...
        var jsonResult = httpClient.text;

        List<string> tags = new List<string>();
        // http://answers.unity3d.com/questions/1148632/jsonutility-and-arrays-error-json-must-represent-a.html
        var jsonResults = "{\"values\":" + httpClient.text + "}";
        
        var result = JsonUtility.FromJson<FaceAPIResult>(jsonResults);

        this.FaceApiCaptionTextbox.text = string.Join("\n",  result.values.Select(x => string.Format("Age: {0} - Gender: {1} - Glasses: {2}", x.attributes.age, x.attributes.gender, x.attributes.glasses)).ToArray());
    }

    private void GetComputerVisionTags(byte[] image)
    {
        StartCoroutine(RunComputerVisionAnalysis(image));
    }

    private const string ComputerVisionUriBase = "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0/analyze";
    private const string ComputerVisionKey = "6427d8d45e8e4e178bc7d52facc63f90";

    private IEnumerator RunComputerVisionAnalysis(byte[] image)
    {
        var headers = new Dictionary<string, string>() {
            { "Ocp-Apim-Subscription-Key", ComputerVisionKey },
            { "Content-Type", "application/octet-stream" }
        };

        string ComputerVisionURL = ComputerVisionUriBase + "?visualFeatures=Categories,Description&language=en";

        WWW httpClient = new WWW(ComputerVisionURL, image, headers);
        yield return httpClient;

        //When return ...
        var jsonResult = httpClient.text;

        List<string> tags = new List<string>();
        var jsonResults = httpClient.text;
        var result = JsonUtility.FromJson<ComputerVisionResult>(jsonResults);

        this.TagsTextbox.text = string.Join("\n", result.description.tags);
        this.ImageCaptionTextbox.text = string.Join("\n", result.description.captions.OrderByDescending(x => x.confidence).Select(x => x.text).ToArray());
    }

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("Analyze", 5f, 30f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
