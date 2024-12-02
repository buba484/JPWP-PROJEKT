using System;
using System.Windows.Forms;

public class InstructionFormBasic : Form
{
    public InstructionFormBasic(string kolor,int pkt,int time)
    {
        this.Text = "Instrukcja";
        this.ClientSize = new System.Drawing.Size(800, 600);

        var cel = new Label
        {
            Text = "Klikaj w "+ kolor + " kształty, aby zdobywać punkty. Unikaj innych celów!",
            AutoSize = true,
            Left = 50,
            Top = 50
        };
        var punkty = new Label
        {
            Text = "Do zdobycia masz "+ Convert.ToString(pkt) +" punktów",
            AutoSize = true,
            Left = 50,
            Top = 100
        };
        var czas = new Label
        {
            Text = "Masz " + Convert.ToString(time) + " sekund",
            AutoSize = true,
            Left = 50,
            Top = 100
        };

        var startButton = new Button
        {
            Text = "Rozpocznij",
            Left = 350,
            Top = 200,
            Width = 100
        };
        startButton.Click += (sender, e) => this.Close();

        this.Controls.Add(cel);
        this.Controls.Add(punkty);
        this.Controls.Add(czas);
        this.Controls.Add(startButton);
    }
}