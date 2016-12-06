using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;

namespace SpectralWindowDeleter
{
    public class HorizontalStripline : StripLine
    {
        SpectralMeasurement figure = new SpectralMeasurement();
      

        public HorizontalStripline(int i)
        {
            this.Interval = 0;
            this.IntervalOffset = 0.1*i;
            this.StripWidth = 0.001;
            this.Tag = i;
            this.BackColor = Color.LightGray;
            this.BackHatchStyle = ChartHatchStyle.DarkVertical;
            
        }
    }
}
