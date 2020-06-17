using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;

namespace ValorantTrainer
{
  public class MainMenu : Form
  {
    private string folderpath = Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK");
    private string ownscriptpath = Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\ownscript.ahk");
    private string bhoppath = Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\bhop.ahk");
    private string triggerpath = Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk");
    private string aaspath = Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\aas.ahk");
    private Process bhop;
    private Process triggerbot;
    private Process ownscript;
    private Process aas;
    private KeyboardHook kHook;
    private int pause;
    private const int WM_NCHITTEST = 132;
    private const int HT_CLIENT = 1;
    private const int HT_CAPTION = 2;
    private IContainer components;
    private Button btnBhopStart;
    private Button btnBhopStop;
    private Button btnBhopPause;
    private GroupBox groupBoxBhop;
    private Panel panelBhop;
    private GroupBox groupBoxTriggerbot;
    private Panel panelTriggerbot;
    private Button btnTriggerbotStop;
    private Button btnTriggerbotStart;
    private Label label1;
    private Button btnClose;
    private CheckBox checkBoxTopMost;
    private GroupBox groupBoxMisc;
    private Button btnPanic;
    private Button btnInfo;
    private Button btnMinimize;
    private GroupBox groupBoxOwn;
    private Panel panelOwn;
    private Button btnOwnStop;
    private Button btnOwnStart;
    private Button btnCreateOpen;
    private TrackBar trackBarOpacity;
    private Label labelOpacityText;
    private Label labelOpacityValue;
    private Label label2;
    private Label label3;
    private GroupBox groupBox1;
    private Button btnAASStart;
    private ComboBox comboBoxAgents;
    private Button btnAASStop;
    private Label label4;
    private ComboBox comboBoxTriggerKey;

    public MainMenu()
    {
      this.InitializeComponent();
      this.kHook = new KeyboardHook();
      this.kHook.KeyCombinationPressed += new KeyboardHook.someKeyPressed(this.KHook_KeyCombinationPressed);
      this.btnBhopStop.Enabled = false;
      this.btnTriggerbotStop.Enabled = false;
      this.btnBhopPause.Enabled = false;
      if (File.Exists(this.ownscriptpath))
        this.btnCreateOpen.Text = "Open";
      try
      {
        if (Directory.Exists(this.folderpath))
          return;
        Directory.CreateDirectory(this.folderpath);
        int num = (int) MessageBox.Show("Script folder created at: " + this.folderpath, "Folder created!");
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Couldn't create folder: " + ex.ToString(), "ERROR!");
      }
      try
      {
        using (StreamWriter text = File.CreateText(this.bhoppath))
        {
          text.WriteLine("F1::");
          text.WriteLine("Suspend");
          text.WriteLine("Return");
          text.WriteLine("*space::");
          text.WriteLine("Loop");
          text.WriteLine("{");
          text.WriteLine("GetKeyState,state,space,P");
          text.WriteLine("If state = U");
          text.WriteLine("Break");
          text.WriteLine("Send,{space}");
          text.WriteLine("Sleep,20");
          text.WriteLine("}");
        }
        using (StreamWriter text = File.CreateText(this.triggerpath))
        {
          text.WriteLine("; --- Change enemy outlines to purple");
          text.WriteLine("Loop");
          text.WriteLine("{");
          text.WriteLine("KeyWait, LCtrl, D");
          text.WriteLine("CoordMode, Pixel, Screen");
          text.WriteLine("PixelSearch, FoundX, FoundY, 957, 519, 961, 583, 0xA145A3, 30, Fast RGB");
          text.WriteLine("If (ErrorLevel = 0){");
          text.WriteLine("sleep, 30");
          text.WriteLine("send {Lbutton down}");
          text.WriteLine("sleep, 10");
          text.WriteLine("send {lbutton up}");
          text.WriteLine("}");
          text.WriteLine("");
          text.WriteLine("}");
          text.WriteLine("return");
        }
        using (StreamWriter text = File.CreateText(this.aaspath))
        {
          text.WriteLine("#NoEnv");
          text.WriteLine("SendMode Input");
          text.WriteLine("#SingleInstance Force");
          text.WriteLine("#MaxThreadsPerHotkey 2");
          text.WriteLine("");
          text.WriteLine("F11::");
          text.WriteLine("toggle := !toggle");
          text.WriteLine("Loop");
          text.WriteLine("{");
          text.WriteLine("if !toggle");
          text.WriteLine("break");
          text.WriteLine("");
          text.WriteLine("Sleep, 51");
          text.WriteLine("MouseClick, left, XAgent, 968");
          text.WriteLine("Sleep, 51");
          text.WriteLine("MouseClick, left, 958, 850");
          text.WriteLine("}");
          text.WriteLine("return");
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error creating scripts: " + ex.ToString(), "ERROR!");
      }
    }

    private static void lineChanger(string newText, string testfile, int line_to_edit)
    {
      string[] contents = File.ReadAllLines(testfile);
      contents[line_to_edit - 1] = newText;
      File.WriteAllLines(testfile, contents);
    }

    protected override void WndProc(ref Message m)
    {
      base.WndProc(ref m);
      if (m.Msg != 132)
        return;
      m.Result = (IntPtr) 2;
    }

    private void btnBhopStart_Click(object sender, EventArgs e)
    {
      this.bhop = Process.Start(Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\bhop.ahk"));
      this.btnBhopStart.Enabled = false;
      this.btnBhopStop.Enabled = true;
      this.btnBhopPause.Enabled = true;
      this.panelBhop.BackColor = Color.Lime;
    }

    private void btnBhopStop_Click(object sender, EventArgs e)
    {
      this.bhop.Kill();
      this.btnBhopStop.Enabled = false;
      this.btnBhopStart.Enabled = true;
      this.btnBhopPause.Enabled = false;
      this.panelBhop.BackColor = Color.Red;
    }

    private void btnBhopPause_Click(object sender, EventArgs e)
    {
      if (this.pause == 0)
      {
        SendKeys.Send("{F1}");
        this.panelBhop.BackColor = Color.RoyalBlue;
        ++this.pause;
        this.btnBhopPause.Text = "Continue (F1)";
      }
      else
      {
        SendKeys.Send("{F1}");
        this.panelBhop.BackColor = Color.Lime;
        --this.pause;
        this.btnBhopPause.Text = "Pause (F1)";
      }
    }

    private void btnStartTriggerbot_Click(object sender, EventArgs e)
    {
      if (this.comboBoxTriggerKey.Text == "Left Control")
      {
        MainMenu.lineChanger("KeyWait, LCtrl, D", Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"), 4);
        this.triggerbot = Process.Start(Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"));
      }
      else if (this.comboBoxTriggerKey.Text == "Right Mouse Button")
      {
        MainMenu.lineChanger("KeyWait, RButton, D", Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"), 4);
        this.triggerbot = Process.Start(Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"));
      }
      else if (this.comboBoxTriggerKey.Text == "Left Alt")
      {
        MainMenu.lineChanger("KeyWait, LAlt, D", Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"), 4);
        this.triggerbot = Process.Start(Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"));
      }
      else if (this.comboBoxTriggerKey.Text == "Left Shift")
      {
        MainMenu.lineChanger("KeyWait, LShift, D", Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"), 4);
        this.triggerbot = Process.Start(Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"));
      }
      else if (this.comboBoxTriggerKey.Text == "Mouse Button 4")
      {
        MainMenu.lineChanger("KeyWait, XButton1, D", Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"), 4);
        this.triggerbot = Process.Start(Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"));
      }
      else if (this.comboBoxTriggerKey.Text == "Mouse Button 5")
      {
        MainMenu.lineChanger("KeyWait, XButton2, D", Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"), 4);
        this.triggerbot = Process.Start(Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"));
      }
      else if (this.comboBoxTriggerKey.Text == "X")
      {
        MainMenu.lineChanger("KeyWait, X, D", Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"), 4);
        this.triggerbot = Process.Start(Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"));
      }
      else if (this.comboBoxTriggerKey.Text == "C")
      {
        MainMenu.lineChanger("KeyWait, C, D", Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"), 4);
        this.triggerbot = Process.Start(Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"));
      }
      this.btnTriggerbotStart.Enabled = false;
      this.btnTriggerbotStop.Enabled = true;
      this.panelTriggerbot.BackColor = Color.Lime;
    }

    private void btnStopTriggerbot_Click(object sender, EventArgs e)
    {
      this.triggerbot.Kill();
      this.btnTriggerbotStop.Enabled = false;
      this.btnTriggerbotStart.Enabled = true;
      this.panelTriggerbot.BackColor = Color.Red;
    }

    private void checkBoxTopMost_CheckedChanged(object sender, EventArgs e)
    {
      if (this.checkBoxTopMost.Checked)
      {
        this.TopMost = true;
      }
      else
      {
        if (this.checkBoxTopMost.Checked)
          return;
        this.TopMost = false;
      }
    }

    private void trackBarOpacity_Scroll(object sender, EventArgs e)
    {
      Form.ActiveForm.Opacity = (double) this.trackBarOpacity.Value / 100.0;
      this.labelOpacityValue.Text = this.trackBarOpacity.Value.ToString();
    }

    private void btnPanic_Click(object sender, EventArgs e)
    {
      this.bhop.Kill();
      this.triggerbot.Kill();
      this.ownscript.Kill();
      this.panelBhop.BackColor = Color.Red;
      this.panelTriggerbot.BackColor = Color.Red;
    }

    private void btnInfo_Click(object sender, EventArgs e)
    {
      int num = (int) MessageBox.Show("Coded by HvRibbecK\ngithub.com/HvRibbecK\nDiscord: 8i#1274", "Contact:");
    }

    private void btnCreateOpen_Click(object sender, EventArgs e)
    {
      if (!File.Exists(this.ownscriptpath))
      {
        try
        {
          using (StreamWriter text = File.CreateText(this.ownscriptpath))
          {
            text.WriteLine("; --- Paste your own script in here!");
            text.WriteLine("; --- It can't have a GUI or any linked files like config.ini!");
          }
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Error creating your own script: " + ex.ToString(), "ERROR!");
        }
      }
      this.btnCreateOpen.Text = "Open";
      Process.Start("notepad.exe", this.ownscriptpath);
    }

    private void btnOwnStart_Click(object sender, EventArgs e)
    {
      this.ownscript = Process.Start(Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\ownscript.ahk"));
      this.btnOwnStart.Enabled = false;
      this.btnOwnStop.Enabled = true;
      this.panelOwn.BackColor = Color.Lime;
    }

    private void btnOwnStop_Click(object sender, EventArgs e)
    {
      this.ownscript.Kill();
      this.btnOwnStart.Enabled = true;
      this.btnOwnStop.Enabled = false;
      this.panelOwn.BackColor = Color.Red;
    }

    private void btnAASStart_Click(object sender, EventArgs e)
    {
      if (this.comboBoxAgents.Text == "Choose Agent..")
      {
        int num = (int) MessageBox.Show("Select an Agent first!", "ERROR!");
      }
      else if (this.comboBoxAgents.Text == "Breach")
      {
        MainMenu.lineChanger("MouseClick, left, 1150, 968", Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\aas.ahk"), 14);
        this.aas = Process.Start(Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\aas.ahk"));
      }
      else if (this.comboBoxAgents.Text == "Cypher")
      {
        MainMenu.lineChanger("MouseClick, left, 1240, 968", Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\aas.ahk"), 14);
        this.aas = Process.Start(Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\aas.ahk"));
      }
      else if (this.comboBoxAgents.Text == "Jett")
      {
        MainMenu.lineChanger("MouseClick, left, 587, 968", Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\aas.ahk"), 14);
        this.aas = Process.Start(Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\aas.ahk"));
      }
      else if (this.comboBoxAgents.Text == "Phoenix")
      {
        MainMenu.lineChanger("MouseClick, left, 680, 968", Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\aas.ahk"), 14);
        this.aas = Process.Start(Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\aas.ahk"));
      }
      else if (this.comboBoxAgents.Text == "Sage")
      {
        MainMenu.lineChanger("MouseClick, left, 870, 968", Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\aas.ahk"), 14);
        this.aas = Process.Start(Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\aas.ahk"));
      }
      else if (this.comboBoxAgents.Text == "Sova")
      {
        MainMenu.lineChanger("MouseClick, left, 960, 968", Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\aas.ahk"), 14);
        this.aas = Process.Start(Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\aas.ahk"));
      }
      else if (this.comboBoxAgents.Text == "Viper")
      {
        MainMenu.lineChanger("MouseClick, left, 1057, 968", Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\aas.ahk"), 14);
        this.aas = Process.Start(Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\aas.ahk"));
      }
      else if (this.comboBoxAgents.Text == "Brimstone")
      {
        MainMenu.lineChanger("MouseClick, left, 493, 968", Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\aas.ahk"), 14);
        this.aas = Process.Start(Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\aas.ahk"));
      }
      else if (this.comboBoxAgents.Text == "Omen")
      {
        MainMenu.lineChanger("MouseClick, left, 1337, 968", Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\aas.ahk"), 14);
        this.aas = Process.Start(Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\aas.ahk"));
      }
      else if (this.comboBoxAgents.Text == "Raze")
      {
        MainMenu.lineChanger("MouseClick, left, 1435, 968", Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\aas.ahk"), 14);
        this.aas = Process.Start(Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\aas.ahk"));
      }
      else
      {
        if (!(this.comboBoxAgents.Text == "Reyna"))
          return;
        MainMenu.lineChanger("MouseClick, left, 775, 968", Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\aas.ahk"), 14);
        this.aas = Process.Start(Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\aas.ahk"));
      }
    }

    private void btnAASStop_Click(object sender, EventArgs e)
    {
      this.aas.Kill();
    }

    private void KHook_KeyCombinationPressed(Key keyPressed)
    {
      if (keyPressed == Key.F7 && this.btnBhopStart.Enabled)
      {
        this.bhop = Process.Start(Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\bhop.ahk"));
        this.btnBhopStart.Enabled = false;
        this.btnBhopStop.Enabled = true;
        this.btnBhopPause.Enabled = true;
        this.panelBhop.BackColor = Color.Lime;
      }
      if (keyPressed == Key.F8 && this.btnBhopStop.Enabled)
      {
        this.bhop.Kill();
        this.btnBhopStop.Enabled = false;
        this.btnBhopStart.Enabled = true;
        this.btnBhopPause.Enabled = false;
        this.panelBhop.BackColor = Color.Red;
      }
      if (keyPressed == Key.F1 && !this.btnBhopStart.Enabled)
      {
        if (this.pause == 0)
        {
          this.panelBhop.BackColor = Color.RoyalBlue;
          ++this.pause;
          this.btnBhopPause.Text = "Continue (F1)";
        }
        else
        {
          this.panelBhop.BackColor = Color.Lime;
          --this.pause;
          this.btnBhopPause.Text = "Pause (F1)";
        }
      }
      if (keyPressed == Key.F5 && this.btnTriggerbotStart.Enabled)
      {
        if (this.comboBoxTriggerKey.Text == "Left Control")
        {
          MainMenu.lineChanger("KeyWait, LCtrl, D", Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"), 4);
          this.triggerbot = Process.Start(Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"));
        }
        else if (this.comboBoxTriggerKey.Text == "Right Mouse Button")
        {
          MainMenu.lineChanger("KeyWait, RButton, D", Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"), 4);
          this.triggerbot = Process.Start(Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"));
        }
        else if (this.comboBoxTriggerKey.Text == "Left Alt")
        {
          MainMenu.lineChanger("KeyWait, LAlt, D", Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"), 4);
          this.triggerbot = Process.Start(Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"));
        }
        else if (this.comboBoxTriggerKey.Text == "Left Shift")
        {
          MainMenu.lineChanger("KeyWait, LShift, D", Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"), 4);
          this.triggerbot = Process.Start(Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"));
        }
        else if (this.comboBoxTriggerKey.Text == "Mouse Button 4")
        {
          MainMenu.lineChanger("KeyWait, XButton1, D", Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"), 4);
          this.triggerbot = Process.Start(Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"));
        }
        else if (this.comboBoxTriggerKey.Text == "Mouse Button 5")
        {
          MainMenu.lineChanger("KeyWait, XButton2, D", Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"), 4);
          this.triggerbot = Process.Start(Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"));
        }
        else if (this.comboBoxTriggerKey.Text == "X")
        {
          MainMenu.lineChanger("KeyWait, X, D", Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"), 4);
          this.triggerbot = Process.Start(Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"));
        }
        else if (this.comboBoxTriggerKey.Text == "C")
        {
          MainMenu.lineChanger("KeyWait, C, D", Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"), 4);
          this.triggerbot = Process.Start(Environment.ExpandEnvironmentVariables("%AppData%\\ValorantTrainer by HvRibbecK\\trigger.ahk"));
        }
        this.btnTriggerbotStart.Enabled = false;
        this.btnTriggerbotStop.Enabled = true;
        this.panelTriggerbot.BackColor = Color.Lime;
      }
      if (keyPressed != Key.F6 || !this.btnTriggerbotStop.Enabled)
        return;
      this.triggerbot.Kill();
      this.btnTriggerbotStop.Enabled = false;
      this.btnTriggerbotStart.Enabled = true;
      this.panelTriggerbot.BackColor = Color.Red;
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void btnMinimize_Click(object sender, EventArgs e)
    {
      this.WindowState = FormWindowState.Minimized;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MainMenu));
      this.btnBhopStart = new Button();
      this.btnBhopStop = new Button();
      this.btnBhopPause = new Button();
      this.groupBoxBhop = new GroupBox();
      this.panelBhop = new Panel();
      this.groupBoxTriggerbot = new GroupBox();
      this.comboBoxTriggerKey = new ComboBox();
      this.btnTriggerbotStop = new Button();
      this.btnTriggerbotStart = new Button();
      this.panelTriggerbot = new Panel();
      this.label1 = new Label();
      this.btnClose = new Button();
      this.checkBoxTopMost = new CheckBox();
      this.groupBoxMisc = new GroupBox();
      this.labelOpacityValue = new Label();
      this.labelOpacityText = new Label();
      this.btnInfo = new Button();
      this.trackBarOpacity = new TrackBar();
      this.btnPanic = new Button();
      this.btnMinimize = new Button();
      this.groupBoxOwn = new GroupBox();
      this.panelOwn = new Panel();
      this.btnOwnStop = new Button();
      this.btnOwnStart = new Button();
      this.btnCreateOpen = new Button();
      this.label2 = new Label();
      this.label3 = new Label();
      this.groupBox1 = new GroupBox();
      this.label4 = new Label();
      this.btnAASStop = new Button();
      this.btnAASStart = new Button();
      this.comboBoxAgents = new ComboBox();
      this.groupBoxBhop.SuspendLayout();
      this.groupBoxTriggerbot.SuspendLayout();
      this.groupBoxMisc.SuspendLayout();
      this.trackBarOpacity.BeginInit();
      this.groupBoxOwn.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      this.btnBhopStart.ForeColor = Color.Black;
      this.btnBhopStart.Location = new Point(6, 33);
      this.btnBhopStart.Name = "btnBhopStart";
      this.btnBhopStart.Size = new Size(139, 23);
      this.btnBhopStart.TabIndex = 0;
      this.btnBhopStart.Text = "Start (F7)";
      this.btnBhopStart.UseVisualStyleBackColor = true;
      this.btnBhopStart.Click += new EventHandler(this.btnBhopStart_Click);
      this.btnBhopStop.BackColor = Color.Transparent;
      this.btnBhopStop.ForeColor = Color.Black;
      this.btnBhopStop.Location = new Point(6, 91);
      this.btnBhopStop.Name = "btnBhopStop";
      this.btnBhopStop.Size = new Size(139, 23);
      this.btnBhopStop.TabIndex = 1;
      this.btnBhopStop.Text = "Stop (F8)";
      this.btnBhopStop.UseVisualStyleBackColor = false;
      this.btnBhopStop.Click += new EventHandler(this.btnBhopStop_Click);
      this.btnBhopPause.BackgroundImageLayout = ImageLayout.Center;
      this.btnBhopPause.ForeColor = Color.Black;
      this.btnBhopPause.Location = new Point(6, 62);
      this.btnBhopPause.Name = "btnBhopPause";
      this.btnBhopPause.Size = new Size(139, 23);
      this.btnBhopPause.TabIndex = 2;
      this.btnBhopPause.Text = "Pause (F1)";
      this.btnBhopPause.UseVisualStyleBackColor = true;
      this.btnBhopPause.Click += new EventHandler(this.btnBhopPause_Click);
      this.groupBoxBhop.BackColor = Color.Transparent;
      this.groupBoxBhop.BackgroundImageLayout = ImageLayout.Zoom;
      this.groupBoxBhop.Controls.Add((Control) this.panelBhop);
      this.groupBoxBhop.Controls.Add((Control) this.btnBhopStart);
      this.groupBoxBhop.Controls.Add((Control) this.btnBhopStop);
      this.groupBoxBhop.Controls.Add((Control) this.btnBhopPause);
      this.groupBoxBhop.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
      this.groupBoxBhop.ForeColor = Color.Aqua;
      this.groupBoxBhop.Location = new Point(12, 78);
      this.groupBoxBhop.Name = "groupBoxBhop";
      this.groupBoxBhop.Size = new Size(151, 185);
      this.groupBoxBhop.TabIndex = 3;
      this.groupBoxBhop.TabStop = false;
      this.groupBoxBhop.Text = "Bunny Hop";
      this.panelBhop.BackColor = Color.Red;
      this.panelBhop.Location = new Point(6, 134);
      this.panelBhop.Name = "panelBhop";
      this.panelBhop.Size = new Size(139, 34);
      this.panelBhop.TabIndex = 3;
      this.groupBoxTriggerbot.BackColor = Color.Transparent;
      this.groupBoxTriggerbot.Controls.Add((Control) this.comboBoxTriggerKey);
      this.groupBoxTriggerbot.Controls.Add((Control) this.btnTriggerbotStop);
      this.groupBoxTriggerbot.Controls.Add((Control) this.btnTriggerbotStart);
      this.groupBoxTriggerbot.Controls.Add((Control) this.panelTriggerbot);
      this.groupBoxTriggerbot.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.groupBoxTriggerbot.ForeColor = Color.Aqua;
      this.groupBoxTriggerbot.Location = new Point(183, 78);
      this.groupBoxTriggerbot.Name = "groupBoxTriggerbot";
      this.groupBoxTriggerbot.Size = new Size(151, 185);
      this.groupBoxTriggerbot.TabIndex = 4;
      this.groupBoxTriggerbot.TabStop = false;
      this.groupBoxTriggerbot.Text = "Triggerbot";
      this.comboBoxTriggerKey.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.comboBoxTriggerKey.FormattingEnabled = true;
      this.comboBoxTriggerKey.Items.AddRange(new object[7]
      {
        (object) "Right Mouse Button",
        (object) "Left Alt",
        (object) "Left Shift",
        (object) "Mouse Button 4",
        (object) "Mouse Button 5",
        (object) "X",
        (object) "C"
      });
      this.comboBoxTriggerKey.Location = new Point(7, 92);
      this.comboBoxTriggerKey.Name = "comboBoxTriggerKey";
      this.comboBoxTriggerKey.Size = new Size(138, 21);
      this.comboBoxTriggerKey.TabIndex = 9;
      this.comboBoxTriggerKey.Text = "Left Control";
      this.btnTriggerbotStop.ForeColor = Color.Black;
      this.btnTriggerbotStop.Location = new Point(6, 62);
      this.btnTriggerbotStop.Name = "btnTriggerbotStop";
      this.btnTriggerbotStop.Size = new Size(139, 21);
      this.btnTriggerbotStop.TabIndex = 8;
      this.btnTriggerbotStop.Text = "Stop (F6)";
      this.btnTriggerbotStop.UseVisualStyleBackColor = true;
      this.btnTriggerbotStop.Click += new EventHandler(this.btnStopTriggerbot_Click);
      this.btnTriggerbotStart.ForeColor = Color.Black;
      this.btnTriggerbotStart.Location = new Point(6, 33);
      this.btnTriggerbotStart.Name = "btnTriggerbotStart";
      this.btnTriggerbotStart.Size = new Size(139, 23);
      this.btnTriggerbotStart.TabIndex = 6;
      this.btnTriggerbotStart.Text = "Start (F5)";
      this.btnTriggerbotStart.UseVisualStyleBackColor = true;
      this.btnTriggerbotStart.Click += new EventHandler(this.btnStartTriggerbot_Click);
      this.panelTriggerbot.BackColor = Color.Red;
      this.panelTriggerbot.Location = new Point(7, 134);
      this.panelTriggerbot.Name = "panelTriggerbot";
      this.panelTriggerbot.Size = new Size(139, 34);
      this.panelTriggerbot.TabIndex = 5;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new Font("Microsoft Sans Serif", 15f, FontStyle.Bold);
      this.label1.Location = new Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(243, 50);
      this.label1.TabIndex = 5;
      this.label1.Text = "Valorant Trainer V1.2.0 \r\nby HvRibbecK";
      this.btnClose.BackColor = Color.Firebrick;
      this.btnClose.FlatAppearance.BorderSize = 2;
      this.btnClose.FlatStyle = FlatStyle.Flat;
      this.btnClose.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnClose.ForeColor = Color.Red;
      this.btnClose.Location = new Point(422, 9);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(85, 30);
      this.btnClose.TabIndex = 6;
      this.btnClose.Text = "Exit";
      this.btnClose.UseVisualStyleBackColor = false;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.checkBoxTopMost.AutoSize = true;
      this.checkBoxTopMost.ForeColor = Color.White;
      this.checkBoxTopMost.Location = new Point(6, 29);
      this.checkBoxTopMost.Name = "checkBoxTopMost";
      this.checkBoxTopMost.Size = new Size(109, 17);
      this.checkBoxTopMost.TabIndex = 7;
      this.checkBoxTopMost.Text = "Make Topmost";
      this.checkBoxTopMost.UseVisualStyleBackColor = true;
      this.checkBoxTopMost.CheckedChanged += new EventHandler(this.checkBoxTopMost_CheckedChanged);
      this.groupBoxMisc.BackColor = Color.Transparent;
      this.groupBoxMisc.Controls.Add((Control) this.labelOpacityValue);
      this.groupBoxMisc.Controls.Add((Control) this.labelOpacityText);
      this.groupBoxMisc.Controls.Add((Control) this.btnInfo);
      this.groupBoxMisc.Controls.Add((Control) this.trackBarOpacity);
      this.groupBoxMisc.Controls.Add((Control) this.btnPanic);
      this.groupBoxMisc.Controls.Add((Control) this.checkBoxTopMost);
      this.groupBoxMisc.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.groupBoxMisc.ForeColor = Color.Aqua;
      this.groupBoxMisc.Location = new Point(353, 78);
      this.groupBoxMisc.Name = "groupBoxMisc";
      this.groupBoxMisc.Size = new Size(151, 185);
      this.groupBoxMisc.TabIndex = 8;
      this.groupBoxMisc.TabStop = false;
      this.groupBoxMisc.Text = "Misc";
      this.labelOpacityValue.AutoSize = true;
      this.labelOpacityValue.ForeColor = Color.Red;
      this.labelOpacityValue.Location = new Point(117, 84);
      this.labelOpacityValue.Name = "labelOpacityValue";
      this.labelOpacityValue.Size = new Size(28, 13);
      this.labelOpacityValue.TabIndex = 103;
      this.labelOpacityValue.Text = "100";
      this.labelOpacityText.AutoSize = true;
      this.labelOpacityText.ForeColor = Color.White;
      this.labelOpacityText.Location = new Point(6, 84);
      this.labelOpacityText.Name = "labelOpacityText";
      this.labelOpacityText.Size = new Size(117, 13);
      this.labelOpacityText.TabIndex = 102;
      this.labelOpacityText.Text = "Opacity Value in %:";
      this.btnInfo.ForeColor = Color.Black;
      this.btnInfo.Location = new Point(6, 145);
      this.btnInfo.Name = "btnInfo";
      this.btnInfo.Size = new Size(139, 23);
      this.btnInfo.TabIndex = 9;
      this.btnInfo.Text = "Contact";
      this.btnInfo.UseVisualStyleBackColor = true;
      this.btnInfo.Click += new EventHandler(this.btnInfo_Click);
      this.trackBarOpacity.BackColor = Color.FromArgb(64, 64, 64);
      this.trackBarOpacity.LargeChange = 1;
      this.trackBarOpacity.Location = new Point(6, 52);
      this.trackBarOpacity.Maximum = 100;
      this.trackBarOpacity.Minimum = 1;
      this.trackBarOpacity.Name = "trackBarOpacity";
      this.trackBarOpacity.Size = new Size(139, 45);
      this.trackBarOpacity.TabIndex = 101;
      this.trackBarOpacity.TickFrequency = 10;
      this.trackBarOpacity.Value = 100;
      this.trackBarOpacity.Scroll += new EventHandler(this.trackBarOpacity_Scroll);
      this.btnPanic.ForeColor = Color.Black;
      this.btnPanic.Location = new Point(6, 116);
      this.btnPanic.Name = "btnPanic";
      this.btnPanic.Size = new Size(139, 23);
      this.btnPanic.TabIndex = 8;
      this.btnPanic.Text = "Panic!!";
      this.btnPanic.UseVisualStyleBackColor = true;
      this.btnPanic.Click += new EventHandler(this.btnPanic_Click);
      this.btnMinimize.BackColor = Color.Black;
      this.btnMinimize.FlatAppearance.BorderColor = Color.RoyalBlue;
      this.btnMinimize.FlatStyle = FlatStyle.Flat;
      this.btnMinimize.ForeColor = Color.RoyalBlue;
      this.btnMinimize.Location = new Point(328, 9);
      this.btnMinimize.Name = "btnMinimize";
      this.btnMinimize.Size = new Size(85, 30);
      this.btnMinimize.TabIndex = 9;
      this.btnMinimize.Text = "Minimize";
      this.btnMinimize.UseVisualStyleBackColor = false;
      this.btnMinimize.Click += new EventHandler(this.btnMinimize_Click);
      this.groupBoxOwn.Controls.Add((Control) this.panelOwn);
      this.groupBoxOwn.Controls.Add((Control) this.btnOwnStop);
      this.groupBoxOwn.Controls.Add((Control) this.btnOwnStart);
      this.groupBoxOwn.Controls.Add((Control) this.btnCreateOpen);
      this.groupBoxOwn.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.groupBoxOwn.ForeColor = Color.Aqua;
      this.groupBoxOwn.Location = new Point(12, 289);
      this.groupBoxOwn.Name = "groupBoxOwn";
      this.groupBoxOwn.Size = new Size(151, 185);
      this.groupBoxOwn.TabIndex = 10;
      this.groupBoxOwn.TabStop = false;
      this.groupBoxOwn.Text = "Your own \"cheat\"";
      this.panelOwn.BackColor = Color.Red;
      this.panelOwn.Location = new Point(6, 145);
      this.panelOwn.Name = "panelOwn";
      this.panelOwn.Size = new Size(139, 34);
      this.panelOwn.TabIndex = 11;
      this.btnOwnStop.ForeColor = Color.Black;
      this.btnOwnStop.Location = new Point(7, 105);
      this.btnOwnStop.Name = "btnOwnStop";
      this.btnOwnStop.Size = new Size(139, 23);
      this.btnOwnStop.TabIndex = 2;
      this.btnOwnStop.Text = "Stop";
      this.btnOwnStop.UseVisualStyleBackColor = true;
      this.btnOwnStop.Click += new EventHandler(this.btnOwnStop_Click);
      this.btnOwnStart.ForeColor = Color.Black;
      this.btnOwnStart.Location = new Point(7, 70);
      this.btnOwnStart.Name = "btnOwnStart";
      this.btnOwnStart.Size = new Size(139, 23);
      this.btnOwnStart.TabIndex = 1;
      this.btnOwnStart.Text = "Start";
      this.btnOwnStart.UseVisualStyleBackColor = true;
      this.btnOwnStart.Click += new EventHandler(this.btnOwnStart_Click);
      this.btnCreateOpen.ForeColor = Color.Black;
      this.btnCreateOpen.Location = new Point(7, 34);
      this.btnCreateOpen.Name = "btnCreateOpen";
      this.btnCreateOpen.Size = new Size(139, 23);
      this.btnCreateOpen.TabIndex = 0;
      this.btnCreateOpen.Text = "Create";
      this.btnCreateOpen.UseVisualStyleBackColor = true;
      this.btnCreateOpen.Click += new EventHandler(this.btnCreateOpen_Click);
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold);
      this.label2.Location = new Point(343, 362);
      this.label2.Name = "label2";
      this.label2.Size = new Size(164, 45);
      this.label2.TabIndex = 11;
      this.label2.Text = "Enemy outlines must be \r\n              \r\nto make triggerbot work!";
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold);
      this.label3.ForeColor = Color.Purple;
      this.label3.Location = new Point(398, 377);
      this.label3.Name = "label3";
      this.label3.Size = new Size(49, 15);
      this.label3.TabIndex = 12;
      this.label3.Text = "Purple";
      this.groupBox1.BackColor = Color.Transparent;
      this.groupBox1.Controls.Add((Control) this.label4);
      this.groupBox1.Controls.Add((Control) this.btnAASStop);
      this.groupBox1.Controls.Add((Control) this.btnAASStart);
      this.groupBox1.Controls.Add((Control) this.comboBoxAgents);
      this.groupBox1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.groupBox1.ForeColor = Color.Aqua;
      this.groupBox1.Location = new Point(183, 289);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(151, 186);
      this.groupBox1.TabIndex = 13;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Auto Agent Selector";
      this.label4.AutoSize = true;
      this.label4.ForeColor = Color.White;
      this.label4.Location = new Point(33, 74);
      this.label4.Name = "label4";
      this.label4.Size = new Size(89, 26);
      this.label4.TabIndex = 3;
      this.label4.Text = "Start -> F11 ->\r\n  F11 -> Stop";
      this.btnAASStop.ForeColor = Color.Black;
      this.btnAASStop.Location = new Point(6, 155);
      this.btnAASStop.Name = "btnAASStop";
      this.btnAASStop.Size = new Size(139, 23);
      this.btnAASStop.TabIndex = 2;
      this.btnAASStop.Text = "Stop";
      this.btnAASStop.UseVisualStyleBackColor = true;
      this.btnAASStop.Click += new EventHandler(this.btnAASStop_Click);
      this.btnAASStart.ForeColor = Color.Black;
      this.btnAASStart.Location = new Point(6, 126);
      this.btnAASStart.Name = "btnAASStart";
      this.btnAASStart.Size = new Size(139, 23);
      this.btnAASStart.TabIndex = 1;
      this.btnAASStart.Text = "Start";
      this.btnAASStart.UseVisualStyleBackColor = true;
      this.btnAASStart.Click += new EventHandler(this.btnAASStart_Click);
      this.comboBoxAgents.BackColor = Color.White;
      this.comboBoxAgents.ForeColor = Color.Black;
      this.comboBoxAgents.FormattingEnabled = true;
      this.comboBoxAgents.Items.AddRange(new object[11]
      {
        (object) "Breach",
        (object) "Cypher",
        (object) "Jett",
        (object) "Phoenix",
        (object) "Sage",
        (object) "Sova",
        (object) "Viper",
        (object) "Brimstone",
        (object) "Omen",
        (object) "Raze",
        (object) "Reyna"
      });
      this.comboBoxAgents.Location = new Point(6, 35);
      this.comboBoxAgents.Name = "comboBoxAgents";
      this.comboBoxAgents.Size = new Size(139, 21);
      this.comboBoxAgents.TabIndex = 0;
      this.comboBoxAgents.Text = "Choose Agent..";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.FromArgb(64, 64, 64);
      this.BackgroundImage = (Image) componentResourceManager.GetObject("$this.BackgroundImage");
      this.BackgroundImageLayout = ImageLayout.Zoom;
      this.ClientSize = new Size(519, 487);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.groupBoxOwn);
      this.Controls.Add((Control) this.btnMinimize);
      this.Controls.Add((Control) this.groupBoxMisc);
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.groupBoxTriggerbot);
      this.Controls.Add((Control) this.groupBoxBhop);
      this.Cursor = Cursors.Arrow;
      this.DoubleBuffered = true;
      this.ForeColor = Color.Transparent;
      this.FormBorderStyle = FormBorderStyle.None;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.Name = nameof (MainMenu);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Valorant Trainer";
      this.groupBoxBhop.ResumeLayout(false);
      this.groupBoxTriggerbot.ResumeLayout(false);
      this.groupBoxMisc.ResumeLayout(false);
      this.groupBoxMisc.PerformLayout();
      this.trackBarOpacity.EndInit();
      this.groupBoxOwn.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
