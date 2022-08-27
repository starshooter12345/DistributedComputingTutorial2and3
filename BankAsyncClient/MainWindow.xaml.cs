using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
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
using BankingBusinessTier;
using BankingDatabase;

namespace BankAsyncClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public delegate DbGenerator Search(string value);
    public partial class MainWindow : Window
    {
        private BusinessServerInterface foob;
        private string searchvalue;
        public MainWindow()
        {
            InitializeComponent();
            ChannelFactory<BusinessServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            //setting the URL and creating the connection:
            string URL = "net.tcp://localhost:8200/BankBusinessService";
            try { 
            foobFactory = new ChannelFactory<BusinessServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
            //Showing how many entries are there in the DB
            Total_Num.Text = foob.GetNumEntries().ToString();
            }
            catch
            {
                MessageBox.Show("Cannot connect to server, please restart the server and then start client again");
                return;
            }

        }

        
        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            int index = 0;
            string fName = "", lName = "", img = "";
            int bal = 0;
            uint acct = 0, pin = 0;

            try
            {
                index = Int32.Parse(Index_Num.Text);
            }
            catch
            {
                MessageBox.Show("Enter a number!");
                return;
            }

            try
            {
                if (index > foob.GetNumEntries() || index <= 0)
                {
                    MessageBox.Show("Index is out of range!");
                    return;
                }
                else
                {
                    foob.GetValuesForEntry(index, out acct, out pin, out bal, out fName, out lName, out img);
                }

                Image Image = new Image();
                Image.Width = 100;

                BitmapImage bmpImage = new BitmapImage();

                bmpImage.BeginInit();
                Console.WriteLine(img);
                bmpImage.UriSource = new Uri(img);
                bmpImage.DecodePixelWidth = 100;
                bmpImage.EndInit();
                Image.Source = bmpImage;


                FirstNameBox.Text = fName;
                LastNameBox.Text = lName;
                Balance_Box.Text = bal.ToString("C");
                AccountNoBox.Text = acct.ToString();
                PinBox.Text = pin.ToString("D4");
                image.Source = Image.Source;

            }
            catch
            {
                MessageBox.Show("Cannot connect to the server,please restart the server and then start the client again");
                return;
            }
        }

        private async void SearchButthon_Click(object sender, RoutedEventArgs e)
        {

            searchvalue = SearchBox.Text;
            try
            {
                
                if (Regex.IsMatch(SearchBox.Text, @"^[A-Za-z]+$"))
                {
                    searchvalue = (SearchBox.Text);

                }
                else
                {
                    
                    
                    MessageBox.Show("Enter a valid name!");
                    return;

                }
            }
            catch
            {
                MessageBox.Show("Enter a valid name!");

            }
              


        

          Task<DbGenerator> task = new Task<DbGenerator>(SearchDB);
          task.Start();
          statusLabel.Content = "Searching starts.....";
            UpdateProgressBar(10);
          DbGenerator dbgener = await task;
            UpdateGui(dbgener);
            UpdateProgressBar(100);
          statusLabel.Content = "Searching ends....";





        }

        private DbGenerator SearchDB()
        {
            string fName = null;
            string lName = null;
            int bal = 0;
            string img = null;
            uint acct = 0;
            uint pin = 0;
            foob.GetValuesForSearch(searchvalue, out acct, out pin, out bal, out fName, out lName, out img);
            if (acct != 0)
            {
                DbGenerator dbgen = new DbGenerator();
                dbgen.firstName = fName;
                dbgen.lastName = lName;
                dbgen.pin = pin;
                dbgen.img = img;
                dbgen.acctNo = acct;
                dbgen.balance = bal;
                return dbgen;
            }
            return null;


        }

        private void UpdateGui( DbGenerator dbgener)
        {
            Image Image = new Image();
            Image.Width = 100;
            BitmapImage bmpImage = new BitmapImage();
            bmpImage.BeginInit();
            Console.WriteLine(dbgener.img);
            bmpImage.UriSource = new Uri(dbgener.img);
            bmpImage.DecodePixelWidth = 100;
            bmpImage.EndInit();
            Image.Source = bmpImage;

            


           
            FirstNameBox.Text = dbgener.firstName;
            LastNameBox.Text = dbgener.lastName;
            PinBox.Text = dbgener.pin.ToString();
            AccountNoBox.Text = dbgener.acctNo.ToString();
            Balance_Box.Text = dbgener.balance.ToString();
            image.Source = Image.Source;


        }

        private void UpdateProgressBar(int value)
        {
            progressBar.Dispatcher.Invoke(new Action(() => progressBar.Value = value));
        }
       

    }
}
