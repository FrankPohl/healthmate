using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace HealthMate.Services
{
    public enum Commands
    {
        Undefined,
        Show,
        ShowList,
        ShowChart,
        Input,
        InputWeight,
        InputGlucose,
        InputBloodPressure,
        InputTemperature,
        Pause,
        Continue
    }

    public class TextEvaluation
    {
        public TextEvaluation()
        {
        }

        private bool CheckForOneOfThePhrases()
        {
            return false;
        }

        // This is used to determine in which mode the user wants to be with his statement
        public Commands GetCommandInText(string Text)
        {
            string evalString = Text.ToLower();
            string[] exitCommandArray = { "continue", "go on", "weiter" };
            if (exitCommandArray.Any(evalString.Contains))
            {
                return Commands.Continue;
            }

            string[] pauseCommandArray = { "pause", "wait", "warte" };
            if (pauseCommandArray.Any(evalString.Contains))
            {
                return Commands.Pause;
            }
            string[] listArray = { "list", "table", "listen", "liste", "tabelle" };
            if (listArray.Any(evalString.Contains))
            {
                return Commands.ShowList;
            }
            string[] chartArray = { "chart", "graph", "grafik" };
            if (chartArray.Any(evalString.Contains))
            {
                return Commands.ShowChart;
            }
            string[] weightArray = { "weight", "weighing", "gewicht", "wiege" };
            if (weightArray.Any(evalString.Contains))
            {
                return Commands.InputWeight;
            }
            string[] bloodPressureArray = { "pressure", "dia", "sys", "blutdruck" };
            if (bloodPressureArray.Any(evalString.Contains))
            {
                return Commands.InputBloodPressure;
            }
            string[] tempArray = { "temp" };
            if (tempArray.Any(evalString.Contains))
            {
                return Commands.InputTemperature;
            }
            string[] glucoseArray = { "glucose", "sugar", "zucker" };
            if (glucoseArray.Any(evalString.Contains))
            {
                return Commands.InputGlucose;
            }
            // check finally for an incomplete command
            // must be done at the end
            string[] inputArray = { "input", "enter", "put in", "eingabe" };
            if (inputArray.Any(evalString.Contains))
            {
                return Commands.Input;
            }
            // final possibility if he wants to navigate in the app but do not knwo where
            string[] showCommandArray = { "show", "open", "go", "öffnen", "gehe zu" };
            if (showCommandArray.Any(evalString.Contains))
            {
                return Commands.Show;
            }

            return Commands.Undefined;
        }

        public double? GetFirstValue(string Text)
        {
            string pattern = @"(?:um|at) \d{2,3}(?:.|,)?\d{0,1}(?: |$)";
            var matches = Regex.Matches(Text, pattern, RegexOptions.IgnoreCase);
            int timePatternStart = 0;
            int timePatternEnd = 0;
            if (matches.Count > 0)
            {
                timePatternStart = matches[0].Index;
                timePatternEnd = matches[0].Index + matches[0].Length;
            }
            pattern = @" \d{2,3}(?:.|,)?\d{0,1}(?: |$)";
            matches = Regex.Matches(Text, pattern, RegexOptions.IgnoreCase);
            foreach (Match item in matches)
            {
                if ((item.Index < timePatternStart) || (item.Index > timePatternEnd))
                {
                    return Convert.ToDouble(item.Value);
                }
            }
            return null;
        }

        public double? GetSecondValue(string Text)
        {
            string pattern = @"(?:um|at) \d{2,3}(?:.|,)?\d{0,1}(?: |$)";
            var matches = Regex.Matches(Text, pattern, RegexOptions.IgnoreCase);
            int timePatternStart = 0;
            int timePatternEnd = 0;
            if (matches.Count > 0)
            {
                timePatternStart = matches[0].Index;
                timePatternEnd = matches[0].Index + matches[0].Length;
            }
            pattern = @" \d{2,3}(?:.|,)?\d{0,1}(?: |$)";

            matches = Regex.Matches(Text, pattern, RegexOptions.IgnoreCase);
            bool firstFound = false;
            foreach (Match item in matches)
            {
                if ((item.Index < timePatternStart) || (item.Index > timePatternEnd))
                {
                    if (firstFound)
                    {
                        return Convert.ToDouble(item.Value);
                    }
                    else
                    {
                        firstFound = true;
                    }
                }
            }
            return null;
        }

        private TimeSpan Get24HourTime(bool IsPM, TimeSpan Time)
        {
            if (IsPM)
            {
                return Time.Add(new TimeSpan(12, 0, 0));
            }
            else
            {
                return Time;
            }
        }

        public DateTime? GetTime(string Text)
        {
            bool isPMTime = false;
            string evalString = Text.ToLower();
            string[] inputArray = { "now", "this moment", "jetzt" };
            if (inputArray.Any(evalString.Contains))
            {
                return DateTime.Now;
            }

            string pattern = @"\d{1,2} PM";
            var matches = Regex.Matches(Text, pattern, RegexOptions.IgnoreCase);
            if (matches.Count > 0)
            {
                isPMTime = true;
            }

            pattern = @"\d{1,2}:{1}\d\d";
            matches = Regex.Matches(Text, pattern, RegexOptions.IgnoreCase);
            if (matches.Count > 0)
            {
                if (TimeSpan.TryParse(matches[0].Value, out var time))
                {
                    return DateTime.MinValue + Get24HourTime(isPMTime, time);
                }
            }

            pattern = @"\d{1,2} (?:vor|to) {1}\d{1,2}";
            matches = Regex.Matches(Text, pattern, RegexOptions.IgnoreCase);
            if (matches.Count > 0)
            {
                var timeMatches = Regex.Matches(matches[0].Value, @"\d{1,2}");
                var hour = Convert.ToInt32(timeMatches[0].Value);
                var minute = 60 - Convert.ToInt32(timeMatches[1].Value);
                return DateTime.MinValue + Get24HourTime(isPMTime, new TimeSpan(hour, minute, 0));
            }
            pattern = @"\d{1,2} (?:nach|past) {1}\d{1,2}";
            matches = Regex.Matches(Text, pattern, RegexOptions.IgnoreCase);
            if (matches.Count > 0)
            {
                var timeMatches = Regex.Matches(matches[0].Value, @"\d{1,2}");
                var hour = Convert.ToInt32(timeMatches[0].Value);
                var minute = Convert.ToInt32(timeMatches[1].Value);
                return DateTime.MinValue + Get24HourTime(isPMTime, new TimeSpan(hour, minute, 0));
            }

            pattern = @"(?:um |at )\d{1,2}";
            matches = Regex.Matches(Text, pattern, RegexOptions.IgnoreCase);
            if (matches.Count > 0)
            {
                var timeMatches = Regex.Matches(matches[0].Value, @"\d{1,2}");
                var hour = Convert.ToInt32(timeMatches[0].Value);
                return DateTime.MinValue + Get24HourTime(isPMTime, new TimeSpan(hour, 0, 0));
            }

            pattern = @"\d{1,2}(?:vor | to )\d{1,2}";
            matches = Regex.Matches(Text, pattern, RegexOptions.IgnoreCase);
            if (matches.Count > 0)
            {
                var timeMatches = Regex.Matches(matches[0].Value, @"\d{1,2}");
                var hour = Convert.ToInt32(timeMatches[0].Value);
                var minute = Convert.ToInt32(timeMatches[2].Value);
                return DateTime.MinValue + Get24HourTime(isPMTime, new TimeSpan(hour - 1, 60 - minute, 0));
            }

            pattern = @"(?:quarter|viertel)(?: vor | to )\d{1,2}";
            matches = Regex.Matches(Text, pattern, RegexOptions.IgnoreCase);
            if (matches.Count > 0)
            {
                var timeMatches = Regex.Matches(matches[0].Value, @"\d{1,2}");
                var hour = Convert.ToInt32(timeMatches[0].Value);
                return DateTime.MinValue + Get24HourTime(isPMTime, new TimeSpan(hour - 1, 45, 0));
            }

            pattern = @"(?:quarter|viertel)(?: nach | past )\d{1,2}";
            matches = Regex.Matches(Text, pattern, RegexOptions.IgnoreCase);
            if (matches.Count > 0)
            {
                var timeMatches = Regex.Matches(matches[0].Value, @"\d{1,2}");
                var hour = Convert.ToInt32(timeMatches[0].Value);
                return DateTime.MinValue + Get24HourTime(isPMTime, new TimeSpan(hour, 15, 0));
            }

            pattern = @"(?:half past )\d{1,2}";
            matches = Regex.Matches(Text, pattern, RegexOptions.IgnoreCase);
            if (matches.Count > 0)
            {
                var timeMatches = Regex.Matches(matches[0].Value, @"\d{1,2}");
                var hour = Convert.ToInt32(timeMatches[0].Value);
                return DateTime.MinValue + Get24HourTime(isPMTime, new TimeSpan(hour, 30, 0));
            }

            pattern = @"(?:halb )\d{1,2}";
            matches = Regex.Matches(Text, pattern, RegexOptions.IgnoreCase);
            if (matches.Count > 0)
            {
                var timeMatches = Regex.Matches(matches[0].Value, @"\d{1,2}");
                var hour = Convert.ToInt32(timeMatches[0].Value);
                return DateTime.MinValue + Get24HourTime(isPMTime, new TimeSpan(hour - 1, 30, 0));
            }

            return null;
        }

        public DateTime? GetDateValue(string Text)
        {
            string evalString = Text.ToLower();
            string[] inputArray = { "now", "this moment", "jetzt", "heute" };
            if (inputArray.Any(evalString.Contains))
            {
                return DateTime.Now;
            }
            string[] dateInputArray = { "yesterday", "gestern" };
            if (dateInputArray.Any(evalString.Contains))
            {
                return DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0));
            }
            string pattern = @"\d{1,2}.\d{1,2}.\d{1,4}";  //1.12.2022
            var matches = Regex.Matches(Text, pattern, RegexOptions.IgnoreCase);
            if (matches.Count > 0)
            {
                var dateMatches = Regex.Matches(matches[0].Value, @"\d{1,2}");
                var day = Convert.ToInt32(dateMatches[0].Value);
                var month = Convert.ToInt32(dateMatches[1].Value);
                var year = Convert.ToInt32(dateMatches[2].Value);
                if (dateMatches.Count == 4)
                {
                    year = year * 100 + Convert.ToInt32(dateMatches[3].Value);
                }
                DateTime dt = new DateTime(year, month, day);
                return dt;
            }

            // German month
            string[] germanMonthArray = { "Januar", "Februar", "März", "April", "Mai", "Juni", "Juli", "August", "September", "Oktober", "November", "Dezember" };
            string[] americnaMonthArray = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "Dezember" };
            pattern = @"\d{1,2}. (?:Januar|Februar|März|April|Mai|Juni|Juli|August|September|Oktober|November|Dezember|January|February|March|May|June|July|October|December) \d{1,4}"; // 1. Januar 2022

            matches = Regex.Matches(Text, pattern, RegexOptions.IgnoreCase);
            if (matches.Count > 0)
            {
                var dateMatches = Regex.Matches(matches[0].Value, @"\d{1,2}");
                var day = Convert.ToInt32(dateMatches[0].Value);
                var year = Convert.ToInt32(dateMatches[1].Value);
                if (dateMatches.Count == 3)
                {
                    year = year * 100 + Convert.ToInt32(dateMatches[2].Value);
                }
                pattern = @"(?:Januar|Februar|März|April|Mai|Juni|Juli|August|September|Oktober|November|Dezember)";
                var monthMatch = Regex.Matches(matches[0].Value, pattern);
                if (monthMatch.Count > 0)
                {
                    var month = Array.IndexOf(germanMonthArray, monthMatch[0].Value);
                    DateTime dt = new DateTime(year, month + 1, day);
                    return dt;
                }
                else
                {
                    // try the American month
                    pattern = @"(?:January|February|March|April|May|June|July|September|October|November|December)";
                    var americanMonthMatch = Regex.Matches(matches[0].Value, pattern);
                    var month = Array.IndexOf(americnaMonthArray, americanMonthMatch[0].Value);
                    DateTime dt = new DateTime(year, month + 1, day);
                    return dt;
                }
            }

            return null;
        }
    }
}