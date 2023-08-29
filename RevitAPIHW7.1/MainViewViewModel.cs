using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Prism.Commands;
using RevitAPIHW7._1.Wrappers;
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

namespace RevitAPIHW7._1
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;
        private Document _doc;

        public List<FamilySymbol> Sheets { get; }
        public FamilySymbol SheetSelect { get; set; }
        public List<ViewPlan> Views { get; }
        public int NumberOfSheets { get; set; }
        public ViewPlan ViewSelect { get; set; }
        public string DesignedBy { get; set; }



        public DelegateCommand AddSheetsCommand { get; }



        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            _doc = _commandData.Application.ActiveUIDocument.Document;
            Sheets = Utils.GetSheets(_commandData);
            Views = Utils.GetViewPlan(_commandData);
            AddSheetsCommand = new DelegateCommand(OnAddSheetsCommand);
        }

        private void OnAddSheetsCommand()
        {

            using (var ts = new Transaction(_doc, "Add new OnAddSheetsCommand"))
            {
                ts.Start();
                for (int i = 0; i < NumberOfSheets; i++)
                {
                    ViewSheet viewSheet = ViewSheet.Create(_doc, SheetSelect.Id);

                    Parameter designedpar = viewSheet.get_Parameter(BuiltInParameter.SHEET_DESIGNED_BY);
                    designedpar.Set(DesignedBy);

                    UV location = new UV((viewSheet.Outline.Max.U - viewSheet.Outline.Min.U) / 2, (viewSheet.Outline.Max.V - viewSheet.Outline.Min.V) / 2);

                    Viewport viewport = Viewport.Create(_doc, viewSheet.Id, ViewSelect.Id, new XYZ(location.U, location.V, 0));
                    //viewSheet.Name = string.Format("Лист-{0}",i+1);
                    //viewSheet.SheetNumber = string.Format("{0}",i+1);
                }
                ts.Commit();
            }
            RaiseCloseRequest();
        }



        public event EventHandler CloseRequest;

        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);//закрытие окна
        }



    }
}
