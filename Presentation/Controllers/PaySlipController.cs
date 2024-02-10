using Business.Abstract;
using DataAccess.Concrete.Context;
using Entities.Concrete;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Presentation.Models;
using SkiaSharp;
using System.Data;
using System.Security.Claims;
using System.Text;

namespace Presentation.Controllers
{
    public class PaySlipController : Controller
    {
        private readonly IPaySlipService _paySlipService;
        private readonly UserManager<AppUser> _userManager;
        private readonly HRMSContext _hrmsContext;

        public PaySlipController(IPaySlipService paySlipService, UserManager<AppUser> userManager, HRMSContext hrmsContext)
        {
            _paySlipService = paySlipService;
            _userManager = userManager;
            _hrmsContext = hrmsContext;
        }

        public IActionResult Index()
        {
            var paySlips = _hrmsContext.PaySlips.Include(p => p.AppUser).ToList();
            var appUserNames = paySlips.Select(p => p.AppUser.FullName).Distinct().ToList();
            var appUserSalary = paySlips.Select(p => p.AppUser.Salary).Distinct().ToList();
            var selectList = new SelectList(appUserNames);
            var selectListSalary = new SelectList(appUserSalary);
            ViewBag.AppUserNames = selectList;

            return View(paySlips);
        }

        [HttpGet]
        public IActionResult Index(string selectedAppUser, DateTime? startDate, DateTime? endDate)
        {
            var paySlips = _hrmsContext.PaySlips.Include(p => p.AppUser).ToList();

            if (!string.IsNullOrEmpty(selectedAppUser) && selectedAppUser != "Personeller")
            {
                paySlips = paySlips.Where(p => p.AppUser.FullName == selectedAppUser).ToList();
            }
            if (startDate.HasValue && endDate.HasValue)
            {
                paySlips = paySlips.Where(p => p.CreatedTime >= startDate && p.CreatedTime <= endDate).ToList();
            }

            var appUserNames = _hrmsContext.Users.Select(u => u.FullName).Distinct().ToList();
            var selectList = new SelectList(appUserNames);

            // Toplam Ücret Hesaplama
            decimal? usersTotalSalary = _hrmsContext.Users.Sum(u => u.Salary);
            decimal payslipTotalAwards = paySlips.Sum(p => p.Awards);

            ViewBag.AppUserNames = selectList;
            return View("Index", paySlips);
        }
        public ActionResult AddPaySlip()
        {
            var appUsers = _userManager.Users.ToList();
            ViewBag.AppUsers = new SelectList(appUsers, "Id", "FullName");

            return View();
        }

        [HttpPost]
        public ActionResult AddPaySlip(PaySlip model)
        {
            if (ModelState.IsValid)
            {
                _hrmsContext.PaySlips.Add(model);
                _hrmsContext.SaveChanges();

                return RedirectToAction("Index");
            }

            var appUsers = _userManager.Users.ToList();
            ViewBag.AppUsers = new SelectList(appUsers, "Id", "FullName");

            return View(model);

        }
        public IActionResult DeletePaySlip(int id)
        {
            var value = _paySlipService.GetById(id);
            _paySlipService.Delete(value);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult UpdatePaySlip(int id)
        {
            var paySlip = _hrmsContext.PaySlips.Find(id);

            if (paySlip == null)
            {
                return NotFound();
            }

            var appUsers = _userManager.Users.ToList();
            ViewBag.AppUsers = new SelectList(appUsers, "Id", "FullName");

            return View(paySlip);
        }
        [HttpPost]
        public ActionResult UpdatePaySlip(int id, PaySlip model)
        {
            if (ModelState.IsValid)
            {
                var paySlip = _hrmsContext.PaySlips.Find(id);

                if (paySlip == null)
                {
                    return NotFound();
                }

                paySlip.AppUserId = model.AppUserId;
                paySlip.Awards = model.Awards;

                _hrmsContext.SaveChanges();

                return RedirectToAction("Index");
            }

            var appUsers = _userManager.Users.ToList();
            ViewBag.AppUsers = new SelectList(appUsers, "Id", "FullName");

            return View(model);
        }

        public ActionResult DownloadAllPaySlips()
        {
            var paySlips = _hrmsContext.PaySlips.Include(p => p.AppUser).ToList();
            string logoImagePath = "C:\\Users\\sdoqa\\OneDrive\\Masaüstü\\Repos\\Miltek\\HRMS\\HRMS\\7533464.png";

            iTextSharp.text.Image logoImage = iTextSharp.text.Image.GetInstance(logoImagePath);
            logoImage.Alignment = iTextSharp.text.Image.ALIGN_LEFT; // Logoyu solo hizala
            logoImage.ScaleAbsolute(80, 100); // Logo boyutunu ayarla

            // PDF belgesini oluştur ve içeriğini ekle
            var document = new Document();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

                document.Open();

                // Logo görüntüsünü ekle
                document.Add(logoImage);


                BaseFont baseFont = BaseFont.CreateFont("c:\\windows\\fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font titleFont = new Font(baseFont, 12, Font.BOLD, BaseColor.Black);
                Font cellFont = new Font(baseFont, 11, Font.NORMAL, BaseColor.Black);


                // Şirket Bilgileri
                string companyInfo = "Şirket Adı: HRMS Friends Şirketi                                                    Vergi Dairesi: Ankara Vergi Dairesi\n" +
                                     "Adres: \tAnkamoll Mah. KazımKarabekir Cad. No:12                                     Vergi No: 1234567890\r\nMerkez / ANKARA\n" +
                                     "Telefon: 123-456-7890\n";
                document.Add(new Paragraph(companyInfo, cellFont));

                // Çizgi Ayırma


                document.Add(new Paragraph(" "));
                document.Add(new Paragraph("Bordro Raporu", titleFont));
                document.Add(new Paragraph(" "));

                PdfPTable table = new PdfPTable(5);
                table.WidthPercentage = 100;
                table.DefaultCell.Border = Rectangle.NO_BORDER; // Kenarlık olmadan hücreleri ayarla

                float[] columnWidths = { 3f, 2f, 2f, 2f, 3f };
                table.SetWidths(columnWidths);

                string[] headers = { "Personel Adı", "Prim Ödeme", "Maaş", "Toplam Ücret", "Tarih" };
                foreach (var header in headers)
                {
                    PdfPCell headerCell = new PdfPCell(new Phrase(header, cellFont));
                    headerCell.BackgroundColor = BaseColor.LightGray;
                    headerCell.HorizontalAlignment = Element.ALIGN_LEFT; // Başlıkları sola dayalı hale getir
                    headerCell.Border = Rectangle.NO_BORDER; // Kenarlık olmadan hücreleri ayarla
                    table.AddCell(headerCell);
                }

                bool isGrayRow = false;
                foreach (var paySlip in paySlips)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(paySlip.AppUser.FullName, cellFont));
                    cell.FixedHeight = 20f; // Hücre yüksekliği 20 birim olarak ayarla
                                            // Gri arka plan rengini ayarla
                    if (isGrayRow)
                    {
                        cell.BackgroundColor = BaseColor.LightGray;
                    }
                    cell.Border = Rectangle.NO_BORDER; // Kenarlık olmadan hücreleri ayarla
                    table.AddCell(cell);

                    PdfPCell awardsCell = new PdfPCell(new Phrase(paySlip.Awards.ToString(), cellFont));
                    awardsCell.BackgroundColor = isGrayRow ? BaseColor.LightGray : BaseColor.White;
                    awardsCell.Border = Rectangle.NO_BORDER; // Kenarlık olmadan hücreleri ayarla
                    cell.FixedHeight = 20f;
                    table.AddCell(awardsCell);

                    PdfPCell salaryCell = new PdfPCell(new Phrase(paySlip.AppUser.Salary.ToString(), cellFont));
                    salaryCell.BackgroundColor = isGrayRow ? BaseColor.LightGray : BaseColor.White;
                    salaryCell.Border = Rectangle.NO_BORDER; // Kenarlık olmadan hücreleri ayarla
                    cell.FixedHeight = 20f;
                    table.AddCell(salaryCell);

                    PdfPCell totalCell = new PdfPCell(new Phrase((paySlip.Awards + paySlip.AppUser.Salary).ToString(), cellFont));
                    totalCell.BackgroundColor = isGrayRow ? BaseColor.LightGray : BaseColor.White;
                    totalCell.Border = Rectangle.NO_BORDER; // Kenarlık olmadan hücreleri ayarla
                    cell.FixedHeight = 20f;
                    table.AddCell(totalCell);

                    PdfPCell dateCell1 = new PdfPCell(new Phrase(paySlip.CreatedTime.ToShortDateString(), cellFont));
                    dateCell1.BackgroundColor = isGrayRow ? BaseColor.LightGray : BaseColor.White;
                    dateCell1.Border = Rectangle.NO_BORDER; // Kenarlık olmadan hücreleri ayarla
                    cell.FixedHeight = 20f;
                    table.AddCell(dateCell1);

                    isGrayRow = !isGrayRow;
                }

                document.Add(table);

                string companySignature = "              HRMS Friends Şirketi\n " +"Ad Soyad\n " +"İMZA";
                Chunk signatureChunk = new Chunk(companySignature, cellFont);

                PdfPTable signatureTable = new PdfPTable(1);
                signatureTable.TotalWidth = 100f; // Tablonun genişliğini ayarla
                PdfPCell signatureCell = new PdfPCell(new Phrase(signatureChunk));
                signatureCell.HorizontalAlignment = Element.ALIGN_RIGHT; // Hücreyi sağa hizala
                signatureCell.Border = Rectangle.NO_BORDER; // Kenarlık olmadan hücreyi ayarla
                signatureTable.AddCell(signatureCell);

                // Sayfanın sağ en alt köşesine eklemek için boşluk ekleyin
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                document.Add(signatureTable);
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));

                string creationDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");

                // Oluşturma tarihini eklemek için tablo
                PdfPTable dateTable = new PdfPTable(1);
                dateTable.TotalWidth = 100f;
                PdfPCell dateCell = new PdfPCell(new Phrase("PDF Oluşturma Tarihi: " + creationDate, cellFont));
                dateCell.HorizontalAlignment = Element.ALIGN_LEFT;
                dateCell.Border = Rectangle.NO_BORDER;
                dateTable.AddCell(dateCell);

                document.Add(dateTable);

                document.Close();

                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();

                return File(bytes, "application/pdf", "PaySlipData.pdf");
            }
        }

        public ActionResult DownloadDataAsPdfID(int id)
        {
            var paySlip = _hrmsContext.PaySlips.Include(p => p.AppUser).FirstOrDefault(p => p.Id == id);

            if (paySlip == null)
            {
                return NotFound();
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                var document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

                document.Open();

                BaseFont baseFont = BaseFont.CreateFont("c:\\windows\\fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font titleFont = new Font(baseFont, 14, Font.BOLD, BaseColor.Black);
                Font cellFont = new Font(baseFont, 11, Font.NORMAL, BaseColor.Black);

                // Şirket Bilgileri
                string companyInfo = "Şirket Adı: HRMS Company                                                      Vergi Dairesi: Ankara Vergi Dairesi\n" +
                                     "Adres: \tAnkamoll Mah. KazımKarabekir Cad. No:12                                     Vergi No: 1234567890\r\nMerkez / ANKARA" +
                                     "Telefon: 123-456-7890\n";
                document.Add(new Paragraph(companyInfo, cellFont));
                // Çizgi Ayırma
                PdfContentByte contentByte = writer.DirectContent;
                contentByte.SetLineWidth(1f);
                contentByte.MoveTo(30, document.Top - 55);
                contentByte.LineTo(document.PageSize.Width - 30, document.Top - 55);
                contentByte.Stroke();

                // Bordro Bilgileri
                document.Add(new Paragraph("Bordro Raporu", titleFont));
                document.Add(new Paragraph(" "));

                PdfPTable table = new PdfPTable(2);
                table.WidthPercentage = 100;
                table.DefaultCell.Border = Rectangle.NO_BORDER; // Kenarlık olmadan hücreleri ayarla

                string[] labels = { "Personel Adı:", "Prim Ödeme:", "Maaş:", "Toplam Ücret:", "Tarih:" };
                string[] values = { paySlip.AppUser.FullName, paySlip.Awards.ToString(), paySlip.AppUser.Salary.ToString(),
                            (paySlip.Awards + paySlip.AppUser.Salary).ToString(), paySlip.CreatedTime.ToShortDateString() };

                for (int i = 0; i < labels.Length; i++)
                {
                    PdfPCell labelCell = new PdfPCell(new Phrase(labels[i], cellFont));
                    labelCell.BackgroundColor = i % 2 == 0 ? BaseColor.LightGray : BaseColor.White;
                    labelCell.Border = Rectangle.NO_BORDER; // Kenarlık olmadan hücreleri ayarla
                    labelCell.FixedHeight = 20f;
                    table.AddCell(labelCell);

                    PdfPCell valueCell = new PdfPCell(new Phrase(values[i], cellFont));
                    valueCell.BackgroundColor = i % 2 == 0 ? BaseColor.LightGray : BaseColor.White;
                    valueCell.Border = Rectangle.NO_BORDER; // Kenarlık olmadan hücreleri ayarla
                    valueCell.FixedHeight = 20f;
                    table.AddCell(valueCell);
                }


                document.Add(table);

                document.Close();

                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();

                return File(bytes, "application/pdf", $"PaySlip_{paySlip.Id}.pdf");
            }
        }

        public IActionResult MyPaySlips()
        {
            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var user = _hrmsContext.Users
                .Include(u => u.PaySlips)
                .FirstOrDefault(u => u.Id == loggedInUserId);
            if (user == null)
            {
                return RedirectToAction("AccessDenied"); // Örnek bir yönlendirme
            }
            var paySlips = user.PaySlips;
            var appUserNames = paySlips.Select(p => p.AppUser.FullName).Distinct().ToList();
            var selectList = new SelectList(appUserNames);

            ViewBag.AppUserNames = selectList;
            return View(paySlips);
        }
    }
}