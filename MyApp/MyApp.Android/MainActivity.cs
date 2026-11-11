using Android.App;
using Android.Content.PM;
using Avalonia.Android;

namespace MyApp.Android;

[Activity(
    Label = "Побег от химии",
    Theme = "@style/Theme.AppCompat.Light.NoActionBar",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity
{
}