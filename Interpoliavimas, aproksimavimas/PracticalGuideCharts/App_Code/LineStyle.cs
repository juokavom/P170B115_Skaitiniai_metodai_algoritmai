﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalGuideCharts
{
    public class LineStyle
    {
        private DashStyle linePattern = DashStyle.Solid;
        private Color lineColor = Color.Black;
        private float LineThickness = 1.0f;
        private bool isVisible = true;
        public LineStyle()
        {
        }
        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }
        virtual public DashStyle Pattern
        {
            get { return linePattern; }
            set { linePattern = value; }
        }
        public float Thickness
        {
            get { return LineThickness; }
            set { LineThickness = value; }
        }
        virtual public Color LineColor
        {
            get { return lineColor; }
            set { lineColor = value; }
        }
    }
}
