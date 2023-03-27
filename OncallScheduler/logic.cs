using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OncallScheduler
{
    class logic
    {
        public string[][] Scheduler(int TotalDays, List<Tuple<int, string>> HaveTo, List<Tuple<int, string>> Cannot, Dictionary<string, int> PersonsToSchedule)
        {
            string[][] Schedule = new string[TotalDays + 1][];

            int Persons = PersonsToSchedule.Count;
            Schedule[0] = new string[Persons];

            List<int> HaveToDays = new List<int>();

            foreach (var haveto in HaveTo)
            {
                Schedule[haveto.Item1] = new string[] { haveto.Item2 };
                HaveToDays.Add(haveto.Item1);
                PersonsToSchedule[haveto.Item2] -= 1;

            }

            for (int i = 1; i <= TotalDays; i++)
            {
                if (!HaveToDays.Contains(i))
                {
                    List<string> PeopleTo = new List<string>();
                    List<string> PeopleThatCannot = Cannot.Where(x => x.Item1.Equals(i)).Select(x => x.Item2).ToList();
                    int j = 0;
                    foreach (string person in PersonsToSchedule.Keys)
                    {
                        if (!PeopleThatCannot.Contains(person))
                        {
                            PeopleTo.Add(person);
                        }
                    }
                    Schedule[i] = PeopleTo.ToArray();
                }
            }

            string[][] Schedule2 = NoNextOrPreviousDayRule(Schedule, HaveTo, Cannot);

            int k = 2;
            while (k <= Persons)
            {
                string[][] Schedule3 = Guess(Schedule2, HaveTo, Cannot, k, PersonsToSchedule);
                k++;
            }


            return Schedule2;
        }

        public string[][] NoNextOrPreviousDayRule(string[][] Schedule, List<Tuple<int, string>> HaveTo, List<Tuple<int, string>> Cannot)
        {
            string[][] OldSchedule = Schedule;

            for (int j = 1; j <= Schedule.GetUpperBound(0); j++)
            {
                if (Schedule[j].Length == 1)
                {
                    int _NxtDay = j + 1;
                    int _PrevDay = j - 1;
                    string PersonInQuestion = Schedule[j].First();

                    if (_NxtDay <= Schedule.GetUpperBound(0))
                    {
                        if (Array.IndexOf(Schedule[_NxtDay], PersonInQuestion) > -1)
                        {
                            List<string> PeopleThatCannot = Cannot.Where(x => x.Item1.Equals(_NxtDay)).Select(x => x.Item2).ToList();
                            List<string> PeopleThatHaveTo = HaveTo.Where(x => x.Item1.Equals(_NxtDay)).Select(x => x.Item2).ToList();

                            if (!PeopleThatHaveTo.Contains(PersonInQuestion))
                            {
                                //RemoveThisPerson
                                Schedule[_NxtDay] = Schedule[_NxtDay].Where(e => e != PersonInQuestion).ToArray();
                            }

                        }
                    }

                    if (_PrevDay >= (Schedule.GetLowerBound(0) + 1))
                    {
                        if (Array.IndexOf(Schedule[_PrevDay], PersonInQuestion) > -1)
                        {
                            List<string> PeopleThatCannot = Cannot.Where(x => x.Item1.Equals(_PrevDay)).Select(x => x.Item2).ToList();
                            List<string> PeopleThatHaveTo = HaveTo.Where(x => x.Item1.Equals(_PrevDay)).Select(x => x.Item2).ToList();

                            if (!PeopleThatHaveTo.Contains(PersonInQuestion))
                            {
                                //RemoveThisPerson
                                Schedule[_PrevDay] = Schedule[_PrevDay].Where(e => e != PersonInQuestion).ToArray();
                            }

                        }
                    }
                }
            }

            if (Schedule != OldSchedule)
            {
                return NoNextOrPreviousDayRule(Schedule, HaveTo, Cannot);
            }
            else
            {
                return Schedule;
            }
        }

        public string[][] Guess(string[][] Schedule, List<Tuple<int, string>> HaveTo, List<Tuple<int, string>> Cannot, int MinLength, Dictionary<string, int> PersonsToSchedule)
        {
            string[][] OldSchedule = Schedule;
            string[] TemporaryHold;
            Random random = new Random();
            List<string> FailedGuess = new List<string>();

            int n = 0;

            for (int j = 1; j <= Schedule.GetUpperBound(0); j++)
            {
                if (Schedule[j].Length == MinLength)
                {
                    string Guess = GiveMeAName(Schedule[j], PersonsToSchedule, FailedGuess);
                    TemporaryHold = Schedule[j];
                    Schedule[j] = new string[] { Guess };
                    PersonsToSchedule[Guess] -= 1;

                    Schedule = NoNextOrPreviousDayRule(Schedule, HaveTo, Cannot);
                    List<int> Lengths = new List<int>(Schedule.Select(x => x.Length));
                    while (Lengths.Contains(0))
                    {
                        PersonsToSchedule[Guess] += 1;
                        Schedule[j] = TemporaryHold;
                        FailedGuess.Add(Guess);

                        if(Schedule[j].Count() != FailedGuess.Count())
                        {
                            Guess = GiveMeAName(Schedule[j], PersonsToSchedule, FailedGuess);
                            Schedule = NoNextOrPreviousDayRule(Schedule, HaveTo, Cannot);
                            Lengths = new List<int>(Schedule.Select(x => x.Length));
                        }
                        else
                        {
                            Console.WriteLine("Trouble");
                        }

                        
                    }
                    n++;
                    break;
                }
            }



            if (n > 0)
            {
                int k = 2;
                string[][] Schedule3 = Schedule;
                while (k <= MinLength)
                {
                    Schedule3 = Guess(Schedule, HaveTo, Cannot, k, PersonsToSchedule);
                    k++;
                }
                return Schedule3;
            }
            else
            {
                return Schedule;
            }
        }

        private string GiveMeAName(string[] Options, Dictionary<string, int> PersonsToSchedule, List<string> FailedList)
        {

            List<string> NewOptions = new List<string>();
            foreach (string person in PersonsToSchedule.Where(x => x.Value > 0).Select(x => x.Key))
            {
                NewOptions.Add(person);
            }
            if (FailedList.Count != 0)
            {
                foreach (string fail in FailedList)
                {
                    NewOptions.Remove(fail);
                }

            }
            
            if(NewOptions.Count != 0)
            {
                string[] _NewOptions = NewOptions.ToArray();

                var rand = new System.Random();
                int index = rand.Next(0, _NewOptions.Length - 1);
                return _NewOptions[index];
            }
            else
            {
                return FailedList.First();
            }
            
        }
    }
}
