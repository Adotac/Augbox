using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;

public class DisplayHandTracking : MonoBehaviour
{
    public string serverIP = "127.0.0.1";
    public int serverPort = 5053;

    private bool receivingData = false;
    private NetworkStream networkStream;
    private Texture2D currentTexture;
    private Texture2D lastTexture;
    private byte[] imageBytes;
    private byte[] sizeBytes;

    private void Start()
    {
        Texture.allowThreadedTextureCreation = true;
        currentTexture = new Texture2D(1, 1);
        lastTexture = new Texture2D(1, 1);
        ConnectToServer();
    }

    private void Update()
    {
        if (!receivingData && networkStream == null)
        {
            // Try to reconnect to the server if not receiving data and no network stream
            ConnectToServer();
        }

        if (receivingData && networkStream != null)
        {
            // Continue receiving data
            ReceiveData();
        }
    }

    private void ConnectToServer()
    {
        try
        {
            TcpClient tcpClient = new TcpClient(serverIP, serverPort);
            networkStream = tcpClient.GetStream();

            receivingData = true;
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed to connect to the server: " + e.Message);
            networkStream = null;
        }
    }

    private void ReceiveData()
    {
        if (networkStream.DataAvailable)
        {
            sizeBytes = new byte[4];
            int bytesRead = networkStream.Read(sizeBytes, 0, sizeBytes.Length);
            // print($"sizeBytes: {bytesRead}");
            if (bytesRead == 0)
            {
                // Connection closed by the server
                Debug.LogWarning("Connection closed by the server.");
                receivingData = false;
                networkStream.Close();
                networkStream = null;
                return;
            }

            int imageSize = BitConverter.ToInt32(sizeBytes, 0);

            imageBytes = new byte[imageSize];
            bytesRead = networkStream.Read(imageBytes, 0, imageBytes.Length);
            // print($"imageBytes: {bytesRead}");
            if (bytesRead == 0)
            {
                // Connection closed by the server
                Debug.LogWarning("Connection closed by the server.");
                receivingData = false;
                networkStream.Close();
                networkStream = null;
                return;
            }

            // Create a texture from the received image data
            currentTexture.LoadImage(imageBytes);

            // Assign the current texture to the material
            if (TryGetComponent<Renderer>(out Renderer r))
                r.material.mainTexture = currentTexture;
            else if (TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
                sr.material.mainTexture = currentTexture;
            else if (TryGetComponent<Image>(out Image img))
                img.material.mainTexture = currentTexture;

            // Swap textures for the next iteration
            SwapTextures();

            // Clear the Bytes array
            Array.Clear(sizeBytes, 0, sizeBytes.Length);
            Array.Clear(imageBytes, 0, imageBytes.Length);

        }

    }

    private void SwapTextures()
    {
        Texture2D temp = currentTexture;
        currentTexture = lastTexture;
        lastTexture = temp;
        ApplyLastTexture();
    }

    private void ApplyLastTexture()
    {
        if (TryGetComponent<Renderer>(out Renderer r))
            r.material.mainTexture = lastTexture;
        else if (TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
            sr.material.mainTexture = lastTexture;
        else if (TryGetComponent<Image>(out Image img))
            img.material.mainTexture = lastTexture;
    }
}
