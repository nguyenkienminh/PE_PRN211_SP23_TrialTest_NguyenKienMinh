using Microsoft.VisualBasic.ApplicationServices;
using StudentGroup_Service.ServiceModel;
using StudentGroup_Service.Services;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace StudentGroup_NguyenKienMinh
{
    /// <summary>
    /// Interaction logic for LoginForm.xaml
    /// </summary>
    public partial class LoginForm : Window
    {
        private readonly UserRoleService _UserRoleService;

        public LoginForm()
        {
            InitializeComponent();
            _UserRoleService = new UserRoleService();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = _UserRoleService.Login(txtuserName.Text, txtPassword.Text);

                if (user != null)
                {
                    var role = _UserRoleService.CheckRoleUser(user);

                    if(role == false)
                    {
                        System.Windows.MessageBox.Show("You have not permission to access this application!");
                    }
                    else
                    {
                        StudentManagementForm mainWindow = new StudentManagementForm();
                        this.Hide();
                        mainWindow.ShowDialog();    
                        
                    }
                }
            }catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
          
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
