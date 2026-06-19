using MesoShqip.Application.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Document = QuestPDF.Fluent.Document; // <-- ALIAS PËR TË ZGJIDHUR AMBIGUITETIN

namespace MesoShqip.Infrastructure.Services;

public class CertificateService : ICertificateService
{
    public CertificateService()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public byte[] GenerateCertificate(string username, string level, DateTime issuedAt)
    {
        return Document.Create(container => // <-- TANI PUNON PA PROBLEM
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(40);

                page.Content().Column(col =>
                {
                    col.Item().AlignCenter()
                       .Text("🦅 Mëso Shqip")
                       .FontSize(36).Bold().FontColor("#E8294A");

                    col.Item().PaddingTop(10).AlignCenter()
                       .Text("Çertifikatë Krenarie")
                       .FontSize(20).Italic();

                    col.Item().PaddingTop(30).AlignCenter()
                       .Text(username)
                       .FontSize(28).Bold();

                    col.Item().PaddingTop(10).AlignCenter()
                       .Text($"ka përfunduar me sukses nivelin {level}")
                       .FontSize(16);

                    col.Item().PaddingTop(20).AlignCenter()
                       .Text($"Lëshuar më {issuedAt:dd MMMM yyyy}")
                       .FontSize(12).FontColor("#555555");
                });
            });
        }).GeneratePdf();
    }
}