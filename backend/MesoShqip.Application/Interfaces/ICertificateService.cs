namespace MesoShqip.Application.Interfaces;

public interface ICertificateService
{
    byte[] GenerateCertificate(string username, string level, DateTime issuedAt);
}