#region Usings
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.Web.WebView2.Core;
using System;
using System.Reflection;
using System.Threading.Tasks;
using UEVRDeluxe.Pages;
#endregion

namespace UEVRDeluxe;

/// <summary>Main application Window</summary>
public sealed partial class MainWindow : Window {
	public static CoreWebView2Environment WebViewEnv;

	public static nint hWnd { get; private set; }

	public MainWindow() {
		InitializeComponent();
		_ = InitializeBrowser();

		var version = Assembly.GetExecutingAssembly().GetName().Version;
		tbCaption.Text = $"Unreal VR Deluxe {version} BETA";

		hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
		var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
		var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);

		if (appWindow != null) {
			appWindow.TitleBar.ExtendsContentIntoTitleBar = true;
			appWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
			appWindow.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
		}

		mainFrame.Navigate(typeof(MainPage));
	}

	/// <summary>Trick to make it work if installed in Program Files folder, where user has no access rights</summary>
	async Task InitializeBrowser() {
		try {
			string userDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\UEVRDeluxe\\BrowserCache";
			WebViewEnv = await CoreWebView2Environment.CreateWithOptionsAsync(null, userDataFolder, new CoreWebView2EnvironmentOptions());
		} catch (Exception ex) {
			System.Diagnostics.Debug.WriteLine(ex.ToString());
		}
	}
}
