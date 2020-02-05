using KawanApp.Interfaces;
using KawanApp.Models;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using Refit;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KawanApp.ViewModels
{
    public class AnalyticsPageViewModel : BaseViewModel
    {
        private PlotModel _rankModel;
        private PlotModel _averageOnlineTimeModel;
        private string _firstMonth = "Jan";
        private string _secondMonth = "Feb";
        private string _thirdMonth = "Mar";
        private string _predictedMonth = "Apr";
        private bool _isKawan;
        private int[] _arrayOfOnlineFrequencies = new int[24];

        private IServerApi ServerApi => RestService.For<IServerApi>(App.Server);

        public PlotModel RankModel
        {
            get { return _rankModel; }
            set
            {
                _rankModel = value;
                OnPropertyChanged();
            }
        }
        public string FirstMonth
        {
            get { return _firstMonth; }
            set
            {
                _firstMonth = value;
                OnPropertyChanged();
            }
        }
        public string SecondMonth
        {
            get { return _secondMonth; }
            set
            {
                _secondMonth = value;
                OnPropertyChanged();
            }
        }
        public string ThirdMonth
        {
            get { return _thirdMonth; }
            set
            {
                _thirdMonth = value;
                OnPropertyChanged();
            }
        }
        public string PredictedMonth
        {
            get { return _predictedMonth; }
            set
            {
                _predictedMonth = value;
                OnPropertyChanged();
            }
        }
        public bool IsKawan
        {
            get { return _isKawan; }
            set
            {
                _isKawan = value;
                OnPropertyChanged();
            }
        }

        public PlotModel AverageOnlineTimeModel
        {
            get { return _averageOnlineTimeModel; }
            set
            {
                _averageOnlineTimeModel = value;
                OnPropertyChanged();
            }
        }

        public int[] ArrayOfOnlineFrequencies
        {
            get { return _arrayOfOnlineFrequencies; }
            set
            {
                _arrayOfOnlineFrequencies = value;
                OnPropertyChanged();
            }
        }

        public AnalyticsPageViewModel(string kawanuserstudentid)
        {
            IsKawan = App.CurrentUserType.Equals("Kawan");

            if (App.NetworkStatus)
            {
                //Fetch and set user ranks
                FetchAndSetUserOnlineTimeFrequencies();
            }
            else
            {
                App.Current.MainPage.DisplayAlert("Error", "Please turn on internet.", "Ok");
                return;
            }

            #region Setup Contribution Graph
            //Set first, second, third and predicted month names.
            DateTime currentDate = DateTime.Now;
            DateTime firstMonthDate = currentDate.AddMonths(-3);
            DateTime secondMonthDate = currentDate.AddMonths(-2);
            DateTime thirdMonthDate = currentDate.AddMonths(-1);

            string[] monthNames = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames;
            FirstMonth = monthNames[firstMonthDate.Month - 1];
            SecondMonth = monthNames[secondMonthDate.Month - 1];
            ThirdMonth = monthNames[thirdMonthDate.Month - 1];
            PredictedMonth = monthNames[currentDate.Month - 1]; //Predict what will be the rank this month

            //Create plot model:
            RankModel = new PlotModel
            {
                Title = "Your Contribution",
                TitleFontSize = 15.5,
                TitlePadding = 10,
                TitleColor = OxyColor.FromRgb(57, 53, 54), //Hex = 393536
                PlotAreaBorderColor = OxyColor.FromRgb(57, 53, 54), //Hex = 393536
                AxisTierDistance = 1.0,
            };

            LinearAxis xAxisRankModel = new LinearAxis() { AbsoluteMinimum = 0, AbsoluteMaximum = 3, Key = "Month", Position = AxisPosition.Bottom };
            LinearAxis yAxisRankModel = new LinearAxis() { AbsoluteMinimum = 0, AbsoluteMaximum = 5, Key = "Contribution", Position = AxisPosition.Left, IntervalLength = 50 };

            var blank = new LineSeries
            {
                StrokeThickness = 1.0,
                Color = OxyColor.FromRgb(57, 53, 54)
            };

            blank.Points.Add(new DataPoint(0, 5));

            //Set these values from the server
            double firstMonthRank = 1;
            double secondMonthRank = 2;
            double thirdMonthRank = 4;

            var existingRanksLine = new LineSeries
            {
                StrokeThickness = 1.0,
                Color = OxyColor.FromRgb(128, 204, 40) //Green, hex = 80cc28
            };

            existingRanksLine.Points.Add(new DataPoint(0, 0)); //Have continuity from the blank line
            existingRanksLine.Points.Add(new DataPoint(0, firstMonthRank));
            existingRanksLine.Points.Add(new DataPoint(1, secondMonthRank));
            existingRanksLine.Points.Add(new DataPoint(2, thirdMonthRank));

            double predictedRank = 3; //Set from server

            OxyColor predictedcolor;
            if (predictedRank >= thirdMonthRank)
                predictedcolor = OxyColor.FromRgb(80, 128, 25); //Dark green, hex = 508019
            else
                predictedcolor = OxyColor.FromRgb(204, 40, 46); //Red, hex = CC282E

            var predictedRankLine = new LineSeries
            {
                StrokeThickness = 1.0,
                LineStyle = LineStyle.LongDashDot,
                Color = predictedcolor
            };

            predictedRankLine.Points.Add(new DataPoint(2, thirdMonthRank)); //Have continuity from the previous line
            predictedRankLine.Points.Add(new DataPoint(3, predictedRank));

            RankModel.Series.Add(blank);
            RankModel.Series.Add(existingRanksLine);
            RankModel.Series.Add(predictedRankLine);
            RankModel.Axes.Add(xAxisRankModel);
            RankModel.Axes.Add(yAxisRankModel);
            #endregion
        }

        private async void FetchAndSetUserOnlineTimeFrequencies()
        {
            FriendRequest fr = new FriendRequest();
            ArrayOfOnlineFrequencies = await ServerApi.FetchUserOnlineTimeFrequencies(fr);
            await SetUserOnlineTimeFrequenciesChart();
        }

        private async Task SetUserOnlineTimeFrequenciesChart() 
        {
            AverageOnlineTimeModel = new PlotModel
            {
                Title = "When Users Were Online Last Month",
                TitleFontSize = 15.5,
                TitlePadding = 10,
                TitleColor = OxyColor.FromRgb(57, 53, 54), //Hex = 393536
                PlotAreaBorderColor = OxyColor.FromRgb(57, 53, 54), //Hex = 393536
                AxisTierDistance = 1.0,
            };

            //Find index of highest value
            int highestValue = ArrayOfOnlineFrequencies.Max();

            int xAxisMaximum = 50;
            if (highestValue < 50) xAxisMaximum = highestValue + 10;
            if (xAxisMaximum > 50) xAxisMaximum = 50;
            CategoryAxis xAxisAverageOnlineTime = new CategoryAxis() { Minimum = 0, AbsoluteMinimum = 0, Maximum = xAxisMaximum, AbsoluteMaximum = xAxisMaximum, Key = "Frequency", Position = AxisPosition.Bottom };
            CategoryAxis yAxisAverageOnlineTime = new CategoryAxis() { AbsoluteMinimum = 0, AbsoluteMaximum = 26, Key = "Time", Position = AxisPosition.Left, IntervalLength = 50 };
            yAxisAverageOnlineTime.Labels.Add("");
            yAxisAverageOnlineTime.Labels.Add("12AM");
            yAxisAverageOnlineTime.Labels.Add("1AM");
            yAxisAverageOnlineTime.Labels.Add("2AM");
            yAxisAverageOnlineTime.Labels.Add("3AM");
            yAxisAverageOnlineTime.Labels.Add("4AM");
            yAxisAverageOnlineTime.Labels.Add("5AM");
            yAxisAverageOnlineTime.Labels.Add("6AM");
            yAxisAverageOnlineTime.Labels.Add("7AM");
            yAxisAverageOnlineTime.Labels.Add("8AM");
            yAxisAverageOnlineTime.Labels.Add("9AM");
            yAxisAverageOnlineTime.Labels.Add("10AM");
            yAxisAverageOnlineTime.Labels.Add("11AM");
            yAxisAverageOnlineTime.Labels.Add("12PM");
            yAxisAverageOnlineTime.Labels.Add("1PM");
            yAxisAverageOnlineTime.Labels.Add("2PM");
            yAxisAverageOnlineTime.Labels.Add("3PM");
            yAxisAverageOnlineTime.Labels.Add("4PM");
            yAxisAverageOnlineTime.Labels.Add("5PM");
            yAxisAverageOnlineTime.Labels.Add("6PM");
            yAxisAverageOnlineTime.Labels.Add("7PM");
            yAxisAverageOnlineTime.Labels.Add("8PM");
            yAxisAverageOnlineTime.Labels.Add("9PM");
            yAxisAverageOnlineTime.Labels.Add("10PM");
            yAxisAverageOnlineTime.Labels.Add("11PM");
            yAxisAverageOnlineTime.Labels.Add("");

            xAxisAverageOnlineTime.Labels.Add("1");
            for (int i = 1; i <= 50; i++)
            {
                if (i % 10 > 0)
                    xAxisAverageOnlineTime.Labels.Add("");
                else
                    xAxisAverageOnlineTime.Labels.Add(i.ToString());
            }
            xAxisAverageOnlineTime.Labels.Add("");
            xAxisAverageOnlineTime.Labels.Add("");
            xAxisAverageOnlineTime.Labels.Add("");
            xAxisAverageOnlineTime.Labels.Add("");

            var barSeries = new BarSeries
            {
                StrokeThickness = 0,
                BarWidth = 4
            };

            //Tag each hour as is highest or not
            bool[] indexesOfIsHighestValue = new bool[24];
            for (int i = 1; i < 24; i++)
            {
                indexesOfIsHighestValue[i] = ArrayOfOnlineFrequencies[i] == highestValue;
            }

            //Create columns:
            barSeries.Items.Add(new BarItem(0) { Color = OxyColors.White }); //Blank column for better look
            for (int i = 0; i < 24; i++)
            {
                OxyColor columnColor;
                double val = ArrayOfOnlineFrequencies[i];
                if (val == 0)
                    columnColor = OxyColors.White;
                else if (indexesOfIsHighestValue[i])
                    columnColor = OxyColor.FromRgb(80, 128, 25); //Dark green, hex = 508019
                else
                    columnColor = OxyColor.FromRgb(128, 204, 40); //Green, hex = 80cc28
                barSeries.Items.Add(new BarItem(ArrayOfOnlineFrequencies[i]) { Color = columnColor });
            }

            AverageOnlineTimeModel.Series.Add(barSeries);
            AverageOnlineTimeModel.Axes.Add(xAxisAverageOnlineTime);
            AverageOnlineTimeModel.Axes.Add(yAxisAverageOnlineTime);
        }
    }
}