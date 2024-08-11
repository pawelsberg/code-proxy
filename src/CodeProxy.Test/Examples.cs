using CodeProxy.Routines;
using SubC.AllegroDotNet;
using SubC.AllegroDotNet.Enums;
using SubC.AllegroDotNet.Models;

namespace CodeProxy.Test;

public static class Examples
{
    public static void GameLoop()
    {
        Al.Init();
        Al.InitFontAddon();
        Al.InitTtfAddon();
        Al.InitPrimitivesAddon();
        Al.InitImageAddon();
        Al.InstallKeyboard();
        Al.InstallMouse();
        var path = Al.GetStandardPath(StandardPath.ResourcesPath);
        var dataPath = Al.PathCstr(path, '\\');
        Al.AppendPathComponent(path, "data");
        Al.ChangeDirectory(Al.PathCstr(path, '\\'));
        Al.DestroyPath(path);
        Al.SetNewDisplayFlags(DisplayFlags.FullscreenWindow);
        Al.CreateDisplay(1024, 768);
        Console.WriteLine("DISPLAY_WIDTH:" + Al.GetDisplayWidth(Al.GetCurrentDisplay()));
        Console.WriteLine("DISPLAY_HEIGHT:" + Al.GetDisplayHeight(Al.GetCurrentDisplay()));
        AllegroTimer? timer = Al.CreateTimer(1.0 / 60.0);
        AllegroEventQueue? eventQueue = Al.CreateEventQueue();
        Al.RegisterEventSource(eventQueue, Al.GetDisplayEventSource(Al.GetCurrentDisplay()));
        Al.RegisterEventSource(eventQueue, Al.GetKeyboardEventSource());
        Al.RegisterEventSource(eventQueue, Al.GetMouseEventSource());
        Al.RegisterEventSource(eventQueue, Al.GetTimerEventSource(timer));
        Al.StartTimer(timer);
        while (!R.ExitRequested)
        {
            AllegroEvent allegroEvent = new AllegroEvent();
            Al.WaitForEvent(eventQueue, ref allegroEvent);
            if (allegroEvent.Type is EventType.KeyDown)
            {
                Console.WriteLine($"KEY_DOWN:{allegroEvent.Keyboard.KeyCode}");
            }
            if (allegroEvent.Type is EventType.KeyUp)
            {
                Console.WriteLine($"KEY_UP:{allegroEvent.Keyboard.KeyCode}");
            }
            if (allegroEvent.Type is EventType.DisplayClose)
            {
                R.Exit();
            }
            if (allegroEvent.Type is EventType.MouseAxes)
            {
                Console.WriteLine($"MOUSE_AXES:{allegroEvent.Mouse.X},{allegroEvent.Mouse.Y}");
            }
            if (allegroEvent.Type is EventType.MouseButtonDown)
            {
                Console.WriteLine($"MOUSE_BUTTON_DOWN:{allegroEvent.Mouse.Button}");
            }
            if (allegroEvent.Type is EventType.MouseButtonUp)
            {
                Console.WriteLine($"MOUSE_BUTTON_UP:{allegroEvent.Mouse.Button}");
            }
            if (allegroEvent.Type is EventType.Timer)
            {
                string? code = null;
                lock (R.CodeStringBuilder)
                {
                    string codeBuffered = R.CodeStringBuilder.ToString();
                    string[] codes = codeBuffered.Split(R.CodeSplitter);
                    if (codes.Length > 1)
                    {
                        code = codes[0];
                        R.CodeStringBuilder.Clear();
                        R.CodeStringBuilder.Append(String.Join(R.CodeSplitter, codes.Skip(1)));
                    }
                }
                if (code != null)
                {
                    R.RunCode(code);
                    Al.FlipDisplay();
                }
            }
        }
        Al.DestroyDisplay(Al.GetCurrentDisplay());
        Al.UninstallSystem();
        Console.WriteLine("DONE.");
    }

    public static void SimpleExample1()
    {
        Al.Init();
        Al.InitFontAddon();
        Al.InitTtfAddon();
        Al.InitPrimitivesAddon();
        Al.InitImageAddon();
        Al.InstallAudio();
        var path = Al.GetStandardPath(StandardPath.ResourcesPath); var dataPath = Al.PathCstr(path, '\\'); Al.AppendPathComponent(path, "data"); Al.ChangeDirectory(Al.PathCstr(path, '\\')); Al.DestroyPath(path);

        Al.SetNewDisplayFlags(DisplayFlags.FullscreenWindow);
        Al.CreateDisplay(3840, 2400);
        Al.ClearToColor(Al.MapRgb(200, 200, 0));
        Al.FlipDisplay();

        Al.ClearToColor(Al.MapRgb(0, 0, 200));

        AllegroFont? font = Al.LoadTtfFont(Environment.GetFolderPath(Environment.SpecialFolder.Fonts) + @"\consola.ttf", 200, LoadFontFlags.None);
        Al.DrawUstr(font, Al.MapRgb(50, 0, 0), 1920, 1200, FontAlignFlags.Center, Al.UstrNew("Hello, World!"));
        Al.DestroyFont(font);
        Al.DrawFilledCircle(1920, 1200, 100, Al.MapRgb(0, 0, 0));
        Al.DrawCircle(1920, 1200, 100, Al.MapRgb(220, 0, 0), 10);
        Al.DrawRectangle(1920, 1200, 100, 100, Al.MapRgb(0, 0, 0), 10f);
        Al.DrawLine(1, 1, 200, 200, Al.MapRgb(5, 0, 0), 10f);
        Al.DrawTriangle(100, 100, 100, 200, 300, 300, Al.MapRgb(5, 0, 0), 20f);
        var bitmap = Al.LoadBitmap("view.jpg");
        Al.DrawBitmapRegion(bitmap,1700,800,500,500, 1500, 1500, FlipFlags.None);
        Al.DestroyBitmap(bitmap);
        Al.FlipDisplay();

        Al.Rest(30);
        Al.DestroyDisplay(Al.GetCurrentDisplay());
        Al.UninstallSystem();
    }

    public static void SimpleExample2()
    {
        Al.Init();
        Al.InitFontAddon();
        Al.InitTtfAddon();
        Al.InitPrimitivesAddon();
        var path = Al.GetStandardPath(StandardPath.ResourcesPath); var dataPath = Al.PathCstr(path, '\\'); Al.AppendPathComponent(path, "data"); Al.ChangeDirectory(Al.PathCstr(path, '\\')); Al.DestroyPath(path);

        Al.SetNewDisplayFlags(DisplayFlags.FullscreenWindow);
        Al.CreateDisplay(3840, 2400);
        Al.ClearToColor(Al.MapRgb(200, 200, 0));
        Al.FlipDisplay();
        Console.WriteLine("DISPLAY_WIDTH:" + Al.GetDisplayWidth(Al.GetCurrentDisplay()));
        Console.WriteLine("DISPLAY_HEIGHT:" + Al.GetDisplayHeight(Al.GetCurrentDisplay()));
        
        Al.ClearToColor(Al.MapRgb(0, 0, 200));
        AllegroFont? font = Al.LoadTtfFont(Environment.GetFolderPath(Environment.SpecialFolder.Fonts) + @"\consola.ttf", 200, LoadFontFlags.None);
        Al.DrawUstr(font, Al.MapRgb(50, 0, 0), 0, 0, FontAlignFlags.Left, Al.UstrNew("Hello, World!"));
        Console.WriteLine("USTR_WIDTH:" + Al.GetUstrWidth(font, Al.UstrNew("Hello, World!")));
        Al.DestroyFont(font);
        Al.DrawRectangle(0, 0, 1430, 200, Al.MapRgb(0, 0, 0), 10f);
        Al.DrawFilledCircle(1920, 1200, 100, Al.MapRgb(0, 0, 0));
        Al.DrawCircle(1920, 1200, 100, Al.MapRgb(220, 0, 0), 10);
        Al.DrawRectangle(1920, 1200, 100, 100, Al.MapRgb(0, 0, 0), 10f);
        Al.DrawLine(1, 1, 200, 200, Al.MapRgb(5, 0, 0), 10f);
        Al.FlipDisplay();

        Al.Rest(30);
        Al.DestroyDisplay(Al.GetCurrentDisplay());
        Al.UninstallSystem();
    }
}