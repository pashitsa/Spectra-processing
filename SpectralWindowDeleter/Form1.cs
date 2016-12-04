using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SpectralWindowDeleter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        SpectralMeasurement measurement = new SpectralMeasurement();
        int MouseCLickQuantity = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;
                //MessageBox.Show();
                measurement.LoadWindowTransmittance(fileName);
            }
        }

        public void LoadFilterButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;
                measurement.LoadFilterTransmittance(fileName);

                //Делаем активными все элементы
                button2.Enabled = true;
                button3.Enabled = true;
                numericUpDown1.Enabled = true;
                numericUpDown2.Enabled = true;
                numericUpDown3.Enabled = true;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox5.Enabled = true;
                textBox6.Enabled = true;
                textBox7.Enabled = true;
                checkBox1.Enabled = true;

                label8.Text = measurement.CoefficientSpectralUsing(300).ToString();

                List<double> Wave = measurement.DataWithHalfPoint(0, 0);
                List<double> Transmission = measurement.DataWithHalfPoint(0, 1);
                List<double> Transmittance = measurement.DataWithHalfPoint(1, 0);
                List<double> Value = measurement.DataWithHalfPoint(1, 1);


                chart1.Series["Series1"].Points.Clear();
                //Series wavelength = new Series("wavelength");

                //chart1.ChartAreas.AxisX.ScrollBar.IsPositionedInside = true;
                //добавляем значения в коллекцию
                for (int i = 0; i < (Wave.Count()); i++)
                {

                    chart1.Series["Series1"].Points.AddXY(Wave[i],Transmission[i]);
                    //chart1.Series["Series2"].Points.AddXY(10000/Wave[i], 1.0 - Transmission[i]);
                    chart1.Series["Series1"].ChartType = SeriesChartType.Line;
                    chart1.Series["Series1"].BorderWidth = 2;


                }


                //branches.ChartType = SeriesChartType.Line;
                
                //chart1.ChartAreas[0].AxisX2.Maximum = 1D;
                

                //отрисовываем на графике вертикальные линии,пересекающие график на уровне 0.5
                for (int i = 0; i < Transmittance.Count(); i++)
                {
                    chart1.Series.Add(Convert.ToString(i));

                    //HalfSeries Series = new HalfSeries(i);
                    chart1.Series[Convert.ToString(i)] = new HalfSeries(i);
                    chart1.Series[Convert.ToString(i)].Points.AddXY(Value[i], 0);
                    chart1.Series[Convert.ToString(i)].Points.AddXY(Value[i], Transmittance[i]);


                    //chart1.Series[Convert.ToString(i)].Points[1].Label = Wave[i].ToString("0.#") + " см^(-1)" + "\n" + (10000/(Wave[i])).ToString("0.00#")+ " мкм";

                    
                    DataAnnotation PointAnnotation = new DataAnnotation(i);
                    PointAnnotation.Text = Value[i].ToString("0.#") + " см^(-1)" + "\n" + (10000 / (Value[i])).ToString("0.00#") + " мкм"; ;
                    PointAnnotation.AnchorDataPoint = chart1.Series[Convert.ToString(i)].Points[1];
                    chart1.Annotations.Add(PointAnnotation);
                    

                }


                //масштабирвоание chart1.ChartAreas[0].InnerPlotPosition.Width = chart1.ChartAreas[0].InnerPlotPosition.Width * ((float)1.1);
                //chart1.Series[0].LegendText = "hello";
                //chart1.Series["0"].Label = "hello";

                for (int i = 0; i < Wave.Count(); i++)
                {
                    StripLine stripline1 = new StripLine();
                    stripline1.Interval = 0;
                    stripline1.IntervalOffset = Wave[i];
                    stripline1.StripWidth = 0.01;
                    stripline1.BackColor = Color.Transparent;
                    stripline1.Tag = Convert.ToString(Transmission[i]) + "\t" + Convert.ToString(Wave[i]);
                    chart1.ChartAreas[0].AxisX.StripLines.Add(stripline1);
                }




                //отрисовываем на графике горизонтальную линию на уровне 0.5
                //StripLine stripline2 = new StripLine();
                //stripline2.Interval = 0;
                //stripline2.IntervalOffset = 0.5;
                //stripline2.StripWidth = 0.001;
                //stripline2.BackColor = Color.Black;
                //stripline2.Text = "Уровень 0.5";
                //stripline2.BackHatchStyle = ChartHatchStyle.DashedVertical;
                //chart1.ChartAreas[0].AxisY2.StripLines.Add(stripline2);

                ////Выделим точки на уровне 0.5
                //for (int i = 0; i < Number.Count(); i++)
                //{
                //    chart1.Series[0].Points[Convert.ToInt32(Number[i])].Color = Color.Black;
                //    chart1.Series[0].Points[Convert.ToInt32(Number[i])].BorderWidth = 6;
                //    chart1.Series[0].Points[Convert.ToInt32(Number[i])].BorderColor = Color.Black;
                //}

                //эта штука работает
                ////chart1.Series[0].ToolTip = "Percent: #PERCENT";
                ////chart1.Series[0].LegendToolTip = "Income in #LABEL  is #VAL million";
                ////chart1.Series[0].LabelToolTip = "#PERCENT";
                ////chart1.Series[0].Points[1].ToolTip = "Unknown";

                //chart1.Series[0].ToolTip = "X = #VALX, Y = #VALY";

                //отображение верхней оси в мкм
                //CustomLabel wavelength = new CustomLabel();
                //wavelength.Text = "Reterw";
               
                //chart1.ChartAreas["ChartArea1"].AxisX2.Maximum = 1000;
                //chart1.ChartAreas["ChartArea1"].AxisX2.Minimum = 1000;
                //chart1.ChartAreas["ChartArea1"].AxisX2.Interval = 0.5;
                //chart1.ChartAreas["ChartArea1"].AxisX2.CustomLabels.Add(wavelength);
                //chart1.ChartAreas["ChartArea1"].AxisX2.IsReversed = true;
                //chart1.ChartAreas["ChartArea1"].AxisX2.Enabled = AxisEnabled.True;
                ////chart1.ChartAreas["ChartArea1"].AxisX2.MajorTickMark = t





            }
        }
        
        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                

                string fileName = saveFileDialog1.FileName;
                double left;
                double right;

                if (double.TryParse(textBox1.Text,out left))
                    measurement.SetLeftBorder(left);
                else
                    measurement.SetLeftBorder(null);

                if (double.TryParse(textBox2.Text, out right))
                {
                    measurement.SetLeftBorder(right);
                }
                else
                {
                    measurement.SetLeftBorder(null);
                }



                measurement.SaveProcessedFile(fileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            //if (LoadFilterButton.Click == true)
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog2.FileName;
            }

            chart1.SaveImage(saveFileDialog2.FileName, ChartImageFormat.Png);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.Minimum = (double)numericUpDown1.Value;
            //chart1.ChartAreas[0].AxisX2.Minimum = 10000/(double)numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.Maximum = (double)numericUpDown2.Value;
            
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.Interval = (double)numericUpDown3.Value;
            //chart1.ChartAreas[0].AxisX2.Interval = 10000/(double)numericUpDown3.Value;
        }

        public void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            //отображение текущего положение курсора,а также при наведение курсора на график,отображение координат точки
            HitTestResult result = chart1.HitTest(e.X, e.Y);
            chart1.ChartAreas[0].CursorX.Interval = 0;
            chart1.ChartAreas[0].CursorY.Interval = 0;
            //Point mousePoint = new Point(e.X, e.Y);

            if (result.ChartElementType == ChartElementType.StripLines)
            {
                string s = ((result.Object as StripLine).Tag).ToString();
                String[] figure = s.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                chart1.ChartAreas[0].CursorX.SetCursorPosition(Convert.ToDouble(figure[1]));
                chart1.ChartAreas[0].CursorY.SetCursorPosition(Convert.ToDouble(figure[0]));
                
            }

            if (result.ChartArea != null)
            {
                label11.Text = ("x = " + chart1.ChartAreas[0].AxisX.PixelPositionToValue(e.X).ToString("0.#") + " : " + "y = " + chart1.ChartAreas[0].AxisY.PixelPositionToValue(e.Y).ToString("0.00"));
            }
            else
            {
                label11.Text = "";
            }
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if((checkBox1.CheckState == CheckState.Checked)&&(textBox7.Text != ""))
            {
                chart1.Titles.Clear();
                Title title = chart1.Titles.Add("Спектральная характеристика образца " + textBox7.Text);
                
            }
            if (checkBox1.CheckState == CheckState.Unchecked)
            {
                chart1.Titles.Clear();
            }

        }

        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            List<double> Transmittance = measurement.DataWithHalfPoint(1, 0);
            List<double> Value = measurement.DataWithHalfPoint(1, 1);

            MouseCLickQuantity = MouseCLickQuantity + 1;
            double result = Math.IEEERemainder(MouseCLickQuantity, 2);


            for (int i = 0; i < Value.Count(); i++)
            {

                if ((Math.Abs(chart1.ChartAreas[0].AxisX.PixelPositionToValue(e.X) - Value[i]) <= 250) && (Math.Abs(chart1.ChartAreas[0].AxisY2.PixelPositionToValue(e.Y) - Transmittance[i]) <= 0.1))// && (result == 1))
                {
                    chart1.Series[Convert.ToString(i)].Enabled = false;
                    chart1.Annotations[Convert.ToString(i)].Visible = false;

                }
                //if ((Math.Abs(chart1.ChartAreas[0].AxisX.PixelPositionToValue(e.X) - Value[i]) <= 250) && (Math.Abs(chart1.ChartAreas[0].AxisY2.PixelPositionToValue(e.Y) - Transmittance[i]) <= 0.1) && (result == 0))
                //{
                //    chart1.Series[Convert.ToString(i)].Enabled = true;
                //    chart1.Annotations[Convert.ToString(i)].Visible = true;
                //}
            }
            
        }
    }
}
