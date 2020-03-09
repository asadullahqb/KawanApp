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
        private bool _kawanStatsIsLoading = true;
        private int[] _arrayOfOnlineFrequencies = new int[24];
        private int _highestNumberOfUsers;
        private string _listOfPeakTimes = "";
        private KawanStats _kawanStats = new KawanStats() { TimeSpent = "--", StudentsHelped = 0, ActivitiesLogged = 0, ListOfRanks = new ListOfRanks() { FirstMonth = 4, SecondMonth = 4, ThirdMonth = 4, PredictedMonth = 4 } };

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
        public bool KawanStatsIsLoading
        {
            get { return _kawanStatsIsLoading; }
            set
            {
                _kawanStatsIsLoading = value;
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

        public PlotModel AverageUsageTimeModel
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

        public int HighestNumberOfUsers
        {
            get { return _highestNumberOfUsers; }
            set
            {
                _highestNumberOfUsers = value;
                OnPropertyChanged();
            }
        }

        public string ListOfPeakTimes
        {
            get { return _listOfPeakTimes; }
            set
            {
                _listOfPeakTimes = value;
                OnPropertyChanged();
            }
        }

        public KawanStats KawanStats
        {
            get { return _kawanStats; }
            set
            {
                _kawanStats = value;
                OnPropertyChanged();
            }
        }

        public AnalyticsPageViewModel(string kawanuserstudentid)
        {
            IsKawan = App.CurrentUserType.Equals("Kawan");

            if (App.NetworkStatus)
            {
                FetchAndSetKawanStats();
                FetchAndSetUserOnlineTimeFrequencies();
            }
            else
            {
                App.Current.MainPage.DisplayAlert("Error", "Please turn on internet.", "Ok");
                return;
            }
        }

        private async void FetchAndSetKawanStats()
        {
            User u = new User() { StudentId = App.CurrentUser } ;
            if (IsKawan)
            {
                KawanStats = await ServerApi.FetchKawanStats(u);
                await SetContributionGraph();
            }
            await Task.Run(() => { KawanStatsIsLoading = false; });
        }

        private async Task SetContributionGraph()
        {
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
            double firstMonthRank = KawanStats.ListOfRanks.FirstMonth;
            double secondMonthRank = KawanStats.ListOfRanks.SecondMonth;
            double thirdMonthRank = KawanStats.ListOfRanks.ThirdMonth;

            var existingRanksLine = new LineSeries
            {
                StrokeThickness = 1.0,
                Color = OxyColor.FromRgb(128, 204, 40) //Green, hex = 80cc28
            };

            existingRanksLine.Points.Add(new DataPoint(0, 0)); //Have continuity from the blank line
            existingRanksLine.Points.Add(new DataPoint(0, firstMonthRank));
            existingRanksLine.Points.Add(new DataPoint(1, secondMonthRank));
            existingRanksLine.Points.Add(new DataPoint(2, thirdMonthRank));

            double predictedRank = KawanStats.ListOfRanks.PredictedMonth; //Set from server

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
        }

        private async void FetchAndSetUserOnlineTimeFrequencies()
        {
            FriendRequest fr = new FriendRequest();
            ArrayOfOnlineFrequencies = await ServerApi.FetchUserOnlineTimeFrequencies(fr);
            await SetUserOnlineTimeFrequenciesChart();
        }

        private async Task SetUserOnlineTimeFrequenciesChart() 
        {
            AverageUsageTimeModel = new PlotModel
            {
                Title = "When Users Used Kawan App Last Month",
                TitleFontSize = 14,
                TitlePadding = 10,
                TitleColor = OxyColor.FromRgb(57, 53, 54), //Hex = 393536
                PlotAreaBorderColor = OxyColor.FromRgb(57, 53, 54), //Hex = 393536
                AxisTierDistance = 1.0,
            };

            //Find index of highest value
            HighestNumberOfUsers = ArrayOfOnlineFrequencies.Max();

            int max = 100; //Maximum value for frequency's scale
            int frequencyAxisMaximum = max;
            if (HighestNumberOfUsers < max) frequencyAxisMaximum = HighestNumberOfUsers + 10;
            if (frequencyAxisMaximum > max) frequencyAxisMaximum = max;
            CategoryAxis frequencyAxisAverageUsageTime = new CategoryAxis() { Minimum = 0, AbsoluteMinimum = 0, Maximum = frequencyAxisMaximum, AbsoluteMaximum = frequencyAxisMaximum, Key = "Frequency", Position = AxisPosition.Left }; //y-axis
            CategoryAxis timeAxisAverageUsageTime = new CategoryAxis() { Minimum = -0.3, Maximum = 0.3, Key = "Time", Position = AxisPosition.Bottom }; //x-axis

            /*//For time axis if is separated bar chart
            string ampm = "AM";
            for (int i = 0; i < 24; i++)
            {
                int time;
                if (i < 12)
                {
                    time = i + 12;
                    ampm = "AM";
                }
                else 
                {
                    time = i;
                    ampm = "PM";
                }

                if (time > 12) //Reset to 12hrs format
                    time -= 12;
                timeAxisAverageOnlineTime.Labels.Add(time.ToString() + ampm);
            }*/

            /*//For time axis if continuous bar chart (but, not working as intended since OxyPlot does not support new line
            string label = "";
            string ampm;
            for (int i = 0; i < 24; i++)
            {
                int time;
                if (i < 12)
                {
                    time = i + 12;
                    ampm = "AM";
                }
                else 
                {
                    time = i;
                    ampm = "PM";
                }

                if (time > 12) //Reset to 12hrs format
                    time -= 12;

                label += time.ToString() + ampm + Environment.NewLine;
            }*/

            //For time axis if using columns instead of bars
            string label = "12AM                           12PM                           12AM";
            timeAxisAverageUsageTime.Labels.Add(label);

            frequencyAxisAverageUsageTime.Labels.Add("0");
            for (int i = 1; i <= max; i++)
            {
                if (i % 10 > 0)
                    frequencyAxisAverageUsageTime.Labels.Add("");
                else
                    frequencyAxisAverageUsageTime.Labels.Add(i.ToString());
            }
            frequencyAxisAverageUsageTime.Labels.Add("");
            frequencyAxisAverageUsageTime.Labels.Add("");
            frequencyAxisAverageUsageTime.Labels.Add("");
            frequencyAxisAverageUsageTime.Labels.Add("");
            frequencyAxisAverageUsageTime.Labels.Add("");

            /*//Creating separated bars
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

            //Create separated bars:
            barSeries.Items.Add(new BarItem(0) { Color = OxyColors.White }); //Start with a blank bar for better look
            for (int i = 0; i < 24; i++)
            {
                OxyColor barColor;
                double val = ArrayOfOnlineFrequencies[i];
                if (val == 0)
                    barColor = OxyColors.White;
                else if (indexesOfIsHighestValue[i])
                    barColor = OxyColor.FromRgb(80, 128, 25); //Dark green, hex = 508019
                else
                    barColor = OxyColor.FromRgb(128, 204, 40); //Green, hex = 80cc28
                barSeries.Items.Add(new BarItem(ArrayOfOnlineFrequencies[i]) { Color = barColor });
            }

            AverageOnlineTimeModel.Series.Add(barSeries);
            */


            //Creating continuous columns (can also be used for bars):

            //Tag each hour as is highest or not and add the highest hour to an appended list
            bool[] indexesOfIsHighestValue = new bool[24];
            string ampm;
            string ampmplus1;
            for (int i = 0; i < 24; i++)
            {
                indexesOfIsHighestValue[i] = ArrayOfOnlineFrequencies[i] == HighestNumberOfUsers;
                
                int time;
                int timeplus1;
                if (i < 12)
                {
                    time = i + 12;
                    ampm = "AM";
                    ampmplus1 = "AM";
                }
                else
                {
                    time = i;
                    ampm = "PM";
                    ampmplus1 = "PM";
                }

                if (time > 12) //Reset to 12hrs format
                    time -= 12;

                timeplus1 = time + 1;
                
                if (timeplus1 > 12)
                    timeplus1 -= 12;

                if (timeplus1 == 12)
                {
                    if (ampmplus1.Equals("AM"))
                        ampmplus1 = "PM";
                    else
                        ampmplus1 = "AM";
                }
                    

                if (ArrayOfOnlineFrequencies[i] == HighestNumberOfUsers)
                        ListOfPeakTimes += "• " + time.ToString() + ampm + " - " + timeplus1.ToString() + ampmplus1 + Environment.NewLine;
            }

            for (int i=0; i<24; i++)
            {
                OxyColor barColor;
                double val = ArrayOfOnlineFrequencies[i];
                if (val == 0)
                    barColor = OxyColors.White;
                else if (indexesOfIsHighestValue[i])
                    barColor = OxyColor.FromRgb(80, 128, 25); //Dark green, hex = 508019
                else
                    barColor = OxyColor.FromRgb(128, 204, 40); //Green, hex = 80cc28
                var columnSeries = new ColumnSeries
                {
                    StrokeThickness = 0,
                    ColumnWidth = 14,
                    FillColor = barColor
                };

                columnSeries.Items.Add(new ColumnItem { Value = ArrayOfOnlineFrequencies[i] });


                AverageUsageTimeModel.Series.Add(columnSeries);
            }

            AverageUsageTimeModel.Axes.Add(timeAxisAverageUsageTime);
            AverageUsageTimeModel.Axes.Add(frequencyAxisAverageUsageTime);
        }
    }
}