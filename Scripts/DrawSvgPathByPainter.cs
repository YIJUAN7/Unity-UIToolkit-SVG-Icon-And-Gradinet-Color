using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;

public static partial class Tool
{
    public static void DrawSvgPathByPainter(string svgPath, Painter2D painter, Vector2 target_size,
        Vector2 original_size, bool isStroke, bool isFill, float scale = 1f)
    {
        try
        {
            //按字母和空格分割得到顺序参数例表
            //再获取命令例表，根据命令的固定参数数量读取参数
            var paramterStr = Regex.Split(svgPath, @"[a-zA-Z\s]+");
            var commands = Regex.Matches(svgPath, @"[a-zA-Z]");
            var paramters = new Queue<float>();
            foreach (var paramter in paramterStr)
            {
                if (string.IsNullOrEmpty(paramter)) continue;
                paramters.Enqueue(float.Parse(paramter));
            }
            ExcuteCommand(commands, paramters, painter, target_size, original_size, scale);
            if (isStroke)
                painter.Stroke();
            if (isFill)
                painter.Fill();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private static void ExcuteCommand(MatchCollection commands, Queue<float> paramters, Painter2D painter,
        Vector2 target_size, Vector2 original_size, float center_scale)
    {
        Vector2 startPoint = Vector2.zero;
        Vector2 currentPoint = Vector2.zero;
        Vector2 p = Vector2.zero;
        var scale = target_size / original_size;
        Vector2 center = new Vector2(12f, 12f) * scale;
        // (point - center) * center_scale + center
        // Func<Vector2> point = () => { return new Vector2(paramters.Dequeue(), paramters.Dequeue())  * scale; };
        Func<int,float> get_coordinate = (int axis) =>
        {
            var coordinate = paramters.Dequeue() * scale[axis];
            return (coordinate - center[axis]) * center_scale + center[axis];
        };
        Func<Vector2> point = () => new Vector2(get_coordinate(0), get_coordinate(1)) ;
        painter.BeginPath();
        foreach (Match c in commands)
        {
            var command = c.Value;
            switch (command)
            {
                case "M":
                    p = point();
                    painter.MoveTo(p);
                    currentPoint = p;
                    startPoint = currentPoint;
                    break;
                case "L":
                    p = point();
                    painter.LineTo(p);
                    currentPoint = p;
                    break;
                case "H":
                    // p = new Vector2(paramters.Dequeue() * scale.x, currentPoint.y);
                    p = new Vector2(get_coordinate(0), currentPoint.y);
                    painter.LineTo(p);
                    currentPoint = p;
                    break;
                case "V":
                    // p = new Vector2(currentPoint.x, paramters.Dequeue() * scale.y);
                    p = new Vector2(currentPoint.x, get_coordinate(1));
                    painter.LineTo(p);
                    currentPoint = p;
                    break;
                case "C":
                    painter.BezierCurveTo(point(), point(), p = point());
                    currentPoint = p;
                    break;
                case "S":
                    p = point();
                    painter.BezierCurveTo(p, p, p = point());
                    currentPoint = p;
                    break;
                case "Q":
                    painter.QuadraticCurveTo(point(), p = point());
                    currentPoint = p;
                    break;
                case "A":
                    Debug.LogWarning(@"Arc command not supported yet.Svg的曲线命令与Unity的画笔API不匹配，无法实现。
                请手动实现或使用贝塞尔曲线或其他曲线命令替代。");
                    //rx ry x-axis-rotation large-arc-flag sweep-flag dx dy
                    //只获取终点坐标，以继续绘制其他路径
                    for (int i = 0; i < 5; i++)
                    {
                        paramters.Dequeue();
                    }

                    painter.MoveTo(p = point());
                    currentPoint = p;
                    break;
                case "Z":
                    painter.ClosePath();
                    currentPoint = startPoint;
                    break;
                default:
                    Debug.LogWarning($"Command {command} not supported yet.");
                    break;
            }
        }
    }
}