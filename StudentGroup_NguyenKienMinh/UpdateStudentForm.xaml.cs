using StudentGroup_Repository.Models;
using StudentGroup_Service.ServiceModel;
using StudentGroup_Service.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for UpdateStudentForm.xaml
    /// </summary>
    public partial class UpdateStudentForm : Window
    {
        int StudentId;
        private readonly StudentService _studentService;
        private readonly StudentGroupService _studentGroupService;

        public UpdateStudentForm(int Id)
        {
            InitializeComponent();
            StudentId = Id;
            _studentService = new StudentService();
            _studentGroupService = new StudentGroupService();
            
        }

        public void LoadDataStudents()
        {
            var model = _studentService.GetStudentsById(StudentId);

            txtFullName.Text = model.FullName;
            txtEmail.Text = model.Email;
            dpDateOfBirth.Text = model.DateOfBirth.Value.Date.ToString("dd/MM/yyyy");
            LoadStudentGroupComboBox(model.GroupName);
        }

        public void LoadStudentGroupComboBox(string groupName)
        {
            var list = _studentGroupService.GetAll();
            cbmGroupName.ItemsSource = list;
            cbmGroupName.SelectedValuePath = "GroupName";
            cbmGroupName.DisplayMemberPath = "GroupName";
            cbmGroupName.SelectedValue = groupName;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string groupName = "";

               StudentModel student = new StudentModel() 
               { 
                    FullName = txtFullName.Text,    
                    Email = txtEmail.Text,
               };

                if (!dpDateOfBirth.SelectedDate.HasValue)
                {
                    System.Windows.MessageBox.Show("Date must required");
                    return;
                }

                DateTime time = (DateTime) dpDateOfBirth.SelectedDate;

                string DateOfBirth = time.ToString("dd/MM/yyyy");

                var check = _studentService.CheckValidStudent(student, DateOfBirth);

                if (check)
                {
                    var dateUpdate = DateTime.ParseExact(DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    #region GroupName

                    if(cbmGroupName.SelectedItem != null)
                    {
                        var Group = cbmGroupName.SelectedItem as StudentGroup;

                        if(Group != null)
                        {
                            groupName = Group.GroupName;
                        }
                    }

                    #endregion

                    student.Id = StudentId;
                    student.DateOfBirth = dateUpdate;
                    student.GroupName = groupName;

                    var update = _studentService.UpdateStudent(student);

                    if (update)
                    {
                        System.Windows.MessageBox.Show("Update Student Success");
                        StudentManagementForm form = new StudentManagementForm();
                        this.Close();    
                        form.ShowDialog();  
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            StudentManagementForm frm = new StudentManagementForm();
            this.Close();
            frm.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataStudents();
        }
    }
}
