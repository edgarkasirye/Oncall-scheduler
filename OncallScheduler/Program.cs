using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OncallScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            logic lg = new logic();

            int TotalDays = 31;
            //var PersonsToSchedule = new List<string> {"Edgar","Rodney","Anthony","Wilson","Sinani","Isaac"};

            var HaveTo = new List<Tuple<int, string>>();
            var PersonsToSchedule = new Dictionary<string,int>();
            var Cannot = new List<Tuple<int, string>>();


            PersonsToSchedule.Add("Edgar",6);
            PersonsToSchedule.Add("Rodney",5);
            PersonsToSchedule.Add("Wilson",5);
            PersonsToSchedule.Add("Anthony",5);
            PersonsToSchedule.Add("Sinani",5);
            PersonsToSchedule.Add("Isaac",5);

            HaveTo.Add(Tuple.Create(1,"Edgar"));
            HaveTo.Add(Tuple.Create(12, "Rodney"));
            HaveTo.Add(Tuple.Create(10, "Wilson"));
            HaveTo.Add(Tuple.Create(25, "Anthony"));
            HaveTo.Add(Tuple.Create(30, "Isaac"));

            Cannot.Add(Tuple.Create(11, "Edgar"));
            Cannot.Add(Tuple.Create(16, "Rodney"));
            Cannot.Add(Tuple.Create(19, "Wilson"));
            Cannot.Add(Tuple.Create(23, "Sinani"));
            Cannot.Add(Tuple.Create(23, "Anthony"));

            string[][] Schedule = lg.Scheduler(TotalDays, HaveTo, Cannot, PersonsToSchedule);

            for (int j = 1; j <= Schedule.GetUpperBound(0); j++)
            {
                Console.WriteLine("{0} ----> {1}", j, (Schedule[j] != null) ?Schedule[j].First():"0");
            }

        }
    }
}
