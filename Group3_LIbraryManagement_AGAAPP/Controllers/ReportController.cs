using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Group3_LIbraryManagement_AGAAPP.Data;
using OfficeOpenXml;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml.Style;
namespace Group3_LIbraryManagement_AGAAPP.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Report
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            var bookCount = await _context.Books.CountAsync();
            var studentCount = await _context.Students.CountAsync();
            var issueCount = await _context.Issues.CountAsync();
            var notIssuedCount = await _context.Issues.CountAsync(i => i.Status == "Issued");
            var unpaidPenaltiesCount = await _context.Penalties.CountAsync(p => p.PaymentStatus == "Unpaid");
            if (startDate.HasValue && endDate.HasValue)
            {
                issueCount = await _context.Issues.CountAsync(i => i.IssueDate >= startDate && i.IssueDate <= endDate);
                notIssuedCount = await _context.Issues.CountAsync(i => i.Status == "Issued" && i.IssueDate >= startDate && i.IssueDate <= endDate);
                unpaidPenaltiesCount = await _context.Penalties.CountAsync(p => p.PaymentStatus == "Unpaid" && p.PenaltyDate >= startDate && p.PenaltyDate <= endDate);
            }
            var ReportData = new
            {
                BookCount = bookCount,
                StudentCount = studentCount,
                IssueCount = issueCount,
                NotIssuedCount = notIssuedCount,
                UnpaidPenalties = unpaidPenaltiesCount
            };
            return View("Index", ReportData);
        }
        public async Task<IActionResult> ExportToExcel(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var bookCount = await _context.Books.CountAsync();
                var studentCount = await _context.Students.CountAsync();
                var issueCount = await _context.Issues.CountAsync();
                var notIssuedCount = await _context.Issues.CountAsync(i => i.Status != "Issued");
                var unpaidPenaltiesCount = await _context.Penalties.CountAsync(p => p.PaymentStatus == "Unpaid");
                if (startDate.HasValue && endDate.HasValue)
                {
                    issueCount = await _context.Issues.CountAsync(i => i.IssueDate >= startDate && i.IssueDate <= endDate);
                    notIssuedCount = await _context.Issues.CountAsync(i => i.Status != "Issued" && i.IssueDate >= startDate && i.IssueDate <= endDate);
                    unpaidPenaltiesCount = await _context.Penalties.CountAsync(p => p.PaymentStatus == "Unpaid" && p.PenaltyDate >= startDate && p.PenaltyDate <= endDate);
                }
                var data = new[]
                {
                    new { Category = "Books", Count = bookCount },
                    new { Category = "Students", Count = studentCount },
                    new { Category = "Issues", Count = issueCount },
                    new { Category = "Not Issued", Count = notIssuedCount },
                    new { Category = "Unpaid Penalties", Count = unpaidPenaltiesCount }
                };
                using var package = new ExcelPackage();
                var worksheet = package.Workbook.Worksheets.Add("Library Report");
                worksheet.Cells[1, 1].Value = "Category";
                worksheet.Cells[1, 2].Value = "Count";
                using (var headerRange = worksheet.Cells[1, 1, 1, 2])
                {
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    headerRange.Style.Font.Color.SetColor(System.Drawing.Color.White);
                    headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    headerRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }
                for (int i = 0; i < data.Length; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = data[i].Category;
                    worksheet.Cells[i + 2, 2].Value = data[i].Count;
                    using (var dataRange = worksheet.Cells[i + 2, 1, i + 2, 2])
                    {
                        dataRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        dataRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    }
                }
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                var excelBytes = package.GetAsByteArray();
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "LibraryReport.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while generating the Excel file: {ex.Message}");
            }
        }
        public async Task<IActionResult> ExportToPdf(DateTime? startDate, DateTime? endDate)
        {
            var bookCount = await _context.Books.CountAsync();
            var studentCount = await _context.Students.CountAsync();
            var issueCount = await _context.Issues.CountAsync();
            var notIssuedCount = await _context.Issues.CountAsync(i => i.Status != "Issued");
            var unpaidPenaltiesCount = await _context.Penalties.CountAsync(p => p.PaymentStatus == "Unpaid");
            if (startDate.HasValue && endDate.HasValue)
            {
                issueCount = await _context.Issues.CountAsync(i => i.IssueDate >= startDate && i.IssueDate <= endDate);
                notIssuedCount = await _context.Issues.CountAsync(i => i.Status != "Issued" && i.IssueDate >= startDate && i.IssueDate <= endDate);
                unpaidPenaltiesCount = await _context.Penalties.CountAsync(p => p.PaymentStatus == "Unpaid" && p.PenaltyDate >= startDate && p.PenaltyDate <= endDate);
            }
            var data = new[]
            {
                new { Category = "Books", Count = bookCount },
                new { Category = "Students", Count = studentCount },
                new { Category = "Issues", Count = issueCount },
                new { Category = "Not Issued", Count = notIssuedCount },
                new { Category = "Unpaid Penalties", Count = unpaidPenaltiesCount }
            };
            var tableData = data.Select(d => new[] { d.Category, d.Count.ToString() }).ToList();
            var pdfScript = $@"
                <script src='https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.5.1/jspdf.umd.min.js'></script>
                <script src='https://cdnjs.cloudflare.com/ajax/libs/jspdf-autotable/3.5.25/jspdf.plugin.autotable.min.js'></script>
                <script>
                    const {{ jsPDF }} = window.jspdf;
                    const doc = new jsPDF();
                    doc.setFontSize(18);
                    doc.text('Library System Report', 14, 16);
                    doc.autoTable({{
                        head: [['Category', 'Count']],
                        body: {JsonConvert.SerializeObject(tableData)},
                        startY: 20,
                        theme: 'grid',
                        styles: {{ fontSize: 12, cellPadding: 3 }},
                        headStyles: {{ fillColor: [22, 160, 133] }},
                        columnStyles: {{ 0: {{ cellWidth: 'auto' }}, 1: {{ cellWidth: 'auto' }} }}
                    }});
                    doc.save('LibraryReport.pdf');
                </script>";
            return Content(pdfScript, "text/html");
        }
    }
}