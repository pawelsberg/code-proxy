# code-proxy
Code-proxy console allows to interact with Allegro library (SubC.AllegroDotNet) using C# and standard input/output.
You can use pipes to redirect input/output to/from the code-proxy to your application which does not support Allegro library.
## Example of a manual usage
1. Run CodeProxy.Console.exe
2. Type in the console the following c# code chunks (with the Enter at the end):
```csharp
Al.Init();
Al.InitFontAddon();
Al.InitTtfAddon();
Al.SetNewDisplayFlags(DisplayFlags.FullscreenWindow);
Al.CreateDisplay(3840, 2400); // this resolution is ignored in fullscreen mode
Console.WriteLine("DISPLAY_WIDTH:" + Al.GetDisplayWidth(Al.GetCurrentDisplay()));
Console.WriteLine("DISPLAY_HEIGHT:" + Al.GetDisplayHeight(Al.GetCurrentDisplay()));
Al.ClearToColor(Al.MapRgb(20, 0, 0));
AllegroFont? font = Al.LoadTtfFont(Environment.GetFolderPath(Environment.SpecialFolder.Fonts) + @"\consola.ttf", 50, LoadFontFlags.None);
Al.DrawUstr(font, Al.MapRgb(200, 0, 0), 0, 0, FontAlignFlags.Left, Al.UstrNew("Hello, World!"));
Al.DestroyFont(font);
Al.FlipDisplay();
Al.Rest(5);
Al.DestroyDisplay(Al.GetCurrentDisplay());
Al.UninstallSystem();
R.Run();
R.Exit();
R.Run();

````
You should be able to see the Allegro window with the text "Hello, World!".
Input is divided to runnable code chunks using a special token "R.Run();"
## Versioning
The versioning follows the following patterns:
- v(major).(minor) - for a new release

or

- v(major).(minor).(patch) - for a bug fix, refactoring or specification update only. This pattern is never used to release a new feature.

Major of 0 means that the library is in the development stage not ready for production.

Branches for these are:

```
release/(major)/(minor)/main 
```

or 

```
release/(major)/(minor)/(patch)/main
```	
