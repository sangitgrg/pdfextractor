using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class TextLocationStrategy : LocationTextExtractionStrategy
{
    public List<textChunk> objectResult = new List<textChunk>();

    public override void EventOccurred(IEventData data, EventType type)
    {
        if (!type.Equals(EventType.RENDER_TEXT))
            return;

        TextRenderInfo renderInfo = (TextRenderInfo)data;

        string curFont = renderInfo.GetFont().GetFontProgram().ToString();

        float curFontSize = renderInfo.GetFontSize();

        IList<TextRenderInfo> text = renderInfo.GetCharacterRenderInfos();
        foreach (TextRenderInfo t in text)
        {
            string letter = t.GetText();
            Vector letterStart = t.GetBaseline().GetStartPoint();
            Vector letterEnd = t.GetAscentLine().GetEndPoint();
            Rectangle letterRect = new Rectangle(letterStart.Get(0), letterStart.Get(1), letterEnd.Get(0) - letterStart.Get(0), letterEnd.Get(1) - letterStart.Get(1));

            if (letter != " " && !letter.Contains(' '))
            {
                textChunk chunk = new textChunk();
                chunk.Text = letter;
                chunk.Rect = letterRect;
                chunk.FontFamily = curFont;
                chunk.FontSize = curFontSize;
                chunk.SpaceWidth = t.GetSingleSpaceWidth() / 2f;

                objectResult.Add(chunk);
            }
        }
    }
}
public class textChunk
{
    public string Text { get; set; }
    public Rectangle Rect { get; set; }
    public string FontFamily { get; set; }
    public float FontSize { get; set; }
    public float SpaceWidth { get; set; }
}