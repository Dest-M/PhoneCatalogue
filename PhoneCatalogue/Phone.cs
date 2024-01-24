using System;
using Microsoft.Data.SqlClient;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Prism;
using Prism.Mvvm;
using Dapper;
using Microsoft.VisualBasic.ApplicationServices;
using System.Data;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;

namespace PhoneCatalogue
{
    public class Phone : INotifyPropertyChanged
    {
        private string _title;
        private string _model;
        private int _price;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged(nameof(Title));
            }
        }
        public string Model
        {
            get { return _model; }
            set
            {
                _model = value;
                RaisePropertyChanged(nameof(Model));
            }
        }
        public int Price
        {
            get { return _price; }
            set
            {
                _price = value;
                RaisePropertyChanged(nameof(Price));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }


    public class PhoneViewModel : INotifyPropertyChanged
    {
        private readonly string _connectionString;
        
        
        Color highlight = new Color();
        private Phone selectedPhone;
        public ObservableCollection<Phone> Phones { get; set; }

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ?? (addCommand = new RelayCommand(
                    obj =>
                    {
                        Phone phone = new Phone();
                        Phones.Insert(0, phone);
                        SelectedPhone = phone;
                    }));
            }
        }

        public Phone SelectedPhone
        {
            get { return selectedPhone; }
            set
            {
                selectedPhone = value;
                RaisePropertyChanged(nameof(selectedPhone));
            }
        }

        public PhoneViewModel(string conn)
        {
            
            _connectionString = conn;
            Phones = new ObservableCollection<Phone>
            { };



            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"SELECT Title, Model, Price FROM [dbo].[Phones]"
                ;
            
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                    List<Phone> a = new List<Phone>();
                    using (IDbConnection db = new SqlConnection(_connectionString))
                    {
                        a = db.Query<Phone>("SELECT * FROM [dbo].[Phones]").ToList();
                        
                    }
                    foreach(var b in a)
                    {
                        Phones.Add(new Phone { Title = b.Title, Model = b.Model, Price = b.Price });
                    }
                    
                }
            }
                

                
            }
        

        public void CommitChanges()
        {
                var ph = Phones;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql1 = @"TRUNCATE TABLE [dbo].[Phones];";
                            
                 var sql2 = @" INSERT INTO [dbo].[Phones](Title, Model, Price) VALUES (@titlee, @mdl, @prc)"
                ;

                using (SqlCommand command = new SqlCommand(sql1, connection))
                {

                    command.ExecuteNonQuery();

                }
                    foreach (var a in ph) {
                using (SqlCommand command = new SqlCommand(sql2, connection))
                {
                        command.Parameters.AddWithValue("@titlee", a.Title);
                        command.Parameters.AddWithValue("@mdl", a.Model);
                        command.Parameters.AddWithValue("@prc", a.Price);
                    command.ExecuteNonQuery();
                    }

                }
                highlight = Color.Transparent;

            }

    }
        public void CreatePhone()
        {
            Phone ph = new Phone();
            Phones.Add(ph);
            SelectedPhone = ph;
            highlight = Color.Yellow;
        }
        public void DeletePhone()
        {
            Phone ph = SelectedPhone;
            Phones.Remove(ph);
            highlight = Color.Yellow;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
}
