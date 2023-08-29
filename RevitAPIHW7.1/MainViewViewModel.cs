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
        public ExportSheetType SelectedType { get; set; }

        public DelegateCommand AddFilterCommand { get; }
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }


        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            _doc = _commandData.Application.ActiveUIDocument.Document;
            Sheets = Utils.GetSheets(_commandData);
            AddFilterCommand = new DelegateCommand(OnAddFilterCommand);
        }

        private void OnAddFilterCommand()
        {


            using (var ts = new Transaction(_doc, "Add filter to schedule"))
            {
                ts.Start();

                ts.Commit();
            }
        }



        public event EventHandler CloseRequest;

        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);//закрытие окна
        }



    }
}
