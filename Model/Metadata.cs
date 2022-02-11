using Syncfusion.Drawing;
using Syncfusion.Pdf.Graphics;

namespace SyncFusionPDFBurning.Model
{
    public class Metadata
    {
        public string Text { get; set; }
        public string FontFamily { set; get; }
        public string FontColor { set; get; }
        public float FontSize { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int PageNumber { get; set; }


        public PdfFontFamily GetFontFamily()
        {
            switch (this.FontFamily.ToLower())
            {
                case "helvetica":
                    return PdfFontFamily.Helvetica;
                case "courier":
                    return PdfFontFamily.Courier;
                case "symbol":
                    return PdfFontFamily.Symbol;
                case "zapfdingbats":
                    return PdfFontFamily.ZapfDingbats;
                default:
                    return PdfFontFamily.TimesRoman;
            }
        }

        public Color GetFontColor()
        {
            switch (this.FontColor.ToLower())
            {
                case "red": return Color.Red;
                case "blue": return Color.Blue;
                default: return Color.Black;
            }
        }


    }
}
