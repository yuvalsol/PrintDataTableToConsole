using System;
using System.Data;
using System.IO;
using System.Text;

namespace PrintData
{
    public class PrintDataDemo
    {
        public void Run()
        {
            DataTable products = GetProducts();

            int demoCount = 1;
            PrintColumns(products, demoCount++);
            Print(products, demoCount++);
            Print_Top5(products, demoCount++);
            Print_Top5_SpecificColumns(products, demoCount++);
            Print_ManipulateDateAddedColumn(products, demoCount++);
            PrintList(products, demoCount++);
            PrintList_Top5(products, demoCount++);
            PrintList_3Columns(products, demoCount++);
            PrintList_3Columns_ManipulateDateAddedColumn_NonDefaultDelimiter(products, demoCount++);
            Print_DataView(products, demoCount++);
            Print_DataRowArray(products, demoCount++);
            PrintToStringBuilder(products, demoCount++);
            PrintToStream(products, demoCount++);
            PrintASCIIBorder(products, demoCount++);
        }

        private DataTable GetProducts()
        {
            DataTable products = new DataTable("Products");
            products.Columns.Add("Id", typeof(int)).AllowDBNull = false;
            products.Columns.Add("ProductName", typeof(string));
            products.Columns.Add("InStock", typeof(bool));
            products.Columns.Add("DateAdded", typeof(DateTime));

            Random rnd = new Random(DateTime.Now.Millisecond);
            int id = 0;
            for (int i = 0; i < 14; i++)
            {
                id = rnd.Next(id + 1, id + 100);
                products.Rows.Add(id, "Product " + id, i % 3 == 0, DateTime.Now.AddDays(id).AddHours(id).AddMinutes(id).AddSeconds(id));
            }

            return products;
        }

        private void PrintColumns(DataTable products, int demoCount)
        {
            Console.WriteLine(demoCount + ".");
            Console.WriteLine("PrintColumns");
            Console.WriteLine();

            products.PrintColumns();
        }

        private void Print(DataTable products, int demoCount)
        {
            Console.WriteLine(demoCount + ".");
            Console.WriteLine("Print");
            Console.WriteLine();

            products.Print();
        }

        private void Print_Top5(DataTable products, int demoCount)
        {
            Console.WriteLine(demoCount + ".");
            Console.WriteLine("Print top 5 with row ordinals");
            Console.WriteLine();

            products.Print(true, 5);
        }

        private void Print_Top5_SpecificColumns(DataTable products, int demoCount)
        {
            Console.WriteLine(demoCount + ".");
            Console.WriteLine("Print specific columns. Id column at begin & end");
            Console.WriteLine("Print top 5 with row ordinals");
            Console.WriteLine();

            products.Print(true, 5, "Id", "DateAdded", "InStock", "Id");
        }

        private void Print_ManipulateDateAddedColumn(DataTable products, int demoCount)
        {
            Console.WriteLine(demoCount + ".");
            Console.WriteLine("Print and manipulate DateAdded column");
            Console.WriteLine();

            products.Print((obj, row, column) =>
            {
                if (obj == DBNull.Value)
                    return null;

                if (column.ColumnName == "DateAdded")
                    return ((DateTime)obj).ToString("yyyy-MM-dd");

                return obj.ToString();
            });
        }

        private void PrintList(DataTable products, int demoCount)
        {
            Console.WriteLine(demoCount + ".");
            Console.WriteLine("PrintList");
            Console.WriteLine();

            products.PrintList();
        }

        private void PrintList_Top5(DataTable products, int demoCount)
        {
            Console.WriteLine(demoCount + ".");
            Console.WriteLine("PrintList top 5 with row ordinals");
            Console.WriteLine();

            products.PrintList(true, 5);
        }

        private void PrintList_3Columns(DataTable products, int demoCount)
        {
            Console.WriteLine(demoCount + ".");
            Console.WriteLine("PrintList with 3 columns and horizontal repeat direction");
            Console.WriteLine();

            products.PrintList(3, PrintDataExtensions.RepeatDirection.Horizontal, "Id", "ProductName");
        }

        private void PrintList_3Columns_ManipulateDateAddedColumn_NonDefaultDelimiter(DataTable products, int demoCount)
        {
            Console.WriteLine(demoCount + ".");
            Console.WriteLine("PrintList with 3 columns and horizontal repeat direction");
            Console.WriteLine("Manipulate DateAdded column");
            Console.WriteLine("Non-default delimiter");
            Console.WriteLine();

            products.PrintList(true, 0, (obj, row, column) =>
            {
                if (obj == DBNull.Value)
                    return null;

                if (column.ColumnName == "DateAdded")
                    return ((DateTime)obj).ToString("yyyy-MM-dd");

                return obj.ToString();
            }, 3, PrintDataExtensions.RepeatDirection.Horizontal, " = ", "Id", "DateAdded");
        }

        private void Print_DataView(DataTable products, int demoCount)
        {
            Console.WriteLine(demoCount + ".");
            Console.WriteLine("Print DataView with row ordinals");
            Console.WriteLine("Id % 2 = 0, DateAdded Desc");
            Console.WriteLine();

            products.DefaultView.RowFilter = "Id % 2 = 0";
            products.DefaultView.Sort = "DateAdded Desc";
            products.DefaultView.Print(true);
        }

        private void Print_DataRowArray(DataTable products, int demoCount)
        {
            Console.WriteLine(demoCount + ".");
            Console.WriteLine("Print DataRow[] with row ordinals");
            Console.WriteLine("products.Select(\"Id % 2 = 0\", \"DateAdded Desc\")");
            Console.WriteLine();

            products.Select("Id % 2 = 0", "DateAdded Desc").Print(true);
        }

        private void PrintToStringBuilder(DataTable products, int demoCount)
        {
            Console.WriteLine(demoCount + ".");
            Console.WriteLine("Print to StringBuilder (and StringBuilder to Console)");
            Console.WriteLine();

            StringBuilder builder = new StringBuilder();
            PrintDataExtensions.SetOutputStringBuilder(builder);
            products.Print();
            PrintDataExtensions.SetOutputConsole(); // revert back to Console
            Console.Write(builder.ToString());
        }

        private void PrintToStream(DataTable products, int demoCount)
        {
            Console.WriteLine(demoCount + ".");
            Console.WriteLine("Print to Stream (and Stream to Console)");
            Console.WriteLine("Default encoding is UTF8");
            Console.WriteLine();

            byte[] buffer = null;
            using (MemoryStream stream = new MemoryStream())
            {
                PrintDataExtensions.SetOutputStream(stream);
                products.Print();
                PrintDataExtensions.SetOutputConsole(); // revert back to Console
                buffer = stream.ToArray();
            }

            string output = Encoding.UTF8.GetString(buffer);
            Console.WriteLine(output);
        }

        private void PrintASCIIBorder(DataTable products, int demoCount)
        {
            Console.WriteLine(demoCount + ".");
            Console.WriteLine("Print ASCII Border");
            Console.WriteLine();

            PrintDataExtensions.ASCIIBorder();
            products.Print();
        }
    }
}
