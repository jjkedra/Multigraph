﻿using MultigraphEditor.src.layers;

namespace MultigraphEditor.src.graph
{
    public interface INodeDrawable : IDrawable
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Diameter { get; set; }
        public static int NodeCounter;
        public (float, float) GetCoordinates();
        public (float, float) GetDrawingCoordinates();
        public void Draw(Graphics g, INodeLayer l);
        public void DrawLabel(Graphics g, INodeLayer l);
    }
}
