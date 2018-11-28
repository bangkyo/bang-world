using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComLib
{
    public enum namespaceEnum
    {
        None, ComLib, SystemControlClassLibrary, BISYS
    }

    public enum FlexGridCellTypeEnum
    {
        Default, CheckBox, ComboBox, TimePicker, TimePicker_All
    }

    public enum FlexGridColumnCaptionEnum
    {
        None, Fixed, Frozen, Fixed_Frozen
    }
    public enum FlexGridRowCaptionEnum
    {
        None, Fixed, Frozen, Fixed_Frozen
    }

    public enum ProductNameEnum
    {
        AI, AU, HB, RB, RR, TP, TS
    }

    public enum WorkSheetEnum
    {
        None, Year, YearMonth
    }
}
