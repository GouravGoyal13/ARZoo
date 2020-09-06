using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.QrCode;
using UnityEngine.UI;

public class QRGenerator : MonoBehaviour
{
    [SerializeField] private RawImage QRTexture;
    [SerializeField] private InputField questId;
    private static Color32[] Encode(string textForEncoding, int width, int height)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }

    public Texture2D generateQR(string text)
    {
        var encoded = new Texture2D(256, 256);
        var color32 = Encode(text, encoded.width, encoded.height);
        encoded.SetPixels32(color32);
        encoded.Apply();
        return encoded;
    }

    public void OnGenerateQRButton()
    {
        if (!string.IsNullOrEmpty(questId.text))
        {
            Texture2D myQR = generateQR(questId.text);
            QRTexture.texture = myQR;
        }
    }
}
