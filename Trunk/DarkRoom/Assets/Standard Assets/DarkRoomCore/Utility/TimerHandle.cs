using System;
using System.Collections.Generic;

namespace DarkRoom.Core
{
 public struct FTimerHandle
    {
        public bool IsValid()
        {
            return Handle != 0;
        }

        public void Invalidate()
        {
            Handle = 0;
        }

        public string ToString()
        {
            return "";
        }
        private int Handle;
    };
}
