# Nice vs Neo ZPL

Compare of ZPL output with the following data ready as byte printstream in memory

```
return new List<Label>
{
    new Label { Qty = 2, Name = "Label 1", Ean13 = "460566400005"},
    new Label { Qty = 3, Name = "Label 2", Ean13 = "460566400005"},
    new Label { Qty = 30, Name = "Label 3", Ean13 = "460566400005"}
};
```

## ZPL OUTPUT FOR (ALMOST) SAME PRINT RESULT
<a href="zpl-output-neodynamic.txt">zpl-output-neodynamic.txt</a> (41,58kb)<br>
<a href="zpl-output-nicelabel.txt">zpl-output-nicelabel.txt</a> (3,54kb)<br>

Solution - 3 Projects
- DATA DLL as 100% same Datasource
- Nicelabel Console App .NET Framework 4.8
- NeoDynamic Console App .NET Core 9

## RESULTS

|  | NiceLabel | NeoDynamic  
| ------------- | ------------- | ------------- |
| Speed  | ⛔ 1.4s  |  ✅ 250ms   | 
| Output Quality  |  ✅ Right, one print job with qty per label data  | ⛔ Not right paged, because of manual mergin print jobs  | 
| Size  |  ✅ 3,5kb using PQ  | ⛔ 41,58kb (more than 10x) not using PQ  | 
|  |  |  |
|  | **5x slower but does it right**  | **over 10x bigger with wrong output** |

## NiceLabel
After 1477,7476 ms ZPL Printstream ready in memory with 3,54 kb<br>
<img src="result-nicelabel.jpg" width="600"/>


## NeoDynamic
After 252,2687 ms ZPL Printstream ready in memory with 41,58 kb<br>
<img src="result-neodynamic.jpg" width="600"/>

