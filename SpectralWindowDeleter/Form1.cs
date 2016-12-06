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
        
        List<double> Transmittance;
        List<double> Value;


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

                label8.Text = measurement.CoefficientSpectralUsing(800).ToString();

                List<double> Wave = measurement.DataWithHalfPoint(0, 0);
                List<double> Transmission = measurement.DataWithHalfPoint(0, 1);
                Transmittance = measurement.DataWithHalfPoint(1, 0);
                Value = measurement.DataWithHalfPoint(1, 1);


                chart1.Series[0].Points.Clear();
                
                //добавляем значения в коллекцию
                for (int i = 0; i < (Wave.Count()); i++)
                {

                    chart1.Series[0].Points.AddXY(Wave[i],Transmission[i]);
                    chart1.Series[0].ChartType = SeriesChartType.Line;
                    chart1.Series[0].BorderWidth = 2;
                    
                    //chart1.Series[0].IsVisibleInLegend = true;
                    

                }

                Legend legend2 = new Legend();
                legend2.Title = "GTTFVH";
                legend2.Enabled = true;
                chart1.Legends.Add(legend2);


                //отрисовываем на графике вертикальные линии,пересекающие график на уровне 0.1, 0.5, 0.8
                for (int i = 0; i < Transmittance.Count(); i++)
                {
                    chart1.Series.Add(Convert.ToString(i));
                    chart1.Series[Convert.ToString(i)] = new HalfSeries(i);
                    chart1.Series[Convert.ToString(i)].Points.AddXY(Value[i], 0);
                    chart1.Series[Convert.ToString(i)].Points.AddXY(Value[i], Transmittance[i]);
                    if (i == 0) chart1.Series[Convert.ToString(i)].Enabled = false;
                    
                    

                    DataAnnotation PointAnnotation = new DataAnnotation(i);
                    PointAnnotation.Text = Value[i].ToString("0.#") + " см^(-1)" + "\n" + (10000 / (Value[i])).ToString("0.00#") + " мкм"; ;
                    PointAnnotation.AnchorDataPoint = chart1.Series[Convert.ToString(i)].Points[1];
                    chart1.Annotations.Add(PointAnnotation);
                    

                }

                //отрисовываем горизонтальные стриплайны, чтобы в дальнейшем прикрепить к ним текст
                for (int i = 0; i <= 11; i++)
                {
                    HorizontalStripline stripline1 = new HorizontalStripline(i);
                    stripline1.TextAlignment = StringAlignment.Near;
                    stripline1.TextLineAlignment = StringAlignment.Far;//положение текста относительно линии:ниже, выше, посередине
                    stripline1.TextOrientation = TextOrientation.Horizontal;
                    stripline1.Font = new System.Drawing.Font("Palatino Linotype", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World, ((byte)(204)));
                    
                    chart1.ChartAreas[0].AxisY2.StripLines.Add(stripline1);
                    if (i == 9)
                    {
                        stripline1.Text = "K800 = " + measurement.CoefficientSpectralUsing(800).ToString("0");
                        chart1.Legends[0].Title = "K800 = " + measurement.CoefficientSpectralUsing(800).ToString("0") + "\n"
                            + "fsfsfsdfs";
                        
                    }
                    

                }

               



                // chart1.ChartAreas.AxisX.ScrollBar.IsPositionedInside = true;

                    //масштабирвоание chart1.ChartAreas[0].InnerPlotPosition.Width = chart1.ChartAreas[0].InnerPlotPosition.Width * ((float)1.1);
                    //chart1.Series[0].LegendText = "hello";
                    //chart1.Series["0"].Label = "hello";

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
                    ////chart1.ChartAreas["ChartArea1"].AxisX2.MajorTickMark = t;

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
            
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.Maximum = (double)numericUpDown2.Value;
            
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.Interval = (double)numericUpDown3.Value;
            
        }

        public void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            //отображение текущего положение курсора,а также при наведение курсора на график,отображение координат точки
            HitTestResult result = chart1.HitTest(e.X, e.Y);
            //chart1.ChartAreas[0].CursorX.Interval = 0;
            //chart1.ChartAreas[0].CursorY.Interval = 0;
            //Point mousePoint = new Point(e.X, e.Y);

            //if (result.ChartElementType == ChartElementType.StripLines)
            //{
            //    string s = ((result.Object as StripLine).Tag).ToString();
            //    String[] figure = s.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
            //    chart1.ChartAreas[0].CursorX.SetCursorPosition(Convert.ToDouble(figure[1]));
            //    chart1.ChartAreas[0].CursorY.SetCursorPosition(Convert.ToDouble(figure[0]));
                
            //}

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
           
            
            


            for (int i = 0; i < Value.Count(); i++)
            {

                if ((Math.Abs(chart1.ChartAreas[0].AxisX.PixelPositionToValue(e.X) - Value[i]) <= 100) && (Math.Abs(chart1.ChartAreas[0].AxisY2.PixelPositionToValue(e.Y) - Transmittance[i]) <= 0.1))
                {
                    chart1.Series[Convert.ToString(i)].Enabled = !chart1.Series[Convert.ToString(i)].Enabled;
                    chart1.Annotations[Convert.ToString(i)].Visible = !chart1.Annotations[Convert.ToString(i)].Visible;

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
