using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace Practice17_18_MultiWindowCalculator;

public class MainForm : Form
{
    private readonly Label lblDisplay = new();
    private readonly TableLayoutPanel tableButtons = new();
    private readonly RadioButton radioOn = new();
    private readonly RadioButton radioOff = new();

    private double firstNumber;
    private string operation = "";
    private bool isOperationSelected;

    public MainForm()
    {
        Text = "Калькулятор";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(660, 540);
        Size = new Size(700, 560);
        BackColor = Color.FromArgb(245, 245, 245);
        KeyPreview = true;

        Panel pnlDisplay = new()
        {
            Dock = DockStyle.Top,
            Height = 110,
            Padding = new Padding(14, 18, 14, 10)
        };

        lblDisplay.Name = "lblDisplay";
        lblDisplay.Text = "4453";
        lblDisplay.Dock = DockStyle.Fill;
        lblDisplay.BackColor = Color.WhiteSmoke;
        lblDisplay.ForeColor = Color.DarkGreen;
        lblDisplay.TextAlign = ContentAlignment.MiddleRight;
        lblDisplay.Font = new Font("Segoe UI", 26, FontStyle.Regular);
        lblDisplay.BorderStyle = BorderStyle.FixedSingle;
        pnlDisplay.Controls.Add(lblDisplay);

        Panel pnlState = new()
        {
            Dock = DockStyle.Top,
            Height = 36,
            Padding = new Padding(14, 0, 14, 0)
        };

        radioOn.Name = "radioOn";
        radioOn.Text = "ON";
        radioOn.ForeColor = Color.Green;
        radioOn.Checked = true;
        radioOn.AutoSize = true;
        radioOn.Location = new Point(16, 7);

        radioOff.Name = "radioOff";
        radioOff.Text = "OFF";
        radioOff.ForeColor = Color.Red;
        radioOff.AutoSize = true;
        radioOff.Location = new Point(72, 7);

        pnlState.Controls.Add(radioOn);
        pnlState.Controls.Add(radioOff);

        tableButtons.Dock = DockStyle.Fill;
        tableButtons.ColumnCount = 4;
        tableButtons.RowCount = 5;
        tableButtons.Padding = new Padding(14);
        tableButtons.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;

        for (int i = 0; i < 4; i++)
        {
            tableButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        }

        for (int i = 0; i < 5; i++)
        {
            tableButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
        }

        Button btnCe = AddButton("CE", 0, 0, ButtonCE_Click);
        btnCe.ForeColor = Color.OrangeRed;
        AddButton("←", 1, 0, Backspace_Click);
        AddButton("%", 2, 0, Percent_Click);
        AddButton("/", 3, 0, Operation_Click);
        AddButton("7", 0, 1, Number_Click);
        AddButton("8", 1, 1, Number_Click);
        AddButton("9", 2, 1, Number_Click);
        AddButton("*", 3, 1, Operation_Click);
        AddButton("4", 0, 2, Number_Click);
        AddButton("5", 1, 2, Number_Click);
        AddButton("6", 2, 2, Number_Click);
        AddButton("-", 3, 2, Operation_Click);
        AddButton("1", 0, 3, Number_Click);
        AddButton("2", 1, 3, Number_Click);
        AddButton("3", 2, 3, Number_Click);
        AddButton("+", 3, 3, Operation_Click);
        AddButton("0", 0, 4, Number_Click);
        AddButton(".", 1, 4, Number_Click);
        AddButton("=", 2, 4, Equal_Click, 2);

        Controls.Add(tableButtons);
        Controls.Add(pnlState);
        Controls.Add(pnlDisplay);
        KeyDown += Form_KeyDown;
    }

    private Button AddButton(string text, int column, int row, EventHandler handler, int columnSpan = 1)
    {
        Button button = new()
        {
            Text = text,
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 24, FontStyle.Regular),
            Margin = new Padding(5),
            FlatStyle = FlatStyle.Standard,
            BackColor = Color.WhiteSmoke,
            UseVisualStyleBackColor = true
        };

        button.Click += handler;
        tableButtons.Controls.Add(button, column, row);

        if (columnSpan > 1)
        {
            tableButtons.SetColumnSpan(button, columnSpan);
        }

        return button;
    }

    private void ButtonCE_Click(object? sender, EventArgs e)
    {
        using ConfirmClearForm confirm = new();
        DialogResult result = confirm.ShowDialog(this);

        if (result == DialogResult.Yes)
        {
            ClearDisplay();
        }
    }

    private void Number_Click(object? sender, EventArgs e)
    {
        string symbol = ((Button)sender!).Text;
        AddSymbol(symbol);
    }

    private void AddSymbol(string symbol)
    {
        if (lblDisplay.Text == "0" || isOperationSelected)
        {
            lblDisplay.Text = "";
            isOperationSelected = false;
        }

        if (symbol == "." && lblDisplay.Text.Contains('.'))
        {
            return;
        }

        lblDisplay.Text += symbol;
    }

    private void Operation_Click(object? sender, EventArgs e)
    {
        firstNumber = GetDisplayNumber();
        operation = ((Button)sender!).Text;
        isOperationSelected = true;
    }

    private void Equal_Click(object? sender, EventArgs e)
    {
        double secondNumber = GetDisplayNumber();
        double result;

        switch (operation)
        {
            case "+":
                result = firstNumber + secondNumber;
                break;
            case "-":
                result = firstNumber - secondNumber;
                break;
            case "*":
                result = firstNumber * secondNumber;
                break;
            case "/":
                if (Math.Abs(secondNumber) < double.Epsilon)
                {
                    MessageBox.Show("Ділення на нуль заборонено.", "Помилка");
                    return;
                }

                result = firstNumber / secondNumber;
                break;
            default:
                return;
        }

        lblDisplay.Text = result.ToString(CultureInfo.InvariantCulture);
        operation = "";
        isOperationSelected = true;
    }

    private void Backspace_Click(object? sender, EventArgs e)
    {
        if (lblDisplay.Text.Length <= 1)
        {
            lblDisplay.Text = "0";
            return;
        }

        lblDisplay.Text = lblDisplay.Text[..^1];
    }

    private void Percent_Click(object? sender, EventArgs e)
    {
        double value = GetDisplayNumber();
        lblDisplay.Text = (value / 100).ToString(CultureInfo.InvariantCulture);
    }

    private void ClearDisplay()
    {
        lblDisplay.Text = "0";
        firstNumber = 0;
        operation = "";
        isOperationSelected = false;
    }

    private void Form_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9)
        {
            AddSymbol(((int)(e.KeyCode - Keys.D0)).ToString());
        }
        else if (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9)
        {
            AddSymbol(((int)(e.KeyCode - Keys.NumPad0)).ToString());
        }
        else if (e.KeyCode == Keys.Decimal || e.KeyCode == Keys.OemPeriod)
        {
            AddSymbol(".");
        }
        else if (e.KeyCode == Keys.Add)
        {
            SelectOperation("+");
        }
        else if (e.KeyCode == Keys.Subtract || e.KeyCode == Keys.OemMinus)
        {
            SelectOperation("-");
        }
        else if (e.KeyCode == Keys.Multiply)
        {
            SelectOperation("*");
        }
        else if (e.KeyCode == Keys.Divide)
        {
            SelectOperation("/");
        }
        else if (e.KeyCode == Keys.Enter)
        {
            Equal_Click(this, EventArgs.Empty);
        }
    }

    private void SelectOperation(string selectedOperation)
    {
        firstNumber = GetDisplayNumber();
        operation = selectedOperation;
        isOperationSelected = true;
    }

    private double GetDisplayNumber()
    {
        return double.TryParse(lblDisplay.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double value)
            ? value
            : 0;
    }

    public void CreateScreenshots(string outputDirectory)
    {
        StartPosition = FormStartPosition.Manual;
        Location = new Point(80, 80);
        Show();
        Application.DoEvents();
        SaveFormImage(Path.Combine(outputDirectory, "practice17_18_calculator.png"));

        using ConfirmClearForm confirm = new();
        confirm.StartPosition = FormStartPosition.Manual;
        confirm.Location = new Point(Location.X + 70, Location.Y + 205);
        confirm.Show(this);
        Application.DoEvents();
        SaveControlImage(confirm, Path.Combine(outputDirectory, "practice17_18_confirm.png"));
        SaveOverlayImage(confirm, new Point(70, 205), Path.Combine(outputDirectory, "practice17_18_dialog_on_calculator.png"));
        confirm.Close();
        Hide();
    }

    private void SaveFormImage(string path)
    {
        using Bitmap bitmap = new(Width, Height);
        DrawToBitmap(bitmap, new Rectangle(0, 0, Width, Height));
        bitmap.Save(path);
    }

    private static void SaveControlImage(Control control, string path)
    {
        using Bitmap bitmap = new(control.Width, control.Height);
        control.DrawToBitmap(bitmap, new Rectangle(0, 0, control.Width, control.Height));
        bitmap.Save(path);
    }

    private void SaveOverlayImage(Control overlay, Point overlayLocation, string path)
    {
        using Bitmap baseBitmap = new(Width, Height);
        DrawToBitmap(baseBitmap, new Rectangle(0, 0, Width, Height));

        using Bitmap overlayBitmap = new(overlay.Width, overlay.Height);
        overlay.DrawToBitmap(overlayBitmap, new Rectangle(0, 0, overlay.Width, overlay.Height));

        using Graphics graphics = Graphics.FromImage(baseBitmap);
        graphics.DrawImage(overlayBitmap, overlayLocation);
        baseBitmap.Save(path);
    }
}
