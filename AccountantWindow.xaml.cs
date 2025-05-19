using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Globalization;
using MedicalLaboratory.Models;

namespace MedicalLaboratory
{
    public partial class AccountantWindow : Window
    {
        private User currentUser;

        public AccountantWindow(User user)
        {
            InitializeComponent();
            currentUser = user;
            lblUserName.Text = user.Name;

            LoadInsuranceCompanies();

            dpStartDate.SelectedDate = DateTime.Today.AddMonths(-1);
            dpEndDate.SelectedDate = DateTime.Today;
            dpBillStartDate.SelectedDate = DateTime.Today.AddMonths(-1);
            dpBillEndDate.SelectedDate = DateTime.Today;
        }

        private void LoadInsuranceCompanies()
        {
            using (var db = new MedicalLaboratoryEntities())
            {
                cmbInsuranceCompanies.ItemsSource = db.InsuranceCompanies.ToList();
                cmbInsuranceCompanies.DisplayMemberPath = "Name";
                if (cmbInsuranceCompanies.Items.Count > 0)
                    cmbInsuranceCompanies.SelectedIndex = 0;
            }
        }

        private void ViewReports_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функционал просмотра отчетов будет реализован позже", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void GenerateBill_Click(object sender, RoutedEventArgs e)
        {
            if (cmbInsuranceCompanies.SelectedItem == null)
            {
                MessageBox.Show("Выберите страховую компанию", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (dpBillStartDate.SelectedDate == null || dpBillEndDate.SelectedDate == null)
            {
                MessageBox.Show("Выберите период", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DateTime startDate = dpBillStartDate.SelectedDate.Value;
            DateTime endDate = dpBillEndDate.SelectedDate.Value;

            if (startDate > endDate)
            {
                MessageBox.Show("Дата начала не может быть позже даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var insuranceCompany = (InsuranceCompany)cmbInsuranceCompanies.SelectedItem;

            try
            {
                var billData = GetBillData(insuranceCompany.InsuranceCompanyID, startDate, endDate);
                SaveBillToPdf(billData, insuranceCompany.Name, startDate, endDate);
                SaveBillToCsv(billData, insuranceCompany.Name, startDate, endDate);

                MessageBox.Show($"Счет для {insuranceCompany.Name} успешно сформирован", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при формировании счета: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private List<BillItem> GetBillData(int insuranceCompanyId, DateTime startDate, DateTime endDate)
        {
            List<BillItem> billItems = new List<BillItem>();

            using (var db = new MedicalLaboratoryEntities())
            {
                // Получаем всех пациентов данной страховой компании
                var patientsList = db.Patients
                    .Where(p => p.InsuranceCompanyID == insuranceCompanyId)
                    .ToList();

                foreach (var patient in patientsList)
                {
                    // Получаем все услуги для данного пациента за указанный период
                    var services = db.Orders
                        .Where(o => o.PatientID == patient.PatientID &&
                                   o.OrderDate >= startDate &&
                                   o.OrderDate <= endDate)
                        .Select(o => new
                        {
                            ServiceName = o.Service.ServiceName,
                            ServiceDate = o.OrderDate,
                            ServiceCost = o.Service.Price
                        })
                        .ToList();

                    if (services.Any())
                    {
                        decimal patientTotal = 0;

                        foreach (var service in services)
                        {
                            billItems.Add(new BillItem
                            {
                                PatientName = $"{patient.LastName} {patient.FirstName} {patient.MiddleName}",
                                ServiceName = service.ServiceName,
                                ServiceDate = service.ServiceDate,
                                ServiceCost = service.ServiceCost
                            });

                            patientTotal += service.ServiceCost;
                        }

                        // Добавляем итог по пациенту
                        billItems.Add(new BillItem
                        {
                            PatientName = $"{patient.LastName} {patient.FirstName} {patient.MiddleName}",
                            ServiceName = "Итого по пациенту:",
                            ServiceCost = patientTotal
                        });
                    }
                }
            }

            return billItems;
        }

        private void SaveBillToPdf(List<BillItem> billItems, string insuranceCompanyName, DateTime startDate, DateTime endDate)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF файлы (*.pdf)|*.pdf",
                FileName = $"Счет_{insuranceCompanyName}_{startDate:ddMMyyyy}_{endDate:ddMMyyyy}.pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    Document document = new Document();
                    PdfWriter writer = PdfWriter.GetInstance(document, stream);
                    document.Open();

                    // Шрифты
                    BaseFont baseFont = BaseFont.CreateFont("c:\\windows\\fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                    Font titleFont = new Font(baseFont, 16, Font.BOLD);
                    Font headerFont = new Font(baseFont, 12, Font.BOLD);
                    Font regularFont = new Font(baseFont, 10);

                    // Заголовок
                    document.Add(new iTextSharp.text.Paragraph($"Счет для страховой компании: {insuranceCompanyName}", titleFont));
                    document.Add(new iTextSharp.text.Paragraph($"Период: {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}", headerFont));
                    document.Add(new iTextSharp.text.Paragraph(" "));

                    // Таблица с данными
                    PdfPTable table = new PdfPTable(4);
                    table.WidthPercentage = 100;

                    // Заголовки таблицы
                    table.AddCell(new Phrase("ФИО пациента", headerFont));
                    table.AddCell(new Phrase("Услуга", headerFont));
                    table.AddCell(new Phrase("Дата услуги", headerFont));
                    table.AddCell(new Phrase("Стоимость", headerFont));

                    // Данные
                    decimal total = 0;
                    foreach (var item in billItems)
                    {
                        table.AddCell(new Phrase(item.PatientName, regularFont));
                        table.AddCell(new Phrase(item.ServiceName, regularFont));
                        table.AddCell(new Phrase(item.ServiceDate.HasValue ? item.ServiceDate.Value.ToString("dd.MM.yyyy") : "", regularFont));
                        table.AddCell(new Phrase(item.ServiceCost.ToString("C", CultureInfo.CreateSpecificCulture("ru-RU")), regularFont));

                        if (!item.ServiceName.Contains("Итого"))
                        {
                            total += item.ServiceCost;
                        }
                    }

                    document.Add(table);
                    document.Add(new iTextSharp.text.Paragraph(" "));
                    document.Add(new iTextSharp.text.Paragraph($"Итоговая стоимость: {total.ToString("C", CultureInfo.CreateSpecificCulture("ru-RU"))}", headerFont));

                    document.Close();
                }
            }
        }

        private void SaveBillToCsv(List<BillItem> billItems, string insuranceCompanyName, DateTime startDate, DateTime endDate)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV файлы (*.csv)|*.csv",
                FileName = $"Счет_{insuranceCompanyName}_{startDate:ddMMyyyy}_{endDate:ddMMyyyy}.csv"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName, false, System.Text.Encoding.UTF8))
                {
                    writer.WriteLine($"Счет для страховой компании: {insuranceCompanyName}");
                    writer.WriteLine($"Период: {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}");
                    writer.WriteLine();
                    writer.WriteLine("ФИО пациента;Услуга;Дата услуги;Стоимость");

                    decimal total = 0;
                    foreach (var item in billItems)
                    {
                        writer.WriteLine($"{item.PatientName};{item.ServiceName};{(item.ServiceDate.HasValue ? item.ServiceDate.Value.ToString("dd.MM.yyyy") : "")};{item.ServiceCost.ToString("C", CultureInfo.CreateSpecificCulture("ru-RU"))}");

                        if (!item.ServiceName.Contains("Итого"))
                        {
                            total += item.ServiceCost;
                        }
                    }

                    writer.WriteLine();
                    writer.WriteLine($"Итоговая стоимость;{total.ToString("C", CultureInfo.CreateSpecificCulture("ru-RU"))}");
                }
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
    }

    public class BillItem
    {
        public string PatientName { get; set; }
        public string ServiceName { get; set; }
        public DateTime? ServiceDate { get; set; }
        public decimal ServiceCost { get; set; }
    }
}
