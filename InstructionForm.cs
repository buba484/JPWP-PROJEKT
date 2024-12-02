using System.Windows.Forms;

public class InstructionForm : Form
{
    public InstructionForm()
    {
        this.Text = "Instrukcja";
        this.ClientSize = new System.Drawing.Size(800, 600);

        var instructionLabel = new Label
        {
            Text = "Klikaj w czerwone kształty, aby zdobywać punkty. Unikaj innych celów!",
            AutoSize = true,
            Left = 50,
            Top = 50
        };

        var startButton = new Button
        {
            Text = "Rozpocznij",
            Left = 350,
            Top = 200,
            Width = 100
        };
        startButton.Click += (sender, e) => this.Close();

        this.Controls.Add(instructionLabel);
        this.Controls.Add(startButton);
    }
}