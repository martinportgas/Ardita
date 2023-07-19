using Microsoft.AspNetCore.Http;
using Net.Codecrete.QrCodeGenerator;
using QRCoder;
using System.Drawing;
using System.IO;
using System.Text;

namespace Ardita.Extensions;

public static class QRCodeExtension
{
    public static string Generate(string text, int size = 12)
    {
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.M);
        QRCode qrCode = new QRCode(qrCodeData);
        Bitmap qrCodeImage1 = qrCode.GetGraphic(size, Color.Black, Color.White, null, 15, 2);

        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", $"QRCode_{text}_{DateTime.Now.ToString("ddMMyyyyHHmmssfff")}.png");
        qrCodeImage1.Save(path);
        return path;
    }
    public static byte[] GenerateOld(string text)
    {
        var qr = QrCode.EncodeText(text, QrCode.Ecc.Medium);
        string svg = qr.ToSvgString(4);
        File.WriteAllText("QR.svg", svg, Encoding.UTF8);
        var bytes = File.ReadAllBytes("QR.svg");

        return bytes;

    }
}
