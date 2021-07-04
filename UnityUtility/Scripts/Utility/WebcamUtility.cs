using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.WebCam;

public static class WebcamUtility
{

    private static PhotoCapture currentPicture = null;
    private static Texture2D currentPhoto;

    private static Action<Texture2D> onPhotoTaken;

    private static bool canTakePhoto = true;
    private static bool hasWebcam;

    public static Resolution GetWebcamResolution()
    {
        hasWebcam = WebCamTexture.devices.Length > 0;
        if (!hasWebcam)
        {
            Debug.Log("Please hook up a webcam.");
            return new Resolution() { width = -1, height = -1, refreshRate = -1 };
        }

        if (Application.platform != RuntimePlatform.WindowsPlayer && Application.platform != RuntimePlatform.WindowsEditor)
        {
            Debug.Log("WebcamUtility only works on windows.");
            return new Resolution() { width = -1, height = -1, refreshRate = -1 };
        }

        Resolution cameraResolution = VideoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();

        return cameraResolution;
    }

    public static float GetWebcamFrameRate()
    {
        hasWebcam = WebCamTexture.devices.Length > 0;
        if (!hasWebcam)
        {
            Debug.Log("Please hook up a webcam.");
            return -1;
        }

        if (Application.platform != RuntimePlatform.WindowsPlayer && Application.platform != RuntimePlatform.WindowsEditor)
        {
            Debug.Log("WebcamUtility only works on windows.");
            return -1;
        }

        float cameraFramerate = VideoCapture.GetSupportedFrameRatesForResolution(GetWebcamResolution()).OrderByDescending((fps) => fps).First();
        return cameraFramerate;
    }

    public static void TakePhoto()
    {
        hasWebcam = WebCamTexture.devices.Length > 0;
        if (!hasWebcam)
        {
            Debug.Log("Please hook up a webcam.");
            return;
        }

        if (Application.platform != RuntimePlatform.WindowsPlayer && Application.platform != RuntimePlatform.WindowsEditor)
        {
            Debug.Log("WebcamUtility only works on windows.");
            return;
        }

        if (!canTakePhoto) return;

        canTakePhoto = false;
        PhotoCapture.CreateAsync(false, (PhotoCapture capture) => {
            currentPicture = capture;

            Resolution cameraResolution = GetWebcamResolution();

            CameraParameters c = new CameraParameters();
            c.hologramOpacity = 0.0f;
            c.cameraResolutionWidth = cameraResolution.width;
            c.cameraResolutionHeight = cameraResolution.height;
            c.pixelFormat = CapturePixelFormat.BGRA32;

            capture.StartPhotoModeAsync(c, (PhotoCapture.PhotoCaptureResult result) => {
                if (result.success)
                {
                    currentPicture.TakePhotoAsync((PhotoCapture.PhotoCaptureResult r, PhotoCaptureFrame photoCaptureFrame) => {
                        currentPhoto = new Texture2D(cameraResolution.width, cameraResolution.height);
                        photoCaptureFrame.UploadImageDataToTexture(currentPhoto);

                        if (onPhotoTaken != null) onPhotoTaken(GetLastPhoto());
                        canTakePhoto = true;
                    });
                }   
                else
                {
                    Debug.Log("Failed to capture photo with webcam.");
                }
            });
        });
    }

    private static Texture2D GetLastPhoto()
    {
        hasWebcam = WebCamTexture.devices.Length > 0;
        if (!hasWebcam)
        {
            Debug.Log("Please hook up a webcam.");
            return null;
        }

        if (Application.platform != RuntimePlatform.WindowsPlayer && Application.platform != RuntimePlatform.WindowsEditor)
        {
            Debug.Log("WebcamUtility only works on windows.");
            return null;
        }

        return currentPhoto;
    }

    public static Sprite TextureToSprite(Texture2D texture)
    {
        Sprite s = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        return s;
    }

    public static void OnPhotoTaken(Action<Texture2D> action, bool dispose = true)
    {
        hasWebcam = WebCamTexture.devices.Length > 0;
        if (!hasWebcam)
        {
            Debug.Log("Please hook up a webcam.");
            return;
        }

        if (Application.platform != RuntimePlatform.WindowsPlayer && Application.platform != RuntimePlatform.WindowsEditor)
        {
            Debug.Log("WebcamUtility only works on windows.");
            return;
        }

        if (dispose)
        {
            action += (Texture2D photo) =>
            {
                DisposePhoto();
            };
        }

        onPhotoTaken = action;
    }

    public static void DisposePhoto()
    {
        currentPhoto = null;
        currentPicture = null;
    }

}
