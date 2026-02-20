using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutDiary.Helper
{
    public class OctagonBackground : IDrawable
    {
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            // Białe tło
            canvas.FillColor = Colors.White;
            canvas.FillRectangle(dirtyRect);

            // Kolor kropek
            canvas.FillColor = Colors.Black.WithAlpha(0.15f);

            float spacing = 40; // odstęp między kropkami
            float radius = 3;   // promień kropki

            for (float y = 0; y < dirtyRect.Height; y += spacing)
            {
                for (float x = 0; x < dirtyRect.Width; x += spacing)
                {
                    canvas.FillCircle(x, y, radius);
                }
            }
        }


    }
}
