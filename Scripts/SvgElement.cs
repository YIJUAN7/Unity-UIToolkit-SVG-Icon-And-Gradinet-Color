using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class SvgElement : VisualElement
{
    [Header("SVG Setting")] [UxmlAttribute]
    public string svg_path =
        "M5.46257 4.43262C7.21556 2.91688 9.5007 2 12 2C17.5228 2 22 6.47715 22 12C22 14.1361 21.3302 16.1158 20.1892 17.7406L17 12H20C20 7.58172 16.4183 4 12 4C9.84982 4 7.89777 4.84827 6.46023 6.22842L5.46257 4.43262ZM18.5374 19.5674C16.7844 21.0831 14.4993 22 12 22C6.47715 22 2 17.5228 2 12C2 9.86386 2.66979 7.88416 3.8108 6.25944L7 12H4C4 16.4183 7.58172 20 12 20C14.1502 20 16.1022 19.1517 17.5398 17.7716L18.5374 19.5674Z";

    [UxmlAttribute] public Color svg_color = Color.black;

    // [UxmlAttribute] public float target_size = 24f;
    [UxmlAttribute] public Vector2 original_size = new(24f, 24f);
    
    [UxmlAttribute] public float scale = 1f;
    [UxmlAttribute] public bool svg_fill = true;
    [UxmlAttribute] public bool svg_stroke = false;
    [UxmlAttribute] public Gradient gradient;
    [UxmlAttribute] public float stroke_width = 2f;

    public SvgElement()
    {
        generateVisualContent += OnGenerateVisualContent;
    }

    private void OnGenerateVisualContent(MeshGenerationContext context)
    {
        var painter = context.painter2D;
        painter.fillColor = svg_color;
        painter.strokeColor = Color.white;//默认描边色
        painter.lineWidth = stroke_width;
        painter.strokeGradient = gradient;
        // Debug.Log($"{contentRect.width}, {contentRect.height}");
        Tool.DrawSvgPathByPainter(svg_path, painter, new (contentRect.width, contentRect.height), original_size, svg_stroke, svg_fill, scale);
    }
}