# 矢量图标与渐变背景
在Unity 的新版ui系统uitoolkie中通过自定义VisualElement，利用Painter2D绘制矢量图形并实现线性渐变色。
效果如下:
  ![图片](Caption%20Image/示例.png)
## 使用说明
在Unity 6版本中使用只需将项目中的三个脚本文件拖入即可，在Unity 2022版本中需要修改部分代码，已知Unity 6中使用特性简化了自定义元素的声明。
* DrawSvgPathByPainter.cs中实现了将Svg路径转为Painter2D方法
* SVGElement.cs 继承自VisualElement,声明了Svg图标的绘制相关属性，并在视觉元属重绘回调中调用相应方法进行绘制，属性如下:
  * 复制对应Svg图标的path字符串,粘贴至svg_path属性中即可([免费可商用Svg图标网站 remixicon](https://remixicon.com/))
  * ![](Caption%20Image/Svg_Path.png)
  * ![](Caption%20Image/SvgElement属性.png)
* GradientElement.cs 继承自VisualElement,利用Painter2D的描边渐变实现线性渐变
  * ![](Caption%20Image/GradientElement属性.png)
