using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Prism.Commands;
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

namespace RevitAPICreateFilter
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;
        private Document _doc;

        public List<ViewPlan> Views { get; }
        public object Filters { get; }
        public DelegateCommand AddFilterCommand { get; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            _doc = _commandData.Application.ActiveUIDocument.Document;
            Views = GetFloorPlanViews(_doc);
            Filters = GetFilters(_doc);
            AddFilterCommand = new DelegateCommand(OnAddFilterCommand);

        }

        public ParameterFilterElement SelectedFilters { get; set; }

        private void OnAddFilterCommand()
        {
            if(SelectedViewPlan == null||SelectedFilters==null)
                return;

            using (var ts= new Transaction(_doc,"Set filter"))
            {
                ts.Start();
                SelectedViewPlan.AddFilter(SelectedFilters.Id);
                OverrideGraphicSettings overrideGraphicSettings=SelectedViewPlan.GetFilterOverrides(SelectedFilters.Id);
                overrideGraphicSettings.SetProjectionLineColor(new Color(255,0,0));
                SelectedViewPlan.SetFilterOverrides(SelectedFilters.Id, overrideGraphicSettings);
                ts.Commit();
            }
            RaiseCloseRequest();
        }

        public ViewPlan SelectedViewPlan { get; set; }

        public event EventHandler CloseRequest;

        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);//закрытие окна
        }


        public static List<ViewPlan> GetFloorPlanViews(Document doc)
        {
            var views
               = new FilteredElementCollector(doc)
                   .OfClass(typeof(ViewPlan))
                   .Cast<ViewPlan>()
                   .Where(p => p.ViewType == ViewType.FloorPlan)
                   .ToList();
            return views;
        }


        public static List<ParameterFilterElement> GetFilters(Document doc)
        {
            var filters = new FilteredElementCollector(doc)
                            .OfClass(typeof(ParameterFilterElement))
                            .Cast<ParameterFilterElement>()
                            .ToList();
            return filters;
        }


    }
}
