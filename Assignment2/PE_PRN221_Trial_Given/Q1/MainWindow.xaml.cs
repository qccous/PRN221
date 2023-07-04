using Q1.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Q1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PRN221_TrialContext _contex;
        public MainWindow(PRN221_TrialContext context)
        {
            InitializeComponent();
            _contex = context;
            HandleBeforeLoaded();
        }

        public void UpdateGridView() {
            ListEmployee.ItemsSource = _contex.Employees.ToList();
        }

        private void HandleBeforeLoaded()
        {
            UpdateGridView();
        }

        public Employee GetEmployee() {
            try
            {
                return new Employee
                {
                    Id = string.IsNullOrEmpty(employeeid.Text) ? 0 : int.Parse(employeeid.Text),
                    Name = name.Text,
                    Gender = male.IsChecked == true ? "Male" : "Female",
                    Phone = phone.Text,
                    Dob = dob.SelectedDate,
                    Idnumber = id.Text
                };
            }
            catch(Exception ex) {
                MessageBox.Show("Cannot get employee", "Get Employee");
            }
            return null;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            employeeid.Text = string.Empty;
            name.Text = string.Empty;
            male.IsChecked = true;
            phone.Text = string.Empty;
            dob.SelectedDate = null;
            id.Text = string.Empty;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var employee = GetEmployee();
                if (employee != null)
                {
                    employee.Id = 0;
                    _contex.Employees.Add(employee);
                    _contex.SaveChanges();
                    UpdateGridView();
                    MessageBox.Show("Insert success", "Add Employee");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Insert failed", "Add Employee");
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var employee = GetEmployee();
                if (employee != null)
                {
                    var oldEmployee = _contex.Employees.FirstOrDefault(x => x.Id == employee.Id);
                    if (oldEmployee != null)
                    {
                        oldEmployee.Name = employee.Name;
                        oldEmployee.Gender = employee.Gender;
                        oldEmployee.Dob = employee.Dob;
                        oldEmployee.Phone = employee.Phone;
                        oldEmployee.Idnumber = employee.Idnumber;
                        _contex.Employees.Update(oldEmployee);
                        _contex.SaveChanges();
                        UpdateGridView();
                        MessageBox.Show("Edit success", "Edit Employee");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Edit failed", "Edit Employee");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var employee = GetEmployee();
                if (employee != null)
                {
                    var oldEmployee = _contex.Employees.FirstOrDefault(x => x.Id == employee.Id);
                    if (oldEmployee != null)
                    {
                        _contex.Employees.Remove(oldEmployee);
                        _contex.SaveChanges();
                        UpdateGridView();
                        MessageBox.Show("Delete success", "Delete Employee");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete failed", "Delete Employee");
            }
        }

        private void listView_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null) {
                var gender = ((Employee)item).Gender;
                if (!string.IsNullOrEmpty(gender)) {
                    if (gender.ToLower() == "female")
                    {
                        female.IsChecked = true;
                    }
                    else
                        male.IsChecked = true;
                }
            }
        }
    }
}
