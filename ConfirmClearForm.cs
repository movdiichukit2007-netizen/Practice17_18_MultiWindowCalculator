using System;
using System.Drawing;
using System.Windows.Forms;

namespace Practice17_18_MultiWindowCalculator;

public class ConfirmClearForm : Form
{
    private readonly Button buttonYes = new();
    private readonly Button buttonNo = new();

    public ConfirmClearForm()
    {
        Text = "Підтвердження";
        Size = new Size(470, 180);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MinimizeBox = false;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterParent;

        Label labelQuestion = new()
        {
            Text = "Ви дійсно хочете очистити вміст калькулятора?",
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Location = new Point(20, 18),
            Size = new Size(420, 48),
            Font = new Font("Segoe UI", 10, FontStyle.Regular)
        };

        buttonYes.Name = "buttonYes";
        buttonYes.Text = "Так";
        buttonYes.Location = new Point(126, 82);
        buttonYes.Size = new Size(90, 32);
        buttonYes.Click += ButtonYes_Click;

        buttonNo.Name = "buttonNo";
        buttonNo.Text = "Ні";
        buttonNo.Location = new Point(250, 82);
        buttonNo.Size = new Size(90, 32);
        buttonNo.Click += ButtonNo_Click;

        AcceptButton = buttonYes;
        CancelButton = buttonNo;

        Controls.Add(labelQuestion);
        Controls.Add(buttonYes);
        Controls.Add(buttonNo);
    }

    private void ButtonYes_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Yes;
        Close();
    }

    private void ButtonNo_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.No;
        Close();
    }
}
