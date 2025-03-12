using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

[UxmlElement]
public partial class GradientElement : VisualElement
{
    [Header("SVG Setting")]
    [UxmlAttribute,Tooltip("50%到100%为实际显示区域")]
    public Gradient gradient;
    [UxmlAttribute] public Direction direction;
    private string[] paths = new string[] { "M0 12H24Z", "M12 0V24Z" };

public enum Direction
    {
        Horizontal,
        Vertical
    }
    public GradientElement()
    {
        generateVisualContent += OnGenerateVisualContent;
    }

    private void OnGenerateVisualContent(MeshGenerationContext context)
    {
        var painter = context.painter2D;
        string path;
        if (direction == Direction.Horizontal)
        {
            painter.lineWidth = contentRect.height;
            path = paths[0];
        }
        else
        {
            painter.lineWidth = contentRect.width;
            path = paths[1];
        }
        
        painter.strokeGradient = gradient;
        // Debug.Log($"{contentRect.width}, {contentRect.height}");
         Tool.DrawSvgPathByPainter(path, painter, contentRect.size, new Vector2(24f, 24f), true, false);
    }
}
