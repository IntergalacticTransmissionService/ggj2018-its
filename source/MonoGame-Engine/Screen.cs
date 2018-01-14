using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_Engine
{
    public class Screen
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch screenBatch;
        private RenderTarget2D canvas;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int CanvasWidth { get; private set; }
        public int CanvasHeight { get; private set; }

        public Screen(GraphicsDeviceManager graphics, int width = 1280, int height = 720)
        {
            this.graphics = graphics;
            this.Width = (int)(width * 0.5f);
            this.Height = (int)(height * 0.5f);
            this.CanvasWidth = width;
            this.CanvasHeight = height;
        }

        public void Initialize()
        {
            this.screenBatch = new SpriteBatch(graphics.GraphicsDevice);
            this.canvas = new RenderTarget2D(graphics.GraphicsDevice, CanvasWidth, CanvasHeight);

            graphics.PreferredBackBufferWidth = Width;
            graphics.PreferredBackBufferHeight = Height;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
        }

        public void PreDraw()
        {
            graphics.GraphicsDevice.SetRenderTarget(canvas);
        }

        public void PostDraw()
        {
            graphics.GraphicsDevice.SetRenderTarget(null);
            screenBatch.Begin();
            screenBatch.Draw(canvas, new Rectangle(0, 0,
                graphics.GraphicsDevice.PresentationParameters.BackBufferWidth,
                graphics.GraphicsDevice.PresentationParameters.BackBufferHeight), Color.White);
            screenBatch.End();
        }

        public void SetSize(int width, int height, bool resizeCanvas = true)
        {
            Width = width; Height = height;
            graphics.PreferredBackBufferWidth = Width;
            graphics.PreferredBackBufferHeight = Height;
            graphics.ApplyChanges();

            if (resizeCanvas)
                SetCanvasSize(width, height);
        }

        public void SetCanvasSize(int width, int height)
        {
            CanvasWidth = width; CanvasHeight = height;
            var newCanvas = new RenderTarget2D(graphics.GraphicsDevice, CanvasWidth, CanvasHeight);
            canvas.Dispose();
            canvas = newCanvas;
        }

        public void ToggleFullscreen()
        {
            graphics.PreferredBackBufferWidth = graphics.IsFullScreen ? Width : graphics.GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = graphics.IsFullScreen ? Height : graphics.GraphicsDevice.DisplayMode.Height;
            graphics.ToggleFullScreen();
        }
    }
}
