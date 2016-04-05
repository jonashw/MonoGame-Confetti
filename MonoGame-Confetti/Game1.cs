using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame_Confetti
{
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private readonly MouseEvents _mouse;
        private Confetti _confetti;
        private ResetableArray<Vector2> _explosionPositions;
        private readonly EasyTimer _spawnTimer = new EasyTimer(TimeSpan.FromSeconds(0.05f));
        private readonly EasyTimer _explosionTimer = new EasyTimer(TimeSpan.FromSeconds(0.4f));
        private readonly EasyTimer _transitionTimer = new EasyTimer(TimeSpan.FromSeconds(5));
        private bool _spawningConfetti = false;
        private int _confettiRate = 3;

        public Game1()
        {
            Content.RootDirectory = "Content";
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferMultiSampling = true
            };
            _mouse = new MouseEvents(Window);
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            _mouse.OnLeftClick((x,y) => _confetti.Explode(20,new Vector2(x,y)));

            const int explosionCount = 5;
            _explosionPositions = new ResetableArray<Vector2>(
                Enumerable.Range(1, explosionCount).Select(i =>
                {
                    var c = i/(explosionCount + 1f);
                    return new Vector2(
                        c*GraphicsDevice.Viewport.Width,
                        (1-c)*GraphicsDevice.Viewport.Height);
                }).ToArray());
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var textures = new List<Color>
            {
                new Color( 90, 185, 235), //blue
                new Color(255, 104, 126), //red
                new Color(190, 135, 164), //purple
            }.Select(c => GeometricTextureFactory.Rectangle(GraphicsDevice, 10, 18, c))
                .ToList().AsReadOnly();

            _confetti = new Confetti(textures);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            if (_transitionTimer.IsFinished(gameTime))
            {
                _spawningConfetti = !_spawningConfetti;
                _transitionTimer.Reset(gameTime);
                _explosionPositions.Reset();
                _explosionTimer.Reset(gameTime);
                _spawnTimer.Reset(gameTime);
            }

            _mouse.Update(gameTime);

            if (_spawningConfetti)
            {
                if(_spawnTimer.IsFinished(gameTime))
                {
                    _confetti.Sprinkle(_confettiRate, new Rectangle(0, -10, GraphicsDevice.Viewport.Width, 10));
                    _spawnTimer.Reset(gameTime);
                }
            }
            else
            {
                if (_explosionTimer.IsFinished(gameTime))
                {
                    _explosionPositions.TryDoWithCurrent(p => _confetti.Explode(20,p));
                    _explosionTimer.Reset(gameTime);
                }
            }
            _confetti.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            _spriteBatch.Begin();
            _confetti.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}