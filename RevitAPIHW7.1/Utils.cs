using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPIHW7._1
{
    public class Utils
    {
        public static List<FamilySymbol> GetSheets(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            List<FamilySymbol> sheetsTypes = new FilteredElementCollector(doc)
                                                        .OfClass(typeof(FamilySymbol))
                                                        .OfCategory(BuiltInCategory.OST_TitleBlocks)
                                                        .Cast<FamilySymbol>()
                                                        .ToList();

            return sheetsTypes;
        }

        public static List<ViewPlan> GetViewPlan(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            List<ViewPlan> viewPlansList = new FilteredElementCollector(doc)
                                                        .OfClass(typeof(ViewPlan))
                                                        .Cast<ViewPlan>()
                                                        .ToList();

            return viewPlansList;
        }
    }
}
