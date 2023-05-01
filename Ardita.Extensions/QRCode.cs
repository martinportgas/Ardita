using Net.Codecrete.QrCodeGenerator;
using System.Text;

namespace Ardita.Extensions;

public static class QRCodeExtension
{
    public static byte[] Generate(string text)
    {
        var qr = QrCode.EncodeText(text, QrCode.Ecc.Medium);
        string svg = qr.ToSvgString(4);
        File.WriteAllText("QR.svg", svg, Encoding.UTF8);
        var bytes = File.ReadAllBytes("QR.svg");

        return bytes;

    }
}
