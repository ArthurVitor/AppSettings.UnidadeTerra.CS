using AppSettings.Options;

namespace AppSettings.Interfaces;

public interface ISettingsMap
{
    NotificacaoOptions NotificacaoOptions { get; set; }
}