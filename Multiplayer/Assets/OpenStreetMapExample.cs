using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class OpenStreetMapExample : MonoBehaviour
{
    // Variables for specifying the map coordinates and zoom level
    public double latitude = 51.5074; // Default to London
    public double longitude = -0.1278;
    public int zoomLevel = 15; // Adjust the zoom level as needed

    // URL template for OpenStreetMap tile server
    private string urlTemplate = "https://a.tile.openstreetmap.org/{0}/{1}/{2}.png";

    void Start()
    {
        // Construct the URL for the map tile based on the coordinates and zoom level
        string url = string.Format(urlTemplate, zoomLevel, LatitudeToTileY(latitude, zoomLevel), LongitudeToTileX(longitude, zoomLevel));

        // Start a coroutine to download the map tile
        StartCoroutine(DownloadMapTile(url));
    }

    IEnumerator DownloadMapTile(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
        {
            // Send the request and wait for a response
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                // Get the downloaded texture
                Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);

                // Display the texture on a GameObject (e.g., a plane)
                Renderer renderer = GetComponent<Renderer>();
                renderer.material.mainTexture = texture;
            }
            else
            {
                Debug.Log("Failed to download the map tile. Error: " + webRequest.error);
            }
        }
    }

    // Convert latitude to Y tile coordinate
    int LatitudeToTileY(double latitude, int zoomLevel)
    {
        return (int)((1 - Mathf.Log(Mathf.Tan((float)(latitude * Mathf.PI / 180) + Mathf.PI / 2)) / Mathf.PI) / 2 * (1 << zoomLevel));
    }

    // Convert longitude to X tile coordinate
    int LongitudeToTileX(double longitude, int zoomLevel)
    {
        return (int)((longitude + 180.0) / 360.0 * (1 << zoomLevel));
    }
}
