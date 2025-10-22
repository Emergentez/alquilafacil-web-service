using AlquilaFacilPlatform.Booking.Application.Internal.CommandServices;
using AlquilaFacilPlatform.Booking.Application.Internal.QueryServices;
using AlquilaFacilPlatform.Booking.Domain.Repositories;
using AlquilaFacilPlatform.Booking.Domain.Services;
using AlquilaFacilPlatform.Booking.Infrastructure.Persistence.EFC.Repositories;
using AlquilaFacilPlatform.IAM.Application.Internal.CommandServices;
using AlquilaFacilPlatform.IAM.Application.Internal.OutboundServices;
using AlquilaFacilPlatform.IAM.Application.Internal.QueryServices;
using AlquilaFacilPlatform.IAM.Domain.Model.Commands;
using AlquilaFacilPlatform.IAM.Domain.Repositories;
using AlquilaFacilPlatform.IAM.Domain.Services;
using AlquilaFacilPlatform.IAM.Infrastructure.Hashing.BCrypt.Services;
using AlquilaFacilPlatform.IAM.Infrastructure.Persistence.EFC.Respositories;
using AlquilaFacilPlatform.IAM.Infrastructure.Pipeline.Middleware.Extensions;
using AlquilaFacilPlatform.IAM.Infrastructure.Tokens.JWT.Configuration;
using AlquilaFacilPlatform.IAM.Infrastructure.Tokens.JWT.Services;
using AlquilaFacilPlatform.IAM.Interfaces.ACL;
using AlquilaFacilPlatform.IAM.Interfaces.ACL.Service;
using AlquilaFacilPlatform.Locals.Application.Internal.CommandServices;
using AlquilaFacilPlatform.Locals.Application.Internal.QueryServices;
using AlquilaFacilPlatform.Locals.Domain.Model.Commands;
using AlquilaFacilPlatform.Locals.Domain.Repositories;
using AlquilaFacilPlatform.Locals.Domain.Services;
using AlquilaFacilPlatform.Locals.Infrastructure.Persistence.EFC.Repositories;
using AlquilaFacilPlatform.Locals.Interfaces.ACL;
using AlquilaFacilPlatform.Locals.Interfaces.ACL.Services;
using AlquilaFacilPlatform.Management.Application.Internal.CommandServices;
using AlquilaFacilPlatform.Management.Application.Internal.QueryServices;
using AlquilaFacilPlatform.Management.Domain.Model.Commands;
using AlquilaFacilPlatform.Management.Domain.Repositories;
using AlquilaFacilPlatform.Management.Domain.Services;
using AlquilaFacilPlatform.Management.Infrastructure.Persistence.EFC.Repositories;
using AlquilaFacilPlatform.Management.Interfaces.REST.Hubs;
using AlquilaFacilPlatform.Notifications.Application.Internal.CommandServices;
using AlquilaFacilPlatform.Notifications.Application.Internal.QueryServices;
using AlquilaFacilPlatform.Notifications.Domain.Repositories;
using AlquilaFacilPlatform.Notifications.Domain.Services;
using AlquilaFacilPlatform.Notifications.Infrastructure.Persistence.EFC.Repositories;
using AlquilaFacilPlatform.Notifications.Interfaces.ACL;
using AlquilaFacilPlatform.Notifications.Interfaces.ACL.Services;
using AlquilaFacilPlatform.Profiles.Application.Internal.CommandServices;
using AlquilaFacilPlatform.Profiles.Application.Internal.QueryServices;
using AlquilaFacilPlatform.Profiles.Domain.Repositories;
using AlquilaFacilPlatform.Profiles.Domain.Services;
using AlquilaFacilPlatform.Profiles.Infrastructure.Persistence.EFC.Repositories;
using AlquilaFacilPlatform.Profiles.Interfaces.ACL;
using AlquilaFacilPlatform.Profiles.Interfaces.ACL.Services;
using AlquilaFacilPlatform.Shared.Application.Internal.OutboundServices;
using AlquilaFacilPlatform.Shared.Application.Internal.OutboundServices.ExternalServices;
using AlquilaFacilPlatform.Shared.Domain.Repositories;
using AlquilaFacilPlatform.Shared.Infrastructure.Interfaces.ASP.Configuration;
using AlquilaFacilPlatform.Shared.Infrastructure.Persistence.EFC.Configuration;
using AlquilaFacilPlatform.Shared.Infrastructure.Persistence.EFC.Repositories;
using AlquilaFacilPlatform.Shared.Infrastructure.Endpoints;
using AlquilaFacilPlatform.Subscriptions.Application.Internal.CommandServices;
using AlquilaFacilPlatform.Subscriptions.Application.Internal.QueryServices;
using AlquilaFacilPlatform.Subscriptions.Domain.Model.Commands;
using AlquilaFacilPlatform.Subscriptions.Domain.Repositories;
using AlquilaFacilPlatform.Subscriptions.Domain.Services;
using AlquilaFacilPlatform.Subscriptions.Infrastructure.Persistence.EFC.Repositories;
using AlquilaFacilPlatform.Subscriptions.Interfaces.ACL;
using AlquilaFacilPlatform.Subscriptions.Interfaces.ACL.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers( options => options.Conventions.Add(new KebabCaseRouteNamingConvention()));

// Add Database Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var developmentString = builder.Configuration.GetConnectionString("DevelopmentConnection");
var allowedOrigins = builder.Configuration.GetSection("AllowedFrontEndOrigins").Get<string[]>();


// Configure Database Context and Logging Levels

builder.Services.AddDbContext<AppDbContext>(
    options =>
    {
        if (builder.Environment.IsDevelopment())
        {
            options.UseMySql(developmentString, ServerVersion.AutoDetect(developmentString))
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        }
        else if (builder.Environment.IsProduction())
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .LogTo(Console.WriteLine, LogLevel.Error)
                .EnableDetailedErrors();
        }
    });
// Configure Lowercase URLs
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1",
            new OpenApiInfo
            {
                Title = "AlquilaFacil.API",
                Version = "v1",
                Description = "Alquila Facil API",
                TermsOfService = new Uri("https://alquila-facil.com/tos"),
                Contact = new OpenApiContact
                {
                    Name = "Alquila Facil",
                    Email = "contact@alquilaf.com"
                },
                License = new OpenApiLicense
                {
                    Name = "Apache 2.0",
                    Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html")
                }
            });
        c.EnableAnnotations();
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                },
                Array.Empty<string>()
            }
        });
    });

builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendPolicy", policy =>
        policy.WithOrigins(allowedOrigins!)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

// Add SignalR for real-time communication
builder.Services.AddSignalR();

// Shared Bounded Context Injection Configuration
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IUserExternalService, UserExternalService>();
builder.Services.AddScoped<ILocalExternalService, LocalExternalService>();
builder.Services.AddScoped<IProfilesExternalService, ProfilesExternalService>();
builder.Services.AddScoped<INotificationExternalService, NotificationExternalService>();
builder.Services.AddScoped<ISubscriptionExternalService, SubscriptionExternalService>();

// IAM Bounded Context Injection Configuration

builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<ISeedUserRoleCommandService, SeedUserRoleCommandService>();
builder.Services.AddScoped<ISeedAdminCommandService, SeedAdminCommandService>();
builder.Services.AddScoped<ISeedTechnicianCommandService, SeedTechnicianCommandService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<IIamContextFacade, IamContextFacade>();
builder.Services.AddJwtAuthentication(builder.Configuration);

// Locals Bounded Context Injection Configuration
builder.Services.AddScoped<ILocalCommandService, LocalCommandService>();
builder.Services.AddScoped<ILocalQueryService, LocalQueryService>();
builder.Services.AddScoped<ILocalsContextFacade, LocalsContextFacade>();
builder.Services.AddScoped<ILocalRepository, LocalRepository>();

builder.Services.AddScoped<ILocalCategoryRepository, LocalCategoryRepository>();
builder.Services.AddScoped<ILocalCategoryCommandService, LocalCategoryCommandService>();
builder.Services.AddScoped<ILocalCategoryQueryService, LocalCategoryQueryService>();

builder.Services.AddScoped<ICommentCommandService, CommentCommandService>();
builder.Services.AddScoped<ICommentQueryService, CommentQueryService>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

builder.Services.AddScoped<IReportCommandService, ReportCommandService>();
builder.Services.AddScoped<IReportQueryService, ReportQueryService>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();

// Booking Bounded Context Injection Configuration
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IReservationCommandService, ReservationCommandService>();
builder.Services.AddScoped<IReservationQueryService, ReservationQueryService>();

// Profiles Bounded Context Injection Configuration
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IProfileCommandService, ProfileCommandService>();
builder.Services.AddScoped<IProfileQueryService, ProfileQueryService>();
builder.Services.AddScoped<IProfilesContextFacade, ProfilesContextFacade>();

// Notifications Bounded Context Injection Configuration

builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationCommandService, NotificationCommandService>();
builder.Services.AddScoped<INotificationQueryService, NotificationQueryService>();
builder.Services.AddScoped<INotificationsContextFacade, NotificationsContextFacade>();

// Subscriptions Bounded Context Injection Configuration

builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<ISubscriptionCommandService, SubscriptionCommandService>();
builder.Services.AddScoped<ISubscriptionQueryServices, SubscriptionQueryService>();
builder.Services.AddScoped<ISubscriptionContextFacade, SubscriptionContextFacade>();

builder.Services.AddScoped<ISubscriptionStatusRepository, SubscriptionStatusRepository>();
builder.Services.AddScoped<ISubscriptionStatusCommandService, SubscriptionStatusCommandService>();

builder.Services.AddScoped<IPlanRepository, PlanRepository>();
builder.Services.AddScoped<IPlanCommandService, PlanCommandService>();
builder.Services.AddScoped<IPlanQueryService, PlanQueryService>();
builder.Services.AddScoped<ISeedSubscriptionPlanCommandService, SeedSubscriptionPlanCommandService>();

builder.Services.AddScoped<IInvoiceQueryService, InvoiceQueryService>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IInvoiceCommandService, InvoiceCommandService>();

// Management Bounded Context Injection Configuration

builder.Services.AddScoped<IReadingCommandService, ReadingCommandService>();
builder.Services.AddScoped<IReadingQueryService, ReadingQueryService>();
builder.Services.AddScoped<IReadingRepository, ReadingRepository>();

builder.Services.AddScoped<ISensorTypeRepository, SensorTypeRepository>();
builder.Services.AddScoped<ISeedSensorTypeCommandService, SeedSensorTypeCommandService>();

builder.Services.AddScoped<ILocalEdgeNodeCommandService, LocalEdgeNodeCommandService>();
builder.Services.AddScoped<ILocalEdgeNodeQueryService, LocalEdgeNodeQueryService>();
builder.Services.AddScoped<ILocalEdgeNodeRepository, LocalEdgeNodeRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
    
    var planCommandService = services.GetRequiredService<ISeedSubscriptionPlanCommandService>();
    await planCommandService.Handle(new SeedSubscriptionPlanCommand());
    
    var subscriptionStatusCommandService = services.GetRequiredService<ISubscriptionStatusCommandService>();
    await subscriptionStatusCommandService.Handle(new SeedSubscriptionStatusCommand());
    
    var userRoleCommandService = services.GetRequiredService<ISeedUserRoleCommandService>();
    await userRoleCommandService.Handle(new SeedUserRolesCommand());
    
    var adminCommandService = services.GetRequiredService<ISeedAdminCommandService>();
    await adminCommandService.Handle(new SeedAdminCommand());
    
    var technicianCommandService = services.GetRequiredService<ISeedTechnicianCommandService>();
    await technicianCommandService.Handle(new SeedTechnicianCommand());
    
    var localCategoryTypeCommandService = services.GetRequiredService<ILocalCategoryCommandService>();
    await localCategoryTypeCommandService.Handle(new SeedLocalCategoriesCommand());
    
    var sensorTypeCommandService = services.GetRequiredService<ISeedSensorTypeCommandService>();
    await sensorTypeCommandService.Handle(new SeedSensorTypesCommand());
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontendPolicy");


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<ReadingHub>(SignalRRoutes.ReadingHub);

app.Run();
