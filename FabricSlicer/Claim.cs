using System;
using System.Collections.Generic;
using System.Text;

namespace FabricSlicer
{
    public class Claim
    {
        public string Id { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Tuple<int, int> TopLeft
        {
            get
            {
                return new Tuple<int, int>(Left, Top);
            }
        }
        public Tuple<int, int> BottomRight
        {
            get
            {
                return new Tuple<int, int>(Left + Width - 1, Top + Height - 1);
            }
        }
    }
}
