using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPIHW7._1.Wrappers
{
    public class ScheduleWrapper
    {
        public ScheduleWrapper(ViewSchedule viewSchedule)
        {
            ViewSchedule = viewSchedule;
            Name = viewSchedule.Name;
        }

        public bool IsSelected { get; set; }

        public ViewSchedule ViewSchedule { get; }
        public string Name { get; }
    }
}
