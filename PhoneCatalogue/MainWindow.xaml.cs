using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PhoneCatalogue
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PhoneViewModel phoneViewModel = new PhoneViewModel("Data Source=sqlsrv;Initial Catalog=Dest;User ID=student;Password=1;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        public MainWindow()
        {
            InitializeComponent();

            DataContext = phoneViewModel;
            
        }

        private void CommitBtn_Click(object sender, RoutedEventArgs e)
        {
            phoneViewModel.CommitChanges();
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            phoneViewModel.CreatePhone();
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            phoneViewModel.DeletePhone();
        }
    }
}
