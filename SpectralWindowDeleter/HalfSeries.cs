using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;

namespace SpectralWindowDeleter
{
    public class HalfSeries : Series
    {
        public HalfSeries(int k)
        {
            this.Name = Convert.ToString(k);
            this.ChartType = SeriesChartType.Line;
            this.Color = Color.Black;
            this.BorderWidth = 1;
            this.BorderDashStyle = ChartDashStyle.Dash;
            this.IsVisibleInLegend = false;
            //if((k == 0)||(k == 1)||(k == 2))
            //{
            //    this.Enabled = true;
            //}
            //else
            //{
            //    this.Enabled = false;
            //}
        }
    }
}
