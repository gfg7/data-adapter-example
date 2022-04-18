using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DB db;
        public MainWindow()
        {
            InitializeComponent();
            db = new DB();
            if (!DB.TryConn())//проверка соединения
            {
                App.Current.Shutdown();
            }
            //получаем данные в таблице по запросу
            DataTable dataTable = DB.AdapterReader("SELECT * FROM new_schema.new_table;");
            //привязка к таблице данных из бд
            grid1.ItemsSource = dataTable.AsDataView();
            grid2.ItemsSource = dataTable.AsDataView();
            cmb.ItemsSource = dataTable.AsDataView();
            list.ItemsSource = dataTable.AsDataView();
        }

    }

    internal class DB
    {
        public static MySqlConnection connection;
        private static MySqlCommand mySqlCommand;
        private static MySqlDataAdapter adapter;

        public DB()
        {
            //строка соединения
            connection = new MySqlConnection("server=localhost;port=3306;database=new_schema;user id=root;password=qaz123;");
        }
        //проверка соединения
        public static bool TryConn()
        {
            try
            {
                connection.Open();
                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        //для выполнения запросов insert\delete\update
        public static int Execute(string query)
        {
            int result = -1;
            mySqlCommand = new MySqlCommand(query, connection);
            try
            {
                connection.Open();
                result = mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (connection.State != System.Data.ConnectionState.Closed)
                    connection.Close();
            }
            return result;
        }
        //чтение через адаптер
        public static DataTable AdapterReader(string query)
        {
            adapter = new MySqlDataAdapter(query, connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            return dataTable;
        }
    }
}
