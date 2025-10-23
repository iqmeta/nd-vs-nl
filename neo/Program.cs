using data;
using Neodynamic.SDK.Printing;
using System.Text;

// LABEL DESIGN
ThermalLabel tLabel = new ThermalLabel(UnitType.Mm, 50, 34);

// WHAT'S MISSING FOR EXAMPLE - THAT PRINT JOB KNOWS HOW MANY LABELS TO PRINT / PAGING ETC:
// COMPARE NICELABEL PROJECT Program.cs Line 40 - Print Qty per Label/DataRow
// tLabel.LabelsPrintOrReplicaQuantityDataField = nameof(Label.Qty);

tLabel.LabelsPerRow = 2;
tLabel.LabelsHorizontalGapLength = 2;

tLabel.Items.Add(new BarcodeItem()
{
    Symbology = BarcodeSymbology.Ean13,    
    DataField = nameof(Label.Ean13),        
    X = 2.0,
    Y = 2.0,
    Width = tLabel.Width - 5,
    Height = 13,
    BarHeight = 14,
    BarWidth = 0.5,
    EanUpcGuardBarHeight = 7,
    //QuietZone = { Bottom = 0.5, Left = 0, Right = 0.0, Top = 0.5 },
    BorderThickness = new FrameThickness(0.1)
});

tLabel.Items.Add(new TextItem
{
    DataField = nameof(Label.Name),
    X = 2,
    Y = 20,
    Width = tLabel.Width - 5,
    Height = 10,
    BorderThickness = new FrameThickness(0.3),
    Font = { Size = 14 },
});

DateTime start = DateTime.Now;
var sb = new StringBuilder();
foreach (var dsRow in DataHelper.GetLabelData())
{
    tLabel.DataSource = new Label[1] { dsRow };
    var pj = new PrintJob()
    {
        Replicates = dsRow.Qty,
        Dpi = 203,
        ProgrammingLanguage = ProgrammingLanguage.ZPL,
        CommandsOptimizationEnabled = false
    };
    // THIS IS BAD BECAUSE MULTIPLE PRINJOBS ARE MERGED INTO A SINGLE PRINTSTREAM
    // WITH PREVENTS OPITIMIZATIONS ACROSS WHOLE PRINTSTREAM
    // ALSO IT GENERATES MORE COMMANDS THAN NEEDED
    // CAUSES WRONG PAGING WITH EMPTY LABELS ON ODD QUANTITIES
    sb.Append(pj.GetNativePrinterCommands(tLabel));
    sb.Append(Environment.NewLine);

    //ANOTHER PROBLEM YOU CAN ONLY CREATE A PDF PER PRINTJOB.
    //SO ALSO NEED TO MERGE PDF FILES IF YOU WANT A SINGLE PDF OUTPUT
}

var zpl = sb.ToString();
var outBytes = Encoding.UTF8.GetBytes(zpl);
DateTime end = DateTime.Now;
var total = end - start;
Console.Write($"After {total.TotalMilliseconds} ms ZPL Printstream ready in memory with {outBytes.Length / 1024.0:F2} kb");
System.IO.File.WriteAllBytes("neodynamic.zpl", outBytes);
Console.Read();
