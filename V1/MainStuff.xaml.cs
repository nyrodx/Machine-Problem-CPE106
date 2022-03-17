using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Microsoft.Data.SqlClient;
using ExcelDataReader;
namespace V1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainStuff : Window
    {
        public MainStuff()
        {
            InitializeComponent();
            // Extend to support Windows-1252 using System.Text.Encoding.CodePages (For ExcelDataReader to read Excel file)           
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        #region LOADING DATA

        #region Show Basic Columns
        private void showBasicColumns()
        {
            hideColumn("all");
            showColumn("studentnum");
            showColumn("firstname");
            showColumn("familyname");
            showColumn("middlename");
            showColumn("condition");
            showColumn("symptoms");
        }
        #endregion

        #region LOAD FROM SQL SERVER
        private DataTable initializeTable(string columns)
        {
            string mainColumns = "student_num,family_name,first_name,middle_name";
            string allColumns = "";
            if (columns == "all")
                allColumns = "*";
            else if (columns.Length > 1)
                allColumns = mainColumns + "," + columns;

            string conString = "Data Source=LAPTOP-OB9B3RFG;Initial Catalog=Students_DB;User ID=student_health_check;Password=123;TrustServerCertificate=true";
            string Query = "SELECT " + allColumns + " FROM Students_DB.dbo.students_info$";
            SqlConnection cnn = new SqlConnection(conString);
            try
            {
                // Open Connection to SQL Server
                cnn.Open();
                // Place selected data based from query
                SqlDataAdapter sqlDa = new SqlDataAdapter(Query, cnn);
                // Make a data table
                DataTable studentTable = new DataTable();
                // Fill dataset with the result of query string
                sqlDa.Fill(studentTable);
                // Fill data grid using data table
                cnn.Close();
                return studentTable;
            }
            catch (Exception x)
            {
                DataTable empty = new DataTable();
                MessageBox.Show(x.Message);
                return empty;
            }
        }
        private void btn_LoadSQL_Click(object sender, RoutedEventArgs e)
        {
            // Let program know we are in SQL mode.
            mode = 2;
            DataView gridSource = initializeTable("all").AsDataView();
            students_grid.ItemsSource = gridSource;
            showBasicColumns();
        }
        #endregion

        #region LOAD FROM EXCEL
        DataTableCollection tableCollection;
        DataTable MAINDATA;
        DataView TableForFiltering;
        private void btn_LoadExcel_Click(object sender, RoutedEventArgs e)
        {
            // Let program know we are in Excel mode.
            mode = 1;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel 97-2003 Workbook|*.xls|Excel Workbook|*.xlsx";
            if (openFileDialog.ShowDialog() == true)
            {
                using (var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                {
                    using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                        });
                        tableCollection = result.Tables;
                        MAINDATA = tableCollection[0];
                        students_grid.ItemsSource = MAINDATA.AsDataView();
                        TableForFiltering = MAINDATA.AsDataView();
                        showBasicColumns();
                    }
                }
            }
        }
        #endregion

        #endregion

        #region COLUMN DISPLAY METHODS
        string[] columnNamesList = { "studentnum", "familyname", "firstname", "middlename",
            "age", "program", "condition", "vaccination", "symptoms", "alert_level",
            "places", "transport", "history", "lastarrived" };
        public int getColIndex(string columnName)
        {
            int index = -1;
            foreach (string col in columnNamesList)
            {
                if (columnName == col)
                {
                    index = Array.IndexOf(columnNamesList, col);
                }
            }
            return index;
        }
        private void hideColumn(string columnName)
        {
            int c = -1;
            if (columnName == "all")
            {
                foreach (string colName in columnNamesList)
                {
                    c = getColIndex(colName);
                    students_grid.Columns[c].Visibility = Visibility.Hidden;
                }
            }
            else
            {
                c = getColIndex(columnName);
                students_grid.Columns[c].Visibility = Visibility.Hidden;
            }
        }
        private void showColumn(string columnName)
        {
            int c = -1;
            if (columnName == "all")
            {
                foreach (string colName in columnNamesList)
                {
                    c = getColIndex(colName);
                    students_grid.Columns[c].Visibility = Visibility.Visible;
                }
            }
            else
            {
                c = getColIndex(columnName);
                students_grid.Columns[c].Visibility = Visibility.Visible;
            }
        }
        #endregion

        #region FILTRATION METHODS

        #region MAIN VARIABLES
        List<string> queryList = new List<string>();
        string sqlQuery = "SELECT * FROM Students_DB.dbo.students_info$";
        string excelQuery = "";

        // 1 - excel mode // 2 = sql mode //
        int mode = 0;
        #endregion

        #region ADDING/REMOVING/UPDATING QUERY
        public void addToQueryList(string column, string value)
        {
            string query = column + " LIKE '%" + value + "%'";
            queryList.Add(query);
        }
        public void removeFromQueryList(string column, string value)
        {
            string query = column + " LIKE '%" + value + "%'";
            if (queryList.Contains(query))
                queryList.Remove(query);
        }
        #endregion

        #region UPDATE QUERY AND TABLE
        public void updateQuery(int mode)
        {
            // Update Query Row Filter Style
            if (mode == 1)
            {
                excelQuery = string.Join(" and ", queryList);
            }
            // Update Query SQL Style
            else
            {
                string concatQuery = string.Join(" AND ", queryList);
                sqlQuery = "SELECT * FROM Students_DB.dbo.students_info$";
                if (queryList.Count != 0)
                {
                    sqlQuery += " WHERE ";
                    sqlQuery += concatQuery;
                }
            }
        }
        public void updateTable(int mode)
        {
            if (mode == 1)
            {
                TableForFiltering.RowFilter = excelQuery;
                students_grid.ItemsSource = TableForFiltering;
            }
            else
            {
                string conString = "Data Source=LAPTOP-OB9B3RFG;Initial Catalog=Students_DB;User ID=student_health_check;Password=123;Trust Server Certificate=True";
                SqlConnection cnn = new SqlConnection(conString);
                cnn.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter(sqlQuery, cnn);
                DataTable filteredTable = new DataTable();
                sqlDa.Fill(filteredTable);
                students_grid.ItemsSource = filteredTable.AsDataView();
                cnn.Close();
            }
        }
        #endregion

        #region FILTERING METHODS
        public void addToFilter(string column, string value)
        {
            try
            {
                addToQueryList(column, value);
                updateQuery(mode);
                updateTable(mode);
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }
        public void clearFromFilter(string column, string value)
        {
            try
            {
                removeFromQueryList(column, value);
                updateQuery(mode);
                updateTable(mode);
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }
        public void checkBoxFilterOperation(string column, string value, CheckBox theButton)
        {
            string columnName = column;
            string cellVal = value;
            if (theButton.IsChecked == true)
            {
                addToFilter(column, cellVal);
            }
            else
            {
                clearFromFilter(column, cellVal);
            }
        }

        #endregion

        #endregion

        #region PANEL METHODS
        private void hidePanel(StackPanel panelName)
        {
            panelName.Visibility = Visibility.Collapsed;
        }
        private void showPanel(StackPanel panelName)
        {
            panelName.Visibility = Visibility.Visible;
        }
        private void RevealHideSubPanel(StackPanel panelName, string column, Button buttonName)
        {
            if (buttonName.Content.Equals("Show " + column))
            {
                showPanel(panelName);
                buttonName.Content = "Hide " + column;
            }
            else
            {
                hidePanel(panelName);
                buttonName.Content = "Show " + column;
            }
        }
        #endregion

        private void mainstuff_Close(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void students_grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Profile profileWindow = new Profile();
            DataGrid gd = (DataGrid)sender;
            DataRowView selectedRow = gd.SelectedItem as DataRowView;
            if (selectedRow != null)
            {
                string stdname = selectedRow["family_name"].ToString() + " " + selectedRow["first_name"].ToString() + " " + selectedRow["middle_name"].ToString();
                string history = selectedRow["history"].ToString();
                string symptoms = selectedRow["symptoms"].ToString();
                string condition = selectedRow["condition"].ToString();
                profileWindow.updateName = stdname;
                profileWindow.updateHistory = history;
                profileWindow.updateSymptoms = symptoms;
                profileWindow.updateCondition = condition;
            }
            profileWindow.Show();
        }
    }
}
