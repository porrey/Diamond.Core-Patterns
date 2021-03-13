using System.Windows;
using Diamond.Core.Wpf;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example.Wpf
{
	public partial class MainWindow : Window, IMainWindow
	{
		public MainWindow(ILogger<MainWindow> logger, IConfiguration configuration, IMainViewModel viewModel)
		{
			InitializeComponent();
			this.DataContext = viewModel;
		}
	}
}
