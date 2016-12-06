using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;

namespace SpectralWindowDeleter
{
    public class DataAnnotation : TextAnnotation
    {
        
        public DataAnnotation(int k)
        {
            this.LineWidth = 2;
            this.IsMultiline = true;
            this.Bottom = 12;
            this.ForeColor = Color.Black;
            this.Name = Convert.ToString(k);
            this.SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Yes;
            this.SmartLabelStyle.IsMarkerOverlappingAllowed = true;
            this.SmartLabelStyle.MovingDirection = LabelAlignmentStyles.Center|LabelAlignmentStyles.Left| LabelAlignmentStyles.Right;
            this.SmartLabelStyle.MaxMovingDistance = 40;
            //this.SmartLabelStyle.MinMovingDistance = 50;
        }  
   
    }
}
