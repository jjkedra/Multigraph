﻿namespace MultigraphEditor.src.layers
{
    public interface ILayer
    {
        Font Font { get; set; }
        Color Color { get; set; }
        int nodeWidth { get; set; }
        int edgeWidth { get; set; }
        bool Active { get; set; }
        int Identifier { get; set; }
        String Name { get; set; }
        void changeActive();
    }
}
