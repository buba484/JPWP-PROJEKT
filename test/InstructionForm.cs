using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

public class InstructionForm : Form
{
    private Button backButton;
    private MainForm mainForm;
    private PrivateFontCollection privateFonts;
    public InstructionForm(MainForm mainForm)
    {
        LoadCustomFont();
        this.mainForm = mainForm;
        this.Text = "Instrukcja";
        this.ClientSize = new System.Drawing.Size(900, 600);
        this.BackgroundImage = Image.FromFile("formbg2.jpg");
        this.BackgroundImageLayout = ImageLayout.Stretch;
        var instruction = new Label
        {
            Text = "Przed rozpoczęciem każdego poziomu \n zostanie wyświetlona instrukcja \n jak wygląda dany poziom,\n po przeczytaniu nacisnij \n przycisk aby rozpocząć gre ",
            AutoSize = true,
            Left = 10,
            Top = 50
            
        };


        backButton = new Button { Text = "Powrót", Left = 150, Top = 500, Width = 180, Height = 50 };
        backButton.Click += backButton_Click;
        backButton.FlatStyle = FlatStyle.Flat;
        backButton.FlatAppearance.BorderSize = 0;
        backButton.BackColor = Color.Transparent;
        backButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
        backButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
        backButton.Font = new Font(privateFonts.Families[0], 12, FontStyle.Regular);
        instruction.BackColor = Color.Transparent;
        instruction.Font = new Font(privateFonts.Families[0], 12, FontStyle.Regular); // Zwiększenie rozmiaru czcionki
        this.Controls.Add(instruction);
        this.Controls.Add(backButton);
    }
    private void backButton_Click(object sender, EventArgs e)
    {
        this.Close();
        mainForm.Show();
    }
    private void LoadCustomFont()
    {
        privateFonts = new PrivateFontCollection();
        // Ścieżka do czcionki (zakładając, że czcionka jest w katalogu wyjściowym)

        privateFonts.AddFontFile("ArchitectsDaughter.ttf");// Zmiana na nazwę swojej czcionki

    }   
}