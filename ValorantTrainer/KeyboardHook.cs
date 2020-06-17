using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace ValorantTrainer
{
  public class KeyboardHook : IDisposable
  {
    private IntPtr hookId = IntPtr.Zero;
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 256;
    private KeyboardHook.LowLevelKeyboardProc keyboardProc;
    private const uint SWP_NOSIZE = 1;
    private const uint SWP_NOMOVE = 2;
    private const uint SWP_SHOWWINDOW = 64;

    public KeyboardHook()
    {
      this.keyboardProc = new KeyboardHook.LowLevelKeyboardProc(this.HookCallback);
      this.hookId = this.SetHook(this.keyboardProc);
    }

    public void Dispose()
    {
      KeyboardHook.UnhookWindowsHookEx(this.hookId);
    }

    public event KeyboardHook.someKeyPressed KeyCombinationPressed;

    private IntPtr SetHook(KeyboardHook.LowLevelKeyboardProc proc)
    {
      using (Process currentProcess = Process.GetCurrentProcess())
      {
        using (ProcessModule mainModule = currentProcess.MainModule)
          return KeyboardHook.SetWindowsHookEx(13, proc, KeyboardHook.GetModuleHandle(mainModule.ModuleName), 0U);
      }
    }

    private IntPtr HookCallback(int nCode, IntPtr wParam, ref KeyboardHookStruct lParam)
    {
      if (nCode >= 0 && wParam == (IntPtr) 256)
      {
        Key keyPressed = KeyInterop.KeyFromVirtualKey(lParam.vkCode);
        KeyboardHook.someKeyPressed combinationPressed = this.KeyCombinationPressed;
        if (combinationPressed != null)
          combinationPressed(keyPressed);
      }
      return KeyboardHook.CallNextHookEx(this.hookId, nCode, wParam, ref lParam);
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(
      int idHook,
      KeyboardHook.LowLevelKeyboardProc lpfn,
      IntPtr hMod,
      uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(
      IntPtr hhk,
      int nCode,
      IntPtr wParam,
      ref KeyboardHookStruct lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    public delegate void someKeyPressed(Key keyPressed);

    private delegate IntPtr LowLevelKeyboardProc(
      int nCode,
      IntPtr wParam,
      ref KeyboardHookStruct lParam);
  }
}
