namespace app.api.Middleware
{
    public static class CorsMiddleware
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowVueApp", policy =>
                {
                    policy.WithOrigins("http://localhost:3000", "http://localhost:8080", "http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });
        }
    }
} 