using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Prism.Commands;
using RevitAPICreateFilterSpecification.Wrappers;
using RevitAPITrainingLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RevitAPICreateFilterSpecification
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;
        private Document _doc;

        public List<ScheduleWrapper> Schedules { get; }
        public DelegateCommand AddFilterCommand { get; }
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }


        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            _doc = _commandData.Application.ActiveUIDocument.Document;
            Schedules = GetAllTheSchedules(_doc).Select(s => new ScheduleWrapper(s)).ToList();
            AddFilterCommand = new DelegateCommand(OnAddFilterCommand);
        }

        private void OnAddFilterCommand()
        {
            List<ScheduleWrapper> selectedSchedule = Schedules.Where(s => s.IsSelected).ToList();
            if (selectedSchedule == null)
                return;
            foreach (var schedule in selectedSchedule)
            {
                var scheduleDef = schedule.ViewSchedule.Definition;
                if (scheduleDef == null)
                    continue;

                ScheduleField field = FindField(schedule.ViewSchedule, ParameterName);
            }
        }

        private ScheduleField FindField(ViewSchedule viewSchedule, string parameterName)
        {
            ScheduleDefinition definition = viewSchedule.Definition;
            var fieldCount = definition.GetFieldCount();

            for (int i = 0; i < fieldCount; i++)
            {
                var field = definition.GetField(i);
                var fieldName = field.GetName();

                if (fieldName == parameterName)
                    return definition.GetField(i);
            }
            return null;
        }

        public event EventHandler CloseRequest;

        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);//закрытие окна
        }


        public static List<ViewSchedule> GetAllTheSchedules(Document doc)
        {
            return new FilteredElementCollector(doc)
                .OfClass(typeof(ViewSchedule))
                .Cast<ViewSchedule>()
                .ToList();
        }




    }
}
