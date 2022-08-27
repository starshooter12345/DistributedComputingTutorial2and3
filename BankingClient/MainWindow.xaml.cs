//using BankingServer;
using BankingDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
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

namespace BankingClient
{
    public delegate DbGenerator Search(string value);//delegate added for searching
    
    
    public partial class MainWindow : Window
    {
        private BankingBusinessTier.BusinessServerInterface foob;
        private Search search;
        public MainWindow()
        {
            InitializeComponent();

            ChannelFactory<BankingBusinessTier.BusinessServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            string URL = "net.tcp://localhost:8200/BankBusinessService";
            //Below is creating the connection between the client and server
            try
            {
                foobFactory = new ChannelFactory<BankingBusinessTier.BusinessServerInterface>(tcp, URL);
                foob = foobFactory.CreateChannel();
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
            string fName = "", lName = "" , img="";
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

        private void SearchButthon_Click(object sender, RoutedEventArgs e)
        {
            
            search = SearchDB;
            AsyncCallback callback;
            callback = this.OnSearchCompletion;
            IAsyncResult result = search.BeginInvoke(SearchBox.Text, callback, null);


        }
        private DbGenerator SearchDB(string value)
        {
            string fName = null;
            string lName = null;
            int bal = 0;
            string img = null;
            uint acct = 0;
            uint pin = 0;
            foob.GetValuesForSearch(value, out acct, out pin, out bal, out fName, out lName, out img);
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
            }return null;
        }

        private void UpdateGui(DbGenerator dbgen)
        {
            FirstNameBox.Dispatcher.Invoke(new Action(() => FirstNameBox.Text = dbgen.firstName));
            LastNameBox.Dispatcher.Invoke(new Action(() => LastNameBox.Text = dbgen.lastName));
            Balance_Box.Dispatcher.Invoke(new Action(() => Balance_Box.Text = dbgen.balance.ToString()));
            PinBox.Dispatcher.Invoke(new Action(()=>PinBox.Text = dbgen.pin.ToString()));
            AccountNoBox.Dispatcher.Invoke(new Action(() => AccountNoBox.Text = dbgen.acctNo.ToString()));

        }

        private void OnSearchCompletion(IAsyncResult asyncResult)
        {
            DbGenerator dbgen2 = null;
            Search search = null;
            AsyncResult asyncobj = (AsyncResult)asyncResult;
            if(asyncobj.EndInvokeCalled == false)
            {
                search = (Search)asyncobj.AsyncDelegate;
                dbgen2 = search.EndInvoke(asyncobj);
                UpdateGui(dbgen2);


            }
            asyncobj.AsyncWaitHandle.Close();

        }
    }
}
