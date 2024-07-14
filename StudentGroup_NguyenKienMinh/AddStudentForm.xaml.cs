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
    /// Interaction logic for AddStudentForm.xaml
    /// </summary>
    public partial class AddStudentForm : Window
    {
        private readonly StudentService _studentService;
        private readonly StudentGroupService _studentGroupService;

        public AddStudentForm()
        {
            InitializeComponent();
            _studentService = new StudentService();
            _studentGroupService = new StudentGroupService();
            LoadStudentGroupComboBox();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string groupName = "";

                var model = new StudentModel()
                {
                    Email = txtEmail.Text,
                    FullName = txtFullName.Text,     
                };
                #region Group Name
                

                var selected = cbmGroupName.SelectedItem as StudentGroup;

                if (selected != null)
                {
                    groupName = selected.GroupName;
                }
                else
                {
                    throw new Exception("Group Name chưa được chọn");
                }
                #endregion  

                if (!dpDateOfBirth.SelectedDate.HasValue)
                {
                    System.Windows.MessageBox.Show("Date must required");
                    return;
                }

                DateTime time = (DateTime) dpDateOfBirth.SelectedDate;

                string birth = time.ToString("dd/MM/yyy");

                var check = _studentService.CheckValidStudent(model, birth);

                if(check)
                {
                    DateTime date = DateTime.ParseExact(birth, "dd/MM/yyyy",CultureInfo.InvariantCulture);

                    var Add = _studentService.AddStudent(new StudentModel
                    {
                        Email = txtEmail.Text,
                        FullName = txtFullName.Text,
                        DateOfBirth = date, 
                        GroupName = groupName,
                    });

                    if(Add == true)
                    {
                        System.Windows.MessageBox.Show("Thêm học sinh thành công");
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

        public void LoadStudentGroupComboBox()
        {
            var list = _studentGroupService.GetAll();
            cbmGroupName.ItemsSource = list;
            cbmGroupName.DisplayMemberPath = "GroupName";
            cbmGroupName.SelectedValuePath = "GroupName";
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            StudentManagementForm frm = new StudentManagementForm();
            this.Close();
            frm.ShowDialog();
        }
    }
}
