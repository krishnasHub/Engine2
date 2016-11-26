using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine2.Input
{
    public class InputEvent
    {
        public Key Key;
        public MouseButton MouseButton;

        public bool IsPressed, IsReleased, IsDown;

        public InputEvent()
        {
            IsPressed = false;
            IsReleased = false;
            IsDown = false;
        }

        public InputEvent(Key key)
        {
            this.Key = key;
            IsPressed = false;
            IsReleased = false;
            IsDown = false;
        }

    }
}
