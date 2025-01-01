using System;
using System.Drawing;
using System.Windows.Forms;

public class InstructionFormAdvance : Form
{
    public InstructionFormAdvance(string target_good,int pkt,int time)
    {
        this.Text = "Instrukcja";
        this.ClientSize = new System.Drawing.Size(800, 600);
        //this.BackgroundImage = Image.FromFile("formbg.png");
        var cel_dobry = new Label
        {
            Text = "Klikaj w "+ target_good + " , aby zdobywać punkty. Unikaj innych celów!",
            AutoSize = true,
            Left = 50,
            Top = 50
        };
        var cel_zly = new Label
        {
            Text = "Nie klikaj w zwierzęta inaczej stracisz punkty",
            AutoSize = true,
            Left = 50,
            Top = 100
        };
        var punkty = new Label
        {
            Text = "Do zdobycia masz "+ Convert.ToString(pkt) +" punktów",
            AutoSize = true,
            Left = 50,
            Top = 150
        };
        var czas = new Label
        {
            Text = "Masz " + Convert.ToString(time) + " sekund",
            AutoSize = true,
            Left = 50,
            Top = 200
        };

        var startButton = new Button
        {
            Text = "Rozpocznij",
            Left = 350,
            Top = 250,
            Width = 100
        };
        startButton.Click += (sender, e) => this.Close();

        this.Controls.Add(cel_dobry);
        this.Controls.Add(cel_zly);
        this.Controls.Add(punkty);
        this.Controls.Add(czas);
        this.Controls.Add(startButton);
    }
}