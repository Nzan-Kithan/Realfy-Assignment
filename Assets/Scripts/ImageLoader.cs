using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageLoader
{
    private const string ImageUrlTemplate = "http://139.59.39.150:8055/assets/{0}";
    private static Sprite missingImage;

    public static void SetDefaultImage(Sprite image)
    {
        missingImage = image;
    }

    public IEnumerator SetImageFromId(Image image, string imageId)
    {
        if (string.IsNullOrEmpty(imageId))
        {
            Debug.LogWarning("Image ID is null or empty.");
            SetDefaultImage(image);
            yield break;
        }

        string url = string.Format(ImageUrlTemplate, imageId);

        using (UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                Texture2D texture = UnityEngine.Networking.DownloadHandlerTexture.GetContent(request);
                image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
            else
            {
                Debug.LogError($"Failed to load image from {url}: {request.error}");

                if (missingImage != null)
                {
                    image.sprite = missingImage;
                }
                else
                {
                    Debug.LogWarning("Default image not set.");
                }
            }
        }
    }

    private void SetDefaultImage(Image image)
    {
        if (missingImage != null)
        {
            image.sprite = missingImage;
        }
        else
        {
            Debug.LogWarning("Default image not set.");
        }
    }
}
