﻿using MultigraphEditor.src.layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultigraphEditor.src.graph
{
    public interface INodeDrawable : IDrawable
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Diameter { get; set; }

        public (float, float) GetCoordinates();
        public (float, float) GetDrawingCoordinates();
        public void Draw(object sender, PaintEventArgs e, MGraphEditorNodeLayer l);
        public void DrawLabel(object sender, PaintEventArgs e, MGraphEditorNodeLayer l);

    }
}