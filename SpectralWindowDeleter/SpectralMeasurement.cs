using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace SpectralWindowDeleter
{
    class SpectralMeasurement
    {
        List<double> windowTransmittance1 = new List<double>();
        List<double> windowTransmittance2 = new List<double>();
        List<double> filterTransmittance1 = new List<double>();
        List<double> filterTransmittance2 = new List<double>();
        List<double> onlyFilterTransmittance1;
        List<double> onlyFilterTransmittance2;
        List<string> onlyFilter;
        List<double> forSplineTransmittancePoint;
        List<double> forSplineWaveValue;

        

        double? LeftBorder;
        double? RightBorder;
        


        public void LoadWindowTransmittance(string fileName)
        {
            string[] windowTransmittance = File.ReadAllLines(fileName, Encoding.Default);

            foreach (string s in windowTransmittance)
            {
                //System.Diagnostics.Debug.WriteLine(s);
                string[] split = s.Split(new Char[] {'\t'});
                windowTransmittance1.Add(Convert.ToDouble(split[0].Replace('.', ',')));
                //Функция Replace(,) не меняет split[], а возвращает измененную строку
                windowTransmittance2.Add(Convert.ToDouble(split[1].Replace('.', ',')));
            }

            
            
        }

        public void SetLeftBorder(double? left)
        {
            LeftBorder = left;
            //Border = null;
        }

        public void SetRightBorder(double? right)
        {
            RightBorder = right;
        }

        public void LoadFilterTransmittance(string fileName)
        {
            forSplineTransmittancePoint = new List<double>();
            forSplineWaveValue = new List<double>();
            

            string[] filterTransmittance = File.ReadAllLines(fileName, Encoding.Default);

            foreach (string s in filterTransmittance)
            {
                
                string[] split = s.Split(new Char[] { '\t' });
                filterTransmittance1.Add(Convert.ToDouble(split[0].Replace('.', ',')));
                filterTransmittance2.Add(Convert.ToDouble(split[1].Replace('.', ',')));
            }

            //составляем массив точек с чувствительностью на уровне 0.1, 0.5, 0.8, запоминая соответствующие волновые числа
            for (int j = 0; j < filterTransmittance2.Count() - 1; j++)
            {
                if (((filterTransmittance2[j] <= 0.1) && (filterTransmittance2[j + 1] >= 0.1)) || ((filterTransmittance2[j] >= 0.1) && (filterTransmittance2[j + 1] <= 0.1)))
                {
                    forSplineTransmittancePoint.Add(0.1);
                    forSplineWaveValue.Add((filterTransmittance1[j] + filterTransmittance1[j + 1]) / 2);
                }
                if (((filterTransmittance2[j] <= 0.5) && (filterTransmittance2[j + 1] >= 0.5)) || ((filterTransmittance2[j] >= 0.5) && (filterTransmittance2[j + 1] <= 0.5)))
                {
                    forSplineTransmittancePoint.Add(0.5);
                    forSplineWaveValue.Add((filterTransmittance1[j] + filterTransmittance1[j + 1]) / 2);
                }
                if (((filterTransmittance2[j] <= 0.8) && (filterTransmittance2[j + 1] >= 0.8)) || ((filterTransmittance2[j] >= 0.8) && (filterTransmittance2[j + 1] <= 0.8)))
                {
                    forSplineTransmittancePoint.Add(0.8);
                    forSplineWaveValue.Add((filterTransmittance1[j] + filterTransmittance1[j + 1]) / 2);
                }
            }
            //удаляем близлежащие точки
            for (int i = 0; i < forSplineWaveValue.Count() - 1; i++)
            {
                for (int j = i + 1; j < forSplineWaveValue.Count() - 1; j++)
                {
                    if (Math.Abs(forSplineWaveValue[i] - forSplineWaveValue[j]) < 10)
                    {
                        //forSplineWaveValue.RemoveAt(i);
                        //forSplineTransmittancePoint.RemoveAt(i);
                        forSplineTransmittancePoint[i] = 0;
                        forSplineWaveValue[i] = 0;


                    }
                }
            }
            double w;
            
            for (int i = 0; i < forSplineWaveValue.Count() - 1; i++)
            {
                for (int j = i; j < forSplineWaveValue.Count() - 1; j++)
                {
                    if (forSplineWaveValue[j] > forSplineWaveValue[j + 1])
                    {
                        w = forSplineWaveValue[j];
                        forSplineWaveValue[j] = forSplineWaveValue[j + 1];
                        forSplineWaveValue[j + 1] = w;
                        w = forSplineTransmittancePoint[j];
                        forSplineTransmittancePoint[j] = forSplineTransmittancePoint[j + 1];
                        forSplineTransmittancePoint[j + 1] = w;

                    }
                }
            }



            //System.Diagnostics.Debug.WriteLine(forSplineWaveValue.Count);

        }

        public void SaveProcessedFile(string fileName)
        {
            ProcessFile();
            onlyFilter = new List<string>();
            for (int i = 0; i < (onlyFilterTransmittance1.Count()); i++)
            {
                onlyFilter.Add((onlyFilterTransmittance1[i].ToString("0.00000") + '\t' + onlyFilterTransmittance2[i].ToString("0.00000")).Replace(',', '.'));
            }

            File.WriteAllLines(fileName, onlyFilter);
            

        }

        private void ProcessFile()
        {
            onlyFilterTransmittance1 = new List<double>();
            onlyFilterTransmittance2 = new List<double>();

            double max_windowTransmittance1 = windowTransmittance1.Max();
            double min_windowTransmittance1 = windowTransmittance1.Min();
            double max_filterTransmittance1 = filterTransmittance1.Max();
            double min_filterTransmittance1 = filterTransmittance1.Min();
            double min;
            double max;
            // "Выравнивание" границы спектров справа
            if (max_windowTransmittance1 > max_filterTransmittance1)
            {
                max = max_filterTransmittance1;
            }
            else
            {
                max = max_windowTransmittance1;
            }

            // "Выравнивание" границы спектров слева
            if (min_windowTransmittance1 > min_filterTransmittance1)
            {
                min = min_windowTransmittance1;
            }
            else
            {
                min = min_filterTransmittance1;
            }

            int Length1 = filterTransmittance1.Count();
            int Length2 = windowTransmittance1.Count();
            
            for (int i = 0; i < Length1; i++)
            {
                if ((filterTransmittance1[i] >= min) && (filterTransmittance1[i] <= max))
                {
                    for (int j = 0; j < Length2 - 1; j++)
                    {
                        
                        if ((filterTransmittance1[i] <= windowTransmittance1[j]) && (filterTransmittance1[i] >= windowTransmittance1[j + 1]) && (!LeftBorder.HasValue || (filterTransmittance1[i] <= LeftBorder)) && (!RightBorder.HasValue || (filterTransmittance1[i] >= RightBorder)))
                        {
                                onlyFilterTransmittance1.Add(filterTransmittance1[i]);
                                onlyFilterTransmittance2.Add((filterTransmittance2[i]) / ((windowTransmittance2[j + 1] + windowTransmittance2[j]) / 2));
                                break;
                        }
                       
                     }
                 }
            }



            //Parallel.For(0, 10, (i) =>
            //{
            //    System.Diagnostics.Debug.WriteLine(i);
            //});
            




            //List<double> num = new List<double>();
            //return new List<double>();//это просто заглушка, чтобы работала программа
            //return onlyFilterTransmittance1;
            //return onlyFilterTransmittance2;
            //throw new NotImplementedException();
        }
        
        public double CoefficientSpectralUsing(int T)
        {
           
            double rs = 0.0;
            double c2 = 14388.0;
            double c1 = 374*Math.Pow(10,6);//*10^12
            double lambda0 = 0.0;
            double lambda1 = 0.0;
            double lambda2 = 0.0;
            double sigma = 5.67/Math.Pow(10, 8);

            for (int i = 0; i < filterTransmittance1.Count() - 1; i++)
            {
                lambda0 = Math.Pow(10, 4)/filterTransmittance1[i]; //полученная величина в мкм
                lambda2 = Math.Pow(10, 4) / filterTransmittance1[i + 1]; //полученная величина в мкм
                lambda1 = (lambda0 + lambda2) / 2; //полученная величина в мкм

                rs = rs + (((c1*(filterTransmittance2[i+1] + filterTransmittance2[i])/2) / (((Math.Pow(lambda1, 5)) * (Math.Exp((c2) / (lambda1 * T)) - 1)))) * Math.Abs(lambda0 - lambda2));
            }

            //System.Diagnostics.Debug.WriteLine(r3);
            System.Diagnostics.Debug.WriteLine(rs);

            return ((Math.Pow(T, 4) * sigma)/rs);


        }


        //Добавим вручную точки на график со значением пропускания 0.5
        //запишем в массив номер этих точек и соответствующие волновые числа

        public List<double> DataWithHalfPoint(int i, int k)
        {
            //System.Diagnostics.Debug.WriteLine(forSplineTransmittancePoint.Count);
            

            if (i == 0)
            {
                if (k == 0)  return filterTransmittance1; else return filterTransmittance2;
            }
            else
            {
                if(k == 0) return forSplineTransmittancePoint; else return forSplineWaveValue;
            }
        }

        public enum Filters { F00, F01, F10, F11};

        //public List<double> DataWithHalfPointOnEmums(Filters f)
        //{
        //  

        //    if (f == Filters.F00)
        //        return filterTransmittanceWithHalfPoint1;
        //    else if (f == Filters.F01)
        //        return filterTransmittanceWithHalfPoint2;
        //    else if (f == Filters.F10)
        //        return forSplineTransmittancePoint;
        //    else
        //        return forSplineWaveValue;
        //}

    }
}
