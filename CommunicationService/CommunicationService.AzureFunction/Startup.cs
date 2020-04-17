﻿using AutoMapper;
using CommunicationService.Core.Configuration;
using CommunicationService.Core.Interfaces.Repositories;
using CommunicationService.Core.Interfaces.Services;
using CommunicationService.EmailService;
using CommunicationService.Handlers;
using CommunicationService.Mappers;
using CommunicationService.Repo;
using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

[assembly: FunctionsStartup(typeof(CommunicationService.AzureFunction.Startup))]
namespace CommunicationService.AzureFunction
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // We need to get the app directory this way.  Using Environment.CurrentDirectory doesn't work in Azure
            ExecutionContextOptions executioncontextoptions = builder.Services.BuildServiceProvider()
                .GetService<IOptions<ExecutionContextOptions>>().Value;
            string currentDirectory = executioncontextoptions.AppDirectory;

            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .SetBasePath(currentDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfigurationRoot config = configBuilder.Build();

            IConfigurationSection sendGridConfigSettings = config.GetSection("SendGridConfig");
            builder.Services.Configure<SendGridConfig>(sendGridConfigSettings);

            builder.Services.AddMediatR(typeof(SendEmailHandler).Assembly);
            builder.Services.AddAutoMapper(typeof(AddressDetailsProfile).Assembly);
            builder.Services.AddSingleton<ISendEmailService, SendEmailService>();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                   options.UseInMemoryDatabase(databaseName: "CommunicationService.AzureFunction"));
            builder.Services.AddTransient<IRepository, Repository>();
        }
    }
}