using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGame_Confetti
{
    public class MouseEvents
    {
        private readonly GameWindow _window;
        private readonly MouseButtonEvents _right = new MouseButtonEvents();
        private readonly MouseButtonEvents _left = new MouseButtonEvents();
        private readonly MouseButtonEvents _middle = new MouseButtonEvents();
        private readonly MouseScrollEvents _scroll = new MouseScrollEvents();

        public MouseEvents(GameWindow window)
        {
            _window = window;
        }

        public void Update(GameTime gameTime)
        {
            var state = Mouse.GetState(_window);
            _right.Update(state.RightButton, state.X, state.Y);
            _left.Update(state.LeftButton, state.X, state.Y);
            _middle.Update(state.MiddleButton, state.X, state.Y);
            _scroll.Update(state.ScrollWheelValue);
        }

        public void OnScroll(Action<bool> handler)
        {
            _scroll.OnChange(handler);
        }

        public void OnLeftClick(Action<int, int> handler)
        {
            _left.OnClick(handler);
        }

        public void OnRightClick(Action<int, int> handler)
        {
            _right.OnClick(handler);
        }

        public void OnMiddleClick(Action<int, int> handler)
        {
            _middle.OnClick(handler);
        }

        private class MouseButtonEvents
        {
            private ButtonState _lastState = ButtonState.Released;
            private readonly List<Action<int, int>> _handlers = new List<Action<int, int>>();

            public void Update(ButtonState nextState, int x, int y)
            {
                if (_lastState == ButtonState.Released)
                {
                    if (nextState != ButtonState.Pressed)
                    {
                        return;
                    }
                    foreach (var h in _handlers)
                    {
                        h(x, y);
                    }
                    _lastState = ButtonState.Pressed;
                }
                else if (nextState == ButtonState.Released)
                {
                    _lastState = ButtonState.Released;
                }
            }

            public void OnClick(Action<int, int> handler)
            {
                _handlers.Add(handler);
            }
        }

        private class MouseScrollEvents
        {
            private readonly List<Action<bool>> _handlers = new List<Action<bool>>();
            private int _lastScrollWheelValue;
            public void Update(int scrollWheelValue)
            {
                if (_lastScrollWheelValue == scrollWheelValue)
                {
                    return;
                }
                var scrollPositive = _lastScrollWheelValue > scrollWheelValue;
                foreach (var handler in _handlers)
                {
                    handler(scrollPositive);
                }
                _lastScrollWheelValue = scrollWheelValue;
            }

            public void OnChange(Action<bool> handler)
            {
                _handlers.Add(handler);
            }
        }
    }
}