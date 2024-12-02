using System;
using System.Windows.Forms;

public class InstructionForm : Form
{
    private Button backButton;
    private MainForm mainForm;

    public InstructionForm(MainForm mainForm)
    {
        this.mainForm = mainForm;
        this.Text = "Instrukcja";
        this.ClientSize = new System.Drawing.Size(900, 600);

        var instruction = new Label
        {
            Text = "Przed rozpoczęciem każdego poziomu zostanie wyświetlona instrukcja jak wygląda dany poziom, po przeczytaniu nacisnij przycisk aby rozpocząć gre",
            AutoSize = true,
            Left = 50,
            Top = 50
        };

        backButton = new Button { Text = "Powrót", Left = 350, Top = 300, Width = 180 };
        backButton.Click += backButton_Click;

        
        this.Controls.Add(instruction);;
        this.Controls.Add(backButton);
    }
    private void backButton_Click(object sender, EventArgs e)
    {
        this.Close();
        mainForm.Show();
    }
}