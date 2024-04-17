# Desafio - API de Preferências de Notificação

A API de Preferências de Notificação é um serviço de consulta que outros serviços acessam, através de um endpoint `GET`, para verificar quais tipos de comunicação estão permitidas para o envio de notificações aos destinatários. Estas preferências são armazenadas em arquivos *appSettings*, que são atualizados de tempos em tempos. Até alguns dias atrás, toda vez que o *appSettings* era atualizado, o serviço precisava ser reiniciado, então o estagiário resolveu ser proativo e implementar o mecanismo de uso de *OptionsSnapshot* para possibilitar uso de settings atualizadas em tempo real sem o "trade-off" de indisponibilidade temporária.

O problema é que agora a aplicação sequer inicializa, e ele está desesperado sem saber o que fazer. Ele veio até você e está pedindo sua ajuda para descobrir uma solução que seja rápida, elegante, e que tenha impacto quase zero na implementação atual. Utilize os *code snippets* disponibilizados abaixo para construir a API, e efetue as modificações necessárias para atingir o comportamento de atualização em tempo real sem indisponibilidade. Existem restrições aqui do que você **NÃO** poderá fazer:
1. Mudar a injeção de dependência do *AppSettingsMap*: Ele precisa permanecer *Singleton*, por questão de boas práticas e performance.
2. Modificar o esqueleto geral das dependências: A dinâmica de dependências `Endpoint GET ➡ AppSettingsMap ➡ NotificacaoOptions` não pode ser alterada.
3. E reforçando, NÃO pode haver mais a necessidade de reinício da aplicação para que configurações atualizadas no *appSettings* passem a ser refletidas no mapa de configurações.

## Snippets

> Nota: Você pode criar um novo projeto Web API "Minimal", deixar ele o mais limpo possível, e colocar todos os arquivos na própria raíz. Não há necessidade de arquiteturação do projeto (se quiser, pode fazê-lo, mas não é obrigatório).

### Options

#### `NotificacaoOptions.cs`
```cs
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
```

#### `ConcordanciaNotificacaoOptions.cs`
```cs
public class ConcordanciaNotificacaoOptions
{
  public const string Key = "ConcordanciaNotificacao";

  public bool AceiteReceberNotificacoes { get; set; }
  public bool AceiteCompartilharContatos { get; set; }
  public int AnoBaseLegislacao { get; set; }
}
```

### Main

#### `appSettings.json`
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Notificacao": {
    "ConcordanciaNotificacao": {
      "AceiteReceberNotificacoes": true,
      "AceiteCompartilharContatos": true,
      "AnoBaseLegislacao": 2024
    },
    "ReceberSMS": false,
    "ReceberLigacao": false,
    "ReceberWhatsApp": false,
    "Telefone": "+5555955555555",
    "ReceberEmail": true,
    "Email": "oemaildapessoa@email.com"
  }
}
```

#### `Program.cs`
```cs
class Program
{
  public static void Main(string[] args)
  {
    var appBuilder = WebApplication.CreateBuilder(args);

    appBuilder.Services.AddEndpointsApiExplorer();
    appBuilder.Services.AddSwaggerGen();

    var notificacaoOptions = appBuilder.Configuration.GetSection(NotificacaoOptions.Key);
    appBuilder.Services.Configure<NotificacaoOptions>(notificacaoOptions);

    appBuilder.Services.AddSingleton<ISettingsMap, AppSettingsMap>();

    var app = appBuilder.Build();

    app.UseHttpsRedirection();
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseDeveloperExceptionPage();

    app.MapGet(
      "/configuracoes-notificacao",
      (ISettingsMap settingsMap) =>
      {
        return Results.Ok(settingsMap);
      }
    );

    app.Run();
  }
}
```
> Dica: A API já está configurada no `Program.cs` para disponibilizar as requisições através do *Swagger* no navegador. Confira em: https://localhost:[porta]/swagger.

### Serviços

#### `ISettingsMap.cs`
```cs
public interface ISettingsMap
{
  NotificacaoOptions NotificacaoOptions { get; set; }
}
```

#### `AppSettingsMap.cs`
```cs
public class AppSettingsMap : ISettingsMap
{
  public AppSettingsMap(IOptionsSnapshot<NotificacaoOptions> notificacaoOptions)
  {
    NotificacaoOptions = notificacaoOptions.Value;
  }

  public NotificacaoOptions NotificacaoOptions { get; set; }
}
```

# ❗ Lembre-se: Este desafio tem a ver com a API e o padrão *Options*. Compreenda o fluxo e analise qual a relação do problema com o uso do *OptionsSnapshot*, e qual alternativa pode ser aplicada para correção.
