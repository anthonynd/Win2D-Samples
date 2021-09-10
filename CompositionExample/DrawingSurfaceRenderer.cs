// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Composition;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.DirectX;
using Windows.UI;
using Windows.UI.Composition;

namespace CompositionExample
{
    class DrawingSurfaceRenderer
    {
        SpriteVisual drawingSurfaceVisual;
        CompositionDrawingSurface drawingSurface;

        int drawCount;

        public Visual Visual { get { return drawingSurfaceVisual; } }

        public Size Size { get { return drawingSurface.Size; } }

        public DrawingSurfaceRenderer(Compositor compositor, CompositionGraphicsDevice compositionGraphicsDevice)
        {
            drawingSurfaceVisual = compositor.CreateSpriteVisual();
            drawingSurface = compositionGraphicsDevice.CreateDrawingSurface(new Size(256, 256), DirectXPixelFormat.B8G8R8A8UIntNormalized, DirectXAlphaMode.Premultiplied);
            drawingSurfaceVisual.Brush = compositor.CreateSurfaceBrush(drawingSurface);
            DrawDrawingSurface();

            compositionGraphicsDevice.RenderingDeviceReplaced += CompositionGraphicsDevice_RenderingDeviceReplaced;
        }

        void CompositionGraphicsDevice_RenderingDeviceReplaced(CompositionGraphicsDevice sender, RenderingDeviceReplacedEventArgs args)
        {
            DrawDrawingSurface();
        }

        async void DrawDrawingSurface()
        {
            ++drawCount;

            using (var ds = CanvasComposition.CreateDrawingSession(drawingSurface))
            {
                ds.Clear(Colors.Transparent);
                var rect = new Rect(new Point(0, 0), (drawingSurface.Size.ToVector2() - new Vector2(20, 20)).ToSize());
                ds.FillRoundedRectangle(rect, 30, 30, Colors.LightBlue);
                var image = await CanvasBitmap.LoadAsync(ds, "dog.jpg");
                ds.DrawImage(image, 0, 0, rect, 1f, CanvasImageInterpolation.Linear, CanvasComposite.SourceIn);
            }
        }
    }
}
