using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Group3_LIbraryManagement_AGAAPP.Data;
using OfficeOpenXml; // For Excel export
using System.IO;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Index()
        {
            var bookCount = await _context.Books.CountAsync();
            var studentCount = await _context.Students.CountAsync();
            var issueCount = await _context.Issues.CountAsync();
            var notIssuedCount = await _context.Issues.CountAsync(i => i.Status == "Issued");
            var unpaidPenaltiesCount = await _context.Penalties.CountAsync(p => p.PaymentStatus == "Unpaid");

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

        // Export to Excel
        public async Task<IActionResult> ExportToExcel()
        {
            var bookCount = await _context.Books.CountAsync();
            var studentCount = await _context.Students.CountAsync();
            var issueCount = await _context.Issues.CountAsync();
            var notIssuedCount = await _context.Issues.CountAsync(i => i.Status == "Issued");
            var unpaidPenaltiesCount = await _context.Penalties.CountAsync(p => p.PaymentStatus == "Unpaid");

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

            // Add headers
            worksheet.Cells[1, 1].Value = "Category";
            worksheet.Cells[1, 2].Value = "Count";

            // Add data
            for (int i = 0; i < data.Length; i++)
            {
                worksheet.Cells[i + 2, 1].Value = data[i].Category;
                worksheet.Cells[i + 2, 2].Value = data[i].Count;
            }

            // Auto-fit columns
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            // Return Excel file
            var stream = new MemoryStream(package.GetAsByteArray());
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "LibraryReport.xlsx");
        }

        // Export to PDF
        public async Task<IActionResult> ExportToPdf()
        {
            var bookCount = await _context.Books.CountAsync();
            var studentCount = await _context.Students.CountAsync();
            var issueCount = await _context.Issues.CountAsync();
            var notIssuedCount = await _context.Issues.CountAsync(i => i.Status == "Issued");
            var unpaidPenaltiesCount = await _context.Penalties.CountAsync(p => p.PaymentStatus == "Unpaid");

            var data = new[]
            {
                new { Category = "Books", Count = bookCount },
                new { Category = "Students", Count = studentCount },
                new { Category = "Issues", Count = issueCount },
                new { Category = "Not Issued", Count = notIssuedCount },
                new { Category = "Unpaid Penalties", Count = unpaidPenaltiesCount }
            };

            // Use jsPDF to generate PDF
            var pdfScript = $@"
                <script src='https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.5.1/jspdf.umd.min.js'></script>
                <script>
                const {{ jsPDF }} = window.jspdf;
                const doc = new jsPDF();
                doc.text('Library System Report', 10, 10);

                let yPos = 20;
                {string.Join("\n", data.Select((d, i) => $"doc.text('{d.Category}: {d.Count}', 10, yPos + {i * 10});"))}

                doc.save('LibraryReport.pdf');
                </script>";

            return Content(pdfScript, "text/html");
        }
    }
}