using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

public class GetMethod
{
    private string apiUrl = "http://139.59.39.150:8055/items/item";

    // ItemList to store the fetched items
    public ItemList itemList { get; private set; } = new ItemList();

    // Method to fetch items from the API
    public IEnumerator FetchItems()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {request.error}");
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;

                // Parse the JSON response into the ItemList object
                itemList = JsonUtility.FromJson<ItemList>(jsonResponse);

                // Decode the HTML for the fetched data
                for (int i = 0; i < itemList.Count; i++)
                {
                    itemList.data[i].description = WebUtility.HtmlDecode(itemList.data[i].description);
                    itemList.data[i].description = RemoveHtmlTags(itemList.data[i].description);
                }

            }
        }
    }

    public static string RemoveHtmlTags(string html)
    {
        // Use regex to remove all HTML tags
        string cleanText = Regex.Replace(html, "<.*?>", string.Empty);
        return cleanText;
    }
}
