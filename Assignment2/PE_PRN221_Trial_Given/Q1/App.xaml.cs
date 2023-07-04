using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Q1.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Q1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; set; }
        public IConfiguration Configuration { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceConllection = new ServiceCollection();
            serviceConllection.AddTransient<MainWindow>();
            serviceConllection.AddScoped<PRN221_TrialContext>();
            ServiceProvider = serviceConllection.BuildServiceProvider();
            ServiceProvider.GetRequiredService<MainWindow>().Show();
        }
    }
}
