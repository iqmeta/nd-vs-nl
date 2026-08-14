using System;
using System.Collections.Generic;
using System.Text;

namespace data
{
    public class Label
    {
        public int Qty { get; set; } = 1;
        public string Name { get; set; } = "World";
        public string Ean13 { get; set; } = "460566400005";
    }
    public static class DataHelper
    {
        public static List<Label> GetLabelData()
        {
            return new List<Label>
            {
                new Label { Qty = 2, Name = "Label 1", Ean13 = "460566400005"},
                new Label { Qty = 3, Name = "Label 2", Ean13 = "460566400005"},
                new Label { Qty = 30, Name = "Label 3", Ean13 = "460566400005"}
            };
        }
    }

}
