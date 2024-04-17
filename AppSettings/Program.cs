using AppSettings.Interfaces;
using AppSettings.Options;
using AppSettings.Services;

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