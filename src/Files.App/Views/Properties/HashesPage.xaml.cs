﻿using CommunityToolkit.WinUI;
using Files.App.Filesystem;
using Files.App.ViewModels.Properties;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Threading.Tasks;

namespace Files.App.Views.Properties
{
	public sealed partial class HashesPage : BasePropertiesPage
	{
		public HashesPage()
		{
			InitializeComponent();
		}

		private HashesViewModel HashesViewModel { get; set; }

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			var np = (MainPropertiesPage.PropertyNavParam)e.Parameter;
			if (np.navParameter is ListedItem listedItem)
				HashesViewModel = new(listedItem);

			base.OnNavigatedTo(e);
		}

		private void CopyHashButton_Click(object sender, RoutedEventArgs e)
		{
			var item = (Backend.Models.HashInfoItem)(((Button)sender).DataContext);

			var dp = new Windows.ApplicationModel.DataTransfer.DataPackage();
			dp.SetText(item.HashValue);
			Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dp);
		}

		private async void BasePropertiesPage_Loaded(object sender, RoutedEventArgs e)
		{
			await App.Window.DispatcherQueue.EnqueueAsync(async () =>
			{
				await HashesViewModel.ExecuteLoadAndCalcHashesCommandAsync(HashesViewModel.CancellationTokenSource.Token);
			});
		}

		protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
			=> Dispose();

		public async override Task<bool> SaveChangesAsync()
		{
			return true;
		}

		public override void Dispose()
		{
			HashesViewModel.Dispose();
		}
	}
}
