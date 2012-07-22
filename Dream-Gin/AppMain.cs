using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

using Sce.PlayStation.Core.Imaging;

namespace DreamGin
{
	public class AppMain
	{
		/*private static GraphicsContext graphics;
		
		public static void Main (string[] args)
		{
			Initialize ();

			while (true) {
				SystemEvents.CheckEvents ();
				Update ();
				Render ();
			}
		}

		public static void Initialize ()
		{
			// Set up the graphics system
			graphics = new GraphicsContext ();
		}

		public static void Update ()
		{
			// Query gamepad for current state
			var gamePadData = GamePad.GetData (0);
		}

		public static void Render ()
		{
			// Clear the screen
			graphics.SetClearColor (0.0f, 0.0f, 0.0f, 0.0f);
			graphics.Clear ();

			// Present the screen
			graphics.SwapBuffers ();
		}*/
		public static void Main (string[] args)
		{
			//JP: initialize GameEngine2D singletion that handles loop
			Director.Initialize();
			//JP: New 2D scene (graph) container (and camera alignment to orthogonal view
			Scene scene = new Scene();
			scene.Camera.SetViewFromViewport();
			//JP: Get width and height to local variables (optimization)
			var width = Director.Instance.GL.Context.GetViewport().Width;
			var height = Director.Instance.GL.Context.GetViewport().Height;
			//JP: Create new image
			Image img = new Image(ImageMode.Rgba, new ImageSize(width,height),
			                      new ImageColor(255,0,0,0));
			img.DrawText("Dream-Gin",
			             new ImageColor(255,0,0,255),
			             new Font(FontAlias.System,170,FontStyle.Regular),
			             new ImagePosition(0,150));
			//JP: Create hax0r frame buffer and add our img to it's buffer (and consequently kill img content)
			Texture2D texture = new Texture2D(width,height,false,
			                                  PixelFormat.Rgba);
			texture.SetPixels(0,img.ToBuffer());
			img.Dispose();                                  
			//JP: Texture info needed for SpriteUV
			TextureInfo ti = new TextureInfo();
			ti.Texture = texture;
			//JP: Add ti to SpriteUV
			SpriteUV sprite = new SpriteUV();
			sprite.TextureInfo = ti;
			//JP: resize sprite and center
			sprite.Quad.S = ti.TextureSizef;
			sprite.CenterSprite();
			sprite.Position = scene.Camera.CalcBounds().Center;
			//JP: Add to Scene
			scene.AddChild(sprite);
			//JP: execute loop shit
			Director.Instance.RunWithScene(scene, true);
			
			bool gameOver = false;
			
			while(!gameOver)
			{
				Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.Update();
				if(Input2.GamePad.GetData(0).Left.Release)
				{
					sprite.Rotate(Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.Deg2Rad(90));
				}
				if(Input2.GamePad0.Right.Release)
				{
					sprite.Rotate(Sce.PlayStation.HighLevel.GameEngine2D.Base.Math.Deg2Rad(-90));
				}
				if((Sce.PlayStation.Core.Input.GamePad.GetData(0).Buttons & GamePadButtons.Up) == GamePadButtons.Up)
				{
					sprite.Quad.S = new Vector2(sprite.Quad.S.X += 10.0f,sprite.Quad.S.Y += 10.0f);
					sprite.CenterSprite();
				}
				if((Sce.PlayStation.Core.Input.GamePad.GetData(0).Buttons & GamePadButtons.Down) == GamePadButtons.Down)
				{
					sprite.Quad.S = new Vector2(sprite.Quad.S.X -= 10.0f,sprite.Quad.S.Y -= 10.0f);
					sprite.CenterSprite();
				}
				if(Input2.GamePad0.Circle.Press == true)
					gameOver = true;
			    
				Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.Render();
				Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.GL.Context.SwapBuffers();
				Sce.PlayStation.HighLevel.GameEngine2D.Director.Instance.PostSwap();
			}
			
			Director.Terminate();
		}
	}
}