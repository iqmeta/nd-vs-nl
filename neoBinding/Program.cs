using Neodynamic.SDK.Printing;

// DATA

List<DataSet> data = new List<DataSet>
{
    new DataSet
    {
        FlatSourceValue = "Value1",
        ProductCode = new ProductCodeInfo
        {
            Value = "123456789012",
            FontSize = 5,
            IsBold = false
        }
    },
    new DataSet
    {
        FlatSourceValue = "Value2",
        ProductCode = new ProductCodeInfo
        {
            Value = "123456789013",
            FontSize = 5,
            IsBold = false
        }
    }
};

// LABEL

ThermalLabel tLabel = new ThermalLabel(UnitType.Mm, 50, 50);
tLabel.Items.Add(new TextItem()
{
    Text = "SET MY WITH DATASOURCE",
    Name = "TEXT1",
    //BackColorHex = "#000000",
    BackColor = Color.Black,
    X = 5,
    Y = 10,
    TextAlignment = TextAlignment.Center,
    Width = 40,
    Height = 40,
    PrintAsGraphic = true,
    //PROBEM 1 - HOW TO SET PROPERTY WITH DATA BINDING - LIKE FONT BOLD OR FONT SIZE
    Font = {
        Size = 10,
        Bold = false, // WITH (GLOBAL) EXPRESSION?
        Name = "Arial"
    },
    DataField = nameof(DataSet.FlatSourceValue),
    // ********** PROBLEM 2 *************
    // WHEN WILL POCO / NESTED C# PROPERTIES / OBJECTS BE SUPPORTED?
    //DataField = "ProductCode.Value", 
});

//WORKS
tLabel.Expressions.Add("Set [Items!TEXT1.BackColorHex] = \"#ffffff\"");

// ********** PROBLEM 3 *************
// DOESN'T WORK WHEN NOT STRING
//tLabel.Expressions.Add("Set [Items!TEXT1.Font.Size] = 16");
//tLabel.Expressions.Add("Set [Items!TEXT1.Font.Size] = \"16\"");

//tLabel.Expressions.Add("Set [Items!TEXT1.Font.BackColor] = Color.White");
//tLabel.Expressions.Add("Set [Items!TEXT1.Font.BackColor] = \"Color.White\"");
//tLabel.Expressions.Add("Set [Items!TEXT1.Font.BackColor] = 0");

tLabel.DataSource = data;
var pj = new PrintJob()
{
    Replicates = 1,
    Dpi = 203,
    ProgrammingLanguage = ProgrammingLanguage.ZPL,
    CommandsOptimizationEnabled = false
};
string zpl = pj.GetNativePrinterCommands(tLabel);
Console.Read();


public class DataSet
{
    public string FlatSourceValue { get; set; }
    public ProductCodeInfo ProductCode { get; set; }
}
public class ProductCodeInfo
{
    public string Value { get; set; }
    public int FontSize { get; set; }
    public bool IsBold { get; set; }
}