using Android.App;
using Android.Runtime;
using Avalonia;
using Avalonia.Android;

namespace MyApp.Android;

[Application]
public class MainApplication : AvaloniaAndroidApplication<App>
{
    protected MainApplication(nint javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
    {
    }
}