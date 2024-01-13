# Print DataTable To Console

The main functionality of this helper class is to print the contents of a `DataTable` to the `Console`. This extension class can also

- Print `DataTable`, `DataView`, `DataSet`, `DataRow[]` (= `DataTable.Select()`)
- Print Columns
- Print in tabular format and list format
- Print to `Console`, `StringBuilder` or `Stream`

All the examples are performed on a products `DataTable`. All the `DataTable` extension methods in these examples have equivalent extension methods for `DataView`, `DataSet` and `DataRow[]`.

```cs
DataTable products = new DataTable("Products");
products.Columns.Add("Id", typeof(int)).AllowDBNull = false;
products.Columns.Add("ProductName", typeof(string));
products.Columns.Add("InStock", typeof(bool));
products.Columns.Add("DateAdded", typeof(DateTime));
```

Original article of [Print DataTable To Console on CodeProject](https://www.codeproject.com/Tips/1147879/Print-DataTable-to-Console-and-more "Print DataTable To Console on CodeProject").

## Installation

Add the helper file to your project. That's it. The namespace, for this extension class, is `System.Data`, same as `DataTable` and `DataView`, so once you start adding these data objects to your project, the helper class works right out of the box.

## Print

The `Print` extension method prints the `DataTable` in a tabular format. The optional parameters are:

- `rowOrdinals` - Whether to print the ordinal position of the current `DataRow` in the `DataTable`.
- `top` - Print only the first top rows. If `top` is less than or equal to 0 than print all rows.
- `toString` - A delegate that can be used to override the default `ToString()` for certain object types or columns.
- `columnNames` - List of columns to print in a given order. If this parameter is empty than print all the columns. The parameter can accept the same column name several times. For example, printing the `Id` column twice, as the first column and as the last column.

```cs
Print(
    this DataTable dataTable,
    bool rowOrdinals = false,
    int top = 0,
    ValueToStringEventHandler toString = null,
    params string[] columnNames
);

delegate string ValueToStringEventHandler(object obj, DataRow row, DataColumn column);
```

This is the simplest print. Prints all products to the console.

```cs
products.Print();
```

![Print](./Solution%20Items/Images/Print.jpg "Print")

This will print the top 5 products with row ordinals, and it will print the `Id` column twice on both sides of the table.

```cs
products.Print(true, 5, "Id", "DateAdded", "InStock", "Id");
```

![Print Top 5](./Solution%20Items/Images/PrintTop5.jpg "Print Top 5")

This will print the `DateAdded` column in a `yyyy-MM-dd` format. Any other column will be printed based on its default `ToString()` data type.

```cs
products.Print((obj, row, column) =>
{
    if (obj == DBNull.Value)
        return null;

    if (column.ColumnName == "DateAdded")
        return ((DateTime)obj).ToString("yyyy-MM-dd");

    return obj.ToString();
});
```

![Print Custom Date](./Solution%20Items/Images/PrintCustomDate.jpg "Print Custom Date")

## PrintList

The `PrintList` extension method prints the `DataTable` in a list format. The default is a 2-columns vertical layout. `PrintList` layout the results the same way [DataList control display its layout](https://learn.microsoft.com/en-us/previous-versions/aspnet/79k821wc(v=vs.100) "Specify Horizontal or Vertical Layout in DataList Web Server Controls").

The parameters `rowOrdinals`, `top`, `toString` and `columnNames` are the same as the `Print` parameters.


- `repeatColumns` - Number of layout columns.
- `repeatDirection` - Vertical or horizontal layout.
- `delimiter` - The delimiter between the column name and the row value.

```cs
PrintList(
    this DataTable dataTable,
    bool rowOrdinals = false,
    int top = 0,
    ValueToStringEventHandler toString = null,
    int repeatColumns = 2,
    RepeatDirection repeatDirection = RepeatDirection.Vertical,
    string delimiter = ": ",
    params string[] columnNames
);
```

Prints all products with 2-columns vertical layout.

```cs
products.PrintList();
```

![PrintList](./Solution%20Items/Images/PrintList.jpg "PrintList")

Prints `Id` and `ProductName` columns with 3-columns horizontal layout.

```cs
products.PrintList(3, PrintDataExtensions.RepeatDirection.Horizontal, "Id", "ProductName");
```

![PrintList 3 Columns Horizontal](./Solution%20Items/Images/PrintList3ColumnsHorizontal.jpg "PrintList 3 Columns Horizontal")

## PrintColumns

The `PrintColumns` extension method prints information about the columns of the `DataTable`. The output columns are Column Ordinal, Column Name, Data Type, Nullable and Data Member. Data Member is a data member declaration which may come in handy.

```cs
PrintColumns(
    this DataTable dataTable,
    params string[] columnNames
);
```

Prints the columns of the products `DataTable`.

```cs
products.PrintColumns();
```

![PrintColumns](./Solution%20Items/Images/PrintColumns.jpg "PrintColumns")

## Output to Console, StringBuilder or Stream

The default output is to the `Console`. However, you can redirect to output to a `StringBuilder` or to a `Stream` (`MemoryStream`, `FileStream`). The default encoding for `Stream` is `Encoding.UTF8`.

You have to redirect the output before printing. The redirection methods are straightforward:

```cs
PrintDataExtensions.SetOutputConsole();
PrintDataExtensions.SetOutputStringBuilder(StringBuilder builder);
PrintDataExtensions.SetOutputStream(Stream stream);
PrintDataExtensions.SetOutputStream(Stream stream, Encoding encoding);
```

Prints to `StringBuilder`.

```cs
StringBuilder builder = new StringBuilder();
PrintDataExtensions.SetOutputStringBuilder(builder);
products.Print();
```

Prints to `MemoryStream`.

```cs
byte[] buffer = null;
using (MemoryStream stream = new MemoryStream())
{
    PrintDataExtensions.SetOutputStream(stream);
    products.Print();
    buffer = stream.ToArray();
}
string output = Encoding.UTF8.GetString(buffer);
```

Prints to `FileStream`.

```cs
string path = @"C:\Path\To\output.txt";
using (FileStream stream = File.OpenWrite(path))
{
    PrintDataExtensions.SetOutputStream(stream);
    products.Print();
}
```

## Border

The characters that make up the border are part of an Extended ASCII characters set. Extended ASCII codes start from code 128. These characters may not necessarily be mapped to box characters, it depends on your computer localization. If the border comes out garbled, you can change the way the border is printed to regular ASCII characters. There is also an option to remove the border completely.

```cs
PrintDataExtensions.ExtendedASCIIBorder();
PrintDataExtensions.ASCIIBorder();
PrintDataExtensions.ClearBorder();
```

You have to set the border before printing.

```cs
PrintDataExtensions.ASCIIBorder();
products.Print();
```

![Print ASCII Border](./Solution%20Items/Images/PrintASCIIBorder.jpg "Print ASCII Border")