using StudentGroup_Repository.Models;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StudentGroup_NguyenKienMinh
{
    /// <summary>
    /// Interaction logic for StudentManagementForm.xaml
    /// </summary>
    public partial class StudentManagementForm : Window
    {
        private readonly StudentService _studentService;
        private string userNameMain;
        private string passphraseMain;

        public StudentManagementForm()
        {
            InitializeComponent();
            _studentService = new StudentService();
            
        }

        private void dtgStudentTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public void LoadStudentGridView()
        {
            try
            {
                List<StudentModel> students = _studentService.GetAllStudents().ToList();
                dtgStudentTable.AutoGenerateColumns = true;
                dtgStudentTable.ItemsSource = students.ToList();
            }catch (Exception ex)   
            {
                System.Windows.MessageBox.Show(ex.Message); 
            }
            
        }

        private void dtgStudentTable_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnSearchYear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var list = _studentService.SearchDateOfBird(txtYearStart.Text, txtYearEnd.Text).ToList();

                if(!list.Any()) 
                {
                    System.Windows.MessageBox.Show($"Danh sách tìm kiếm không tồn tại");
                }
                else
                {
                    dtgStudentTable.AutoGenerateColumns = true;
                    dtgStudentTable.ItemsSource = list.ToList();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"{ex.Message}");    
            }
        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            txtYearStart.Text = "";
            txtYearEnd.Text = "";
            LoadStudentGridView();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddStudentForm addStudentForm = new AddStudentForm();
            this.Close();
            addStudentForm.ShowDialog();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dtgStudentTable.SelectedItem != null)
            {
                var selected = dtgStudentTable.SelectedItem as StudentModel;

                if (selected != null)
                {
                    UpdateStudentForm form = new UpdateStudentForm(selected.Id);
                    this.Close();
                    form.ShowDialog();
                }
            }
            else
            {
                System.Windows.MessageBox.Show($"Vui lòng chọn 1 hàng");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if(dtgStudentTable.SelectedItem != null)
            {
                var selected = dtgStudentTable.SelectedItem as StudentModel;

                if (selected != null)
                {
                    DialogResult result = (DialogResult)System.Windows.MessageBox.Show("Confrim Delete", "Do you want to delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if(result == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                    else
                    {
                        var delete = _studentService.DeleteStudent(selected.Id);

                        if (delete)
                        {
                            System.Windows.MessageBox.Show($"Học sinh đã dược xóa thành công");
                            LoadStudentGridView();
                        }
                        else
                        {
                            System.Windows.MessageBox.Show($"Học sinh đã dược xóa thất bại");
                        }
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Vui lòng chọn 1 hàng");
            }
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            this.Close();
            loginForm.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadStudentGridView();
        }
    }
}
