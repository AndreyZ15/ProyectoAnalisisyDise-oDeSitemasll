using Microsoft.Extensions.Logging;
using Proyecto.Services;
using Microsoft.Extensions.Logging;
using Proyecto.Services;

namespace Proyecto
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Configurar FirebaseService con la URL de tu Firebase Realtime Database
            string firebaseApiKey = "https://proyectoanalisisydiseno2-default-rtdb.firebaseio.com/";
            builder.Services.AddSingleton(new FireBaseService(firebaseApiKey));

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}