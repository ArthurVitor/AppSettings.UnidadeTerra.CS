namespace AppSettings.Options;

public class ConcordanciaNotificacaoOptions
{
    public const string Key = "ConcordanciaNotificacao";

    public bool AceiteReceberNotificacoes { get; set; }
    public bool AceiteCompartilharContatos { get; set; }
    public int AnoBaseLegislacao { get; set; }
}