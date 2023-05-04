//using static System.Net.Mime.MediaTypeNames;
using System.Data;
//using System.Drawing;
using System.Reflection;
using System.Data.SqlClient;
using System.Windows;

namespace UtilityLibrary {

    public class Utility {

        public class Query {

            /// <summary>
            /// <para>Get a table from the delcared query.</para>
            /// <para>'query': The command that is going to be executed.</para>
            /// </summary>
            public static bool GetDataTable(out DataTable dt, string query) {

                dt = new();

                try {
                    SqlConnection con = General.GetDBConnection("Connection");
                    SqlCommand cmd = new(query, con);

                    con.Open();
                    SqlDataAdapter da = new(cmd);
                    con.Close();
                    da.Fill(dt);
                    return true;
                } catch {
                    return false;
                }
            }
            public static bool GetDataTable(out DataTable dt, out string message, string query) {

                dt = new();

                try {
                    SqlConnection con = General.GetDBConnection("Connection");
                    SqlCommand cmd = new(query, con);

                    con.Open();
                    SqlDataAdapter da = new(cmd);
                    con.Close();
                    da.Fill(dt);
                    message = "";
                    return true;
                } catch (Exception ex) {
                    message = ex.Message;
                    return false;
                }
            }

            /// <summary>
            /// <para>Execute Transactions as UPDATE, INSERT and DELETE</para>
            /// <para>'query': The command that is going to be executed.</para>
            /// </summary>
            public static bool Transaction(out string message, string query) {

                try {
                    SqlConnection con = General.GetDBConnection("Connection");
                    SqlCommand cmd = new(query, con);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    message = "";
                    return true;
                } catch (Exception ex) {
                    message = ex.Message;
                    //MessageBox.Show(ex.Message, "Error Transaction 1.0");
                    return false;
                }
            }

            /// <summary>
            /// <para> Execute Transaction UPDATE with value</para>
            /// <para>'query': The command that is going to be executed.</para>
            /// <para>'value': The value that will be inserted in the database</para>
            /// </summary>
            //public static bool Transaction(string query, string value) {

            //    try {
            //        SqlConnection con = General.GetDBConnection("Connection");
            //        SqlCommand cmd = new(query, con);
            //        cmd.Parameters.AddWithValue("@Value", value);
            //        con.Open();
            //        cmd.ExecuteNonQuery();
            //        con.Close();
            //        return true;
            //    } catch (Exception ex) {
            //        MessageBox.Show(ex.Message, "Error Transaction 1.1");
            //        return false;
            //    }
            //}
        }

        public class General {
            // SQL UTILITY
            /// <summary>
            /// <para>Get the connection Data Base from the next default value</para>
            /// <para>Default value: "Data Source=???; Initial Catalog=???; User Id=???; Password=???"</para>
            /// </summary>
            public static SqlConnection GetDBConnection(string s) {
                return new SqlConnection(s);
            }

            /// <summary>
            /// <para>Get the max value of the query.</para>
            /// <para>Example: $"SELECT MAX({ colName }) AS { colName } FROM { tableName }"</para>
            /// <para>If the return value is null, there is an error.</para>
            /// </summary>
            public static long? MaxValue(string colName, string tableName) {

                string query = $"SELECT MAX({ colName }) AS { colName } FROM { tableName }";

                if (Query.GetDataTable(out DataTable dt, query)) {
                    return dt.Rows[0].Field<long?>(colName) != null ? dt.Rows[0].Field<long>(colName) + 1 : 0;
                } else {
                    return null;
                }
            }

            // TIME FORMATS UTILITY
            /// <summary>
            /// <para>Get the Pacific Time with the format MM/dd/yyyy HH:mm.</para>
            /// </summary>
            public static string GetPacificTime() {
                TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                DateTime datetime = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
                return datetime.ToString("MM/dd/yyyy HH:mm");
            }

            // VALIDATION FORMAT UTILITY
            /// <summary>
            /// <para>Detect if the char 'key' is a number.</para>
            /// </summary>
            public static bool IsNumber(char key) {

                foreach (char c in "0123456789")
                    if (c == key) return true;

                return false;
            }

            /// <summary>
            /// <para>Detect if the an string number already has a dot.</para>
            /// <para>To avoid the income of the new dot.</para>
            /// </summary>
            public static bool HasDot(string s, char key) {

                foreach (char c in s) {
                    if (c == '.') return false;
                }
                if (key != '.') return false;

                return true;
            }

            /// <summary>
            /// <param>Detect if a string number has less than 3 decimals.</param>
            /// </summary>
            public static bool LessThreeDecimals(string number) {

                int count = 0;
                for (int i = number.Length - 1; i > -1; --i) {
                    if (number[i] == '.') return count < 3;
                    count++;
                }

                return true;
            }

            // CONSTRUCTION WINDOW UTILITY
            /// <summary>
            /// <param>Generate a new window by name.</param>
            /// </summary>
            //public static void InitializeWindow(string WndName) {

            //    WndPrincipal WP = new();
            //    foreach (Form F in Application.OpenForms) {
            //        if (F.Name == "WndPrincipal") WP = (F as WndPrincipal)!;
            //    }
            //    foreach (Form mdi in WP.MdiChildren) mdi.Close();

            //    var f = Assembly.GetExecutingAssembly().GetTypes()
            //        .Where(a => a.BaseType == typeof(Form) && a.Name == WndName)
            //        .FirstOrDefault();

            //    if (f == null) return; // If there is no form with the given frmname

            //    Form Wnd = (Form)Activator.CreateInstance(f)!;

            //    Wnd.ControlBox = false;
            //    Wnd.FormBorderStyle = FormBorderStyle.None;
            //    Wnd.StartPosition = FormStartPosition.CenterParent;
            //    Wnd.WindowState = FormWindowState.Maximized;
            //    Wnd.MdiParent = WP;
            //    Wnd.Left = 0;
            //    Wnd.Top = 0;
            //    Wnd.Dock = DockStyle.Fill;
            //    Wnd.Show();
            //    GC.Collect();
            //}

            /// <summary>
            /// Get the ID of the connected User in the application.
            /// </summary>
            //public static int GetUserID() {

            //    foreach (Form F in Application.OpenForms) {
            //        if (F.Name == "WndPrincipal") return (F as WndPrincipal)!.UserName;
            //    }
            //    return -1;
            //}


            /// <summary>
            /// Change the properties of DataGridView to custom values.
            /// </summary>
            /// <param name="dgv"></param>
            //public static DataGridView InitializeDataGridView(DataGridView dgv) {

            //    dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            //    dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            //    dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            //    dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            //    dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //    dgv.BorderStyle = BorderStyle.None;
            //    dgv.Anchor = AnchorStyles.None;
            //    //dgv.Dock = DockStyle.;
            //    //dgv.SortedColumn.SortMode = DataGridViewColumnSortMode.NotSortable;

            //    dgv.EnableHeadersVisualStyles = false;
            //    dgv.RowHeadersVisible = false;
            //    dgv.ShowCellErrors = false;
            //    dgv.ShowCellToolTips = false;
            //    dgv.ShowEditingIcon = false;
            //    dgv.ShowRowErrors = false;

            //    dgv.AllowUserToAddRows = false;
            //    dgv.AllowUserToDeleteRows = false;
            //    dgv.AllowUserToResizeColumns = false;
            //    dgv.AllowUserToResizeRows = false;

            //    dgv.BackgroundColor = Color.White;
            //    return dgv;
            //}

            //public static DataGridView CustomDataGridView(DataGridView dgv, Dictionary<string, ColumnProperties> ColumnsProperties) {
            //    int i = 0;
            //    int DTGWidth = 0;
            //    foreach (var v in ColumnsProperties) {
            //        ColumnProperties CP = v.Value;
            //        dgv.Columns[i].HeaderText = CP.DisplayedName;
            //        dgv.Columns[i].ReadOnly = CP.ReadOnly;
            //        dgv.Columns[i].DefaultCellStyle.BackColor = ColorTranslator.FromHtml(CP.DataBackColorRGB);
            //        dgv.Columns[i].DefaultCellStyle.ForeColor = ColorTranslator.FromHtml(CP.DataForeColorRGB);
            //        dgv.Columns[i].Width = CP.Width;
            //        dgv.Columns[i].DefaultCellStyle.Alignment = (DataGridViewContentAlignment)CP.AlignmentRows;
            //        dgv.Columns[i].HeaderCell.Style.Alignment = (DataGridViewContentAlignment)CP.AlignmentHeaders;
            //        dgv.Columns[i].HeaderCell.Style.BackColor = ColorTranslator.FromHtml(CP.HeaderBackColorRGB);
            //        dgv.Columns[i].HeaderCell.Style.ForeColor = ColorTranslator.FromHtml(CP.HeaderForeColorRGB);

            //        if (CP.FormatCell != "NULL") {
            //            dgv.Columns[i].DefaultCellStyle.Format = CP.FormatCell;
            //        }
            //        DTGWidth += CP.Width + 1;
            //        i++;
            //    }

            //    GetScreenSize(out int ScreenWidth, out int HeightWidth);
            //    dgv.Width = ScreenWidth <= DTGWidth ? ScreenWidth : DTGWidth;
            //    return dgv;
            //}

            //public class ColumnProperties {
            //    public string NameID { get; set; }
            //    public bool ReadOnly { get; set; }
            //    public string DisplayedName { get; set; }
            //    public string DataType { get; set; } // Number, Text, Money ($), Date (31-12-2023)
            //    public string DataBackColorRGB { get; set; }
            //    public string DataForeColorRGB { get; set; }
            //    public int DataFontSize { get; set; }
            //    public string HeaderForeColorRGB { get; set; }
            //    public string HeaderBackColorRGB { get; set; }
            //    public int HeaderFontSize { get; set; }
            //    public int Width { get; set; }
            //    public int AlignmentRows { get; set; }
            //    public int AlignmentHeaders { get; set; }
            //    public string FormatCell { get; set; }


            //    public ColumnProperties(bool rdOnly, string nameid, string DspName, string DtTp, int W, string BG, string FC, int AlgR, int AlgH, string HFCrgb, string BFCrgb, int HFS, int DFS, string formatCell) {
            //        NameID = nameid;
            //        ReadOnly = rdOnly;
            //        DisplayedName = DspName;
            //        DataType = DtTp;
            //        Width = W;
            //        DataBackColorRGB = BG;
            //        DataForeColorRGB = FC;
            //        HeaderBackColorRGB = HFCrgb;
            //        HeaderForeColorRGB = BFCrgb;
            //        AlignmentRows = AlgR;
            //        AlignmentHeaders = AlgH;
            //        HeaderFontSize = HFS;
            //        DataFontSize = DFS;
            //        FormatCell = formatCell;

            //    }
            //}

            //// References https://stackoverflow.com/questions/254197/how-can-i-get-the-active-screen-dimensions
            ///// <summary>
            ///// Get screen width and screen height in pixels
            ///// </summary>
            //public static void GetScreenSize(out int ScreenWidth, out int ScreenHeight) {
            //    ScreenWidth = Screen.PrimaryScreen.Bounds.Width;
            //    ScreenHeight = Screen.PrimaryScreen.Bounds.Height;
            //}
        }

    }
}