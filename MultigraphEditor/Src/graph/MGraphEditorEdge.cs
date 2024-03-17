﻿using MultigraphEditor.Forms;
using MultigraphEditor.src.layers;
using MultigraphEditor.Src.graph;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultigraphEditor.src.graph
{
    public class MGraphEditorEdge: IMGraphEditorEdge
    {
        [ExcludeFromForm]
        public int Identifier { get; set; }
        public string Label { get; set; }
        [ExcludeFromForm]
        public INode Source { get; set; }
        [ExcludeFromForm]
        public INode Target { get; set; }
        public bool Bidirectional { get; set; }
        public int Weight { get; set; }
        [ExcludeFromForm]
        public required INodeDrawable SourceDrawable { get; set; }
        [ExcludeFromForm]
        public required INodeDrawable TargetDrawable { get; set; }
        [ExcludeFromForm]
        public required float controlPointX { get; set; }
        [ExcludeFromForm]
        public required float controlPointY { get; set; }

        public static int EdgeCounter = 0;

        public MGraphEditorEdge()
        {
            Identifier = GetIdentifier();
        }

        public int GetIdentifier()
        {
            return EdgeCounter++;
        }
        public void PopulateDrawing(INodeDrawable srcDrw, INodeDrawable tgtDrw)
        {
            SourceDrawable = srcDrw;
            TargetDrawable = tgtDrw;
            controlPointX = (srcDrw.X + tgtDrw.X) / 2;
            controlPointY = (srcDrw.Y + tgtDrw.Y) / 2;
        }

        public void PopulateNode(INode src, INode tgt, bool bidir, int w)
        {
            Source = src;
            Target = tgt;
            Bidirectional = bidir;
            Weight = w;
        }

        public void Draw(Graphics g, IEdgeLayer l)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Pen pen = new Pen(l.Color, l.Width);
            Brush brush = new SolidBrush(l.Color);
            if (SourceDrawable != null && TargetDrawable != null)
            {
                if (SourceDrawable != TargetDrawable)
                {
                    float dx = TargetDrawable.X - SourceDrawable.X;
                    float dy = TargetDrawable.Y - SourceDrawable.Y;
                    float length = (float)Math.Sqrt(dx * dx + dy * dy);
                    float unitDx = dx / length;
                    float unitDy = dy / length;
                    float sourceRadius = SourceDrawable.Diameter / 2;
                    float targetRadius = TargetDrawable.Diameter / 2;
                    float sourceX = SourceDrawable.X + sourceRadius * unitDx;
                    float sourceY = SourceDrawable.Y + sourceRadius * unitDy;
                    float targetX = TargetDrawable.X - targetRadius * unitDx;
                    float targetY = TargetDrawable.Y - targetRadius * unitDy;

                    controlPointX = (sourceX + targetX) / 2;
                    controlPointY = (sourceY + targetY) / 2;

                    // Draw control point
                    g.FillEllipse(brush, controlPointX - 5, controlPointY - 5, 10, 10);

                    // Draw the edge
                    g.DrawLine(pen, sourceX, sourceY, targetX, targetY);
                }
                else
                {
                    throw new NotImplementedException();
                }
                DrawArrow(g, l);
                DrawLabel(g, l);
            }
        }

        public void DrawArrow(Graphics g, IEdgeLayer l)
        {
            float dx = TargetDrawable.X - SourceDrawable.X;
            float dy = TargetDrawable.Y - SourceDrawable.Y;
            float length = (float)Math.Sqrt(dx * dx + dy * dy);
            float unitDx = dx / length;
            float unitDy = dy / length;
            float sourceRadius = SourceDrawable.Diameter / 2;
            float targetRadius = TargetDrawable.Diameter / 2;

            // Calculate the coords of arrow tip
            int arrowSize = l.arrowSize;

            float arrowHeadX = TargetDrawable.X - (unitDx * targetRadius);
            float arrowHeadY = TargetDrawable.Y - (unitDy * targetRadius);
            float arrowHeadAngle = (float)Math.Atan2(dy, dx);

            using (SolidBrush brush = new SolidBrush(l.Color))
            {
                PointF[] points = new PointF[3];
                points[0] = new PointF(arrowHeadX, arrowHeadY);
                points[1] = new PointF(arrowHeadX - (arrowSize * (float)Math.Cos(arrowHeadAngle - (Math.PI / 6))), arrowHeadY - (arrowSize * (float)Math.Sin(arrowHeadAngle - (Math.PI / 6))));
                points[2] = new PointF(arrowHeadX - (arrowSize * (float)Math.Cos(arrowHeadAngle + (Math.PI / 6))), arrowHeadY - (arrowSize * (float)Math.Sin(arrowHeadAngle + (Math.PI / 6))));
                g.FillPolygon(brush, points);
            }

            if (Bidirectional)
            {
                float arrowHeadX2 = SourceDrawable.X + (unitDx * sourceRadius);
                float arrowHeadY2 = SourceDrawable.Y + (unitDy * sourceRadius);
                float arrowHeadAngle2 = (float)Math.Atan2(-dy, -dx);

                using (SolidBrush brush = new SolidBrush(l.Color))
                {
                    PointF[] points = new PointF[3];
                    points[0] = new PointF(arrowHeadX2, arrowHeadY2);
                    points[1] = new PointF(arrowHeadX2 - (arrowSize * (float)Math.Cos(arrowHeadAngle2 - (Math.PI / 6))), arrowHeadY2 - (arrowSize * (float)Math.Sin(arrowHeadAngle2 - (Math.PI / 6))));
                    points[2] = new PointF(arrowHeadX2 - (arrowSize * (float)Math.Cos(arrowHeadAngle2 + (Math.PI / 6))), arrowHeadY2 - (arrowSize * (float)Math.Sin(arrowHeadAngle2 + (Math.PI / 6))));
                    g.FillPolygon(brush, points);
                }
            }
        }

        public void DrawLabel(Graphics g, IEdgeLayer l)
        {
            float dx = TargetDrawable.X - SourceDrawable.X;
            float dy = TargetDrawable.Y - SourceDrawable.Y;
            float length = (float)Math.Sqrt(dx * dx + dy * dy);
            float unitDx = dx / length;
            float unitDy = dy / length;
            float sourceRadius = SourceDrawable.Diameter / 2;
            float targetRadius = TargetDrawable.Diameter / 2;
            float sourceX = SourceDrawable.X + sourceRadius * unitDx;
            float sourceY = SourceDrawable.Y + sourceRadius * unitDy;
            float targetX = TargetDrawable.X - targetRadius * unitDx;
            float targetY = TargetDrawable.Y - targetRadius * unitDy;

            float labelX = (sourceX + targetX) / 2;
            float labelY = (sourceY + targetY) / 2 - 5;
            if (!string.IsNullOrEmpty(Label))
            {
                labelY -= 10;
            }

            using (SolidBrush brush = new SolidBrush(l.Color))
            {
                SizeF textSize = g.MeasureString(Label + "\n" + Weight.ToString(), l.Font);
                PointF labelPosition = new PointF(labelX - textSize.Width / 2, labelY - textSize.Height / 2);

                g.DrawString(Weight.ToString() + "\n" + Label, l.Font, brush, labelPosition);
            }
        }

        public bool IsInside(float x, float y)
        {
            float tolerance = 5f;
            float lineDistance = PointToLineDistance(x, y);
            bool isInsideEllipse = IsPointInsideEllipse(x, y, SourceDrawable.X + (SourceDrawable.Diameter / 4) + SourceDrawable.Diameter, SourceDrawable.Y + SourceDrawable.Diameter, SourceDrawable.Diameter, SourceDrawable.Diameter);

            if (lineDistance <= tolerance || isInsideEllipse)
            {
                return true; // Point is inside the line connecting two nodes
            }

            return false; // Point is not inside any line
        }

        public bool IsInsideControlPoint(float x, float y)
        {
            return IsPointInsideEllipse(x, y, controlPointX, controlPointY, 10, 10);
        }

        public bool IsPointInsideEllipse(float x, float y, float centerX, float centerY, float width, float height)
        {
            // Calculate normalized coordinates
            float normalizedX = (x - centerX) / (width / 2);
            float normalizedY = (y - centerY) / (height / 2);

            // Check if the point is inside the ellipse equation (x^2 / a^2) + (y^2 / b^2) <= 1
            return (normalizedX * normalizedX) + (normalizedY * normalizedY) <= 1;
        }

        private float PointToLineDistance(float x, float y)
        {
            float dx = TargetDrawable.X - SourceDrawable.X;
            float dy = TargetDrawable.Y - SourceDrawable.Y;
            float length = (float)Math.Sqrt(dx * dx + dy * dy);
            float unitDx = dx / length;
            float unitDy = dy / length;
            float sourceRadius = SourceDrawable.Diameter / 2;
            float targetRadius = TargetDrawable.Diameter / 2;
            float sourceX = SourceDrawable.X + sourceRadius * unitDx;
            float sourceY = SourceDrawable.Y + sourceRadius * unitDy;
            float targetX = TargetDrawable.X - targetRadius * unitDx;
            float targetY = TargetDrawable.Y - targetRadius * unitDy;

            float lineX1 = sourceX;
            float lineY1 = sourceY;
            float lineX2 = targetX;
            float lineY2 = targetY;

            float A = x - lineX1;
            float B = y - lineY1;
            float C = lineX2 - lineX1;
            float D = lineY2 - lineY1;

            float dot = (A * C) + (B * D);
            float len_sq = (C * C) + (D * D);
            float param = dot / len_sq;

            float nearestX, nearestY;

            if (param < 0)
            {
                nearestX = lineX1;
                nearestY = lineY1;
            }
            else if (param > 1)
            {
                nearestX = lineX2;
                nearestY = lineY2;
            }
            else
            {
                nearestX = lineX1 + (param * C);
                nearestY = lineY1 + (param * D);
            }

            float dxx = x - nearestX;
            float dyy = y - nearestY;

            return (float)Math.Sqrt((dxx * dxx) + (dyy * dyy));
        }
    }
}
