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
            hidePanel(subpanel_symptoms);
            hidePanel(subpanel_conditions);
            hidePanel(subpanel_vaccination);
            hidePanel(subpanel_alert);
            // Extend to support Windows-1252 using System.Text.Encoding.CodePages (For ExcelDataReader to read Excel file)           
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        #region LOADING DATA

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
            students_grid.ItemsSource = initializeTable("all").AsDataView();
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
        private void debug_getcolindex()
        {
            int index = 0;
            string colname;
            foreach (string col in columnNamesList)
            {
                index = Array.IndexOf(columnNamesList, col);
                colname = col;

                MessageBox.Show(col + " " + index);
            }
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



        private void cb_HideAll_Checked(object sender, RoutedEventArgs e)
        {
            hideColumn("all");
        }

        private void cb_HideAll_Unchecked(object sender, RoutedEventArgs e)
        {
            showColumn("all");
        }

        private void cb_Symptoms_Checked(object sender, RoutedEventArgs e)
        {
            showColumn("symptoms");
            addToFilter("symptoms", "Diarrhea");
        }
        private void cb_Symptoms_Unhecked(object sender, RoutedEventArgs e)
        {
        }

        private void mainstuff_Close(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btn_ShowSymptoms_Click(object sender, RoutedEventArgs e)
        {
            RevealHideSubPanel(subpanel_symptoms, "Symptoms", btn_ShowSymptoms);
        }

        private void btn_ShowCondition_Click(object sender, RoutedEventArgs e)
        {
            RevealHideSubPanel(subpanel_conditions, "Condition", btn_ShowCondition);
        }

        private void btn_ShowVaccination_Click(object sender, RoutedEventArgs e)
        {
            RevealHideSubPanel(subpanel_vaccination, "Vaccination", btn_ShowVaccination);
        }

        private void btn_ShowAlert_Click(object sender, RoutedEventArgs e)
        {
            RevealHideSubPanel(subpanel_alert, "Alert Level", btn_ShowAlert);
        }

        private void cb_BoosterShot_Checked(object sender, RoutedEventArgs e)
        {

        }

    }
}
