namespace AppSettings.Options;

public class NotificacaoOptions
{
    public const string Key = "Notificacao";

    public ConcordanciaNotificacaoOptions ConcordanciaNotificacao { get; set; }
    public bool ReceberSms { get; set; }
    public bool ReceberLigacao { get; set; }
    public bool ReceberWhatsApp { get; set; }
    public string Telefone { get; set; }
    public bool ReceberEmail { get; set; }
    public string Email { get; set; }
}