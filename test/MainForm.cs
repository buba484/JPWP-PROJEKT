using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing.Text;

public class MainForm : Form
{
    private Button startGameButton;
    private Button exitButton;
    private Button instructionButton;
    private Container components;

    public MainForm()
    {
       
        InitializeComponent();
        var name = new Label
        {
            Text = "𝓒𝓮𝓵𝓾𝓳 𝓲 𝓽𝓻𝓪𝓯𝓲𝓪𝓳!",
            AutoSize = false,
            Top = 50,
            Height = 100,
            BackColor = Color.Transparent,
            Font = new Font("Arial", 36, FontStyle.Bold)
        };
        name.TextAlign = ContentAlignment.MiddleCenter; // Ustawienie wyśrodkowania tekstu
        name.Width = this.ClientSize.Width; // Ustaw szerokość etykiety na szerokość formularza


        startGameButton = new Button { Text = "𝓢𝓽𝓪𝓻𝓽 𝓖𝓻𝔂", Left = 550, Top = 300, Width = 180 };
        startGameButton.Click += StartGameButton_Click;

        instructionButton = new Button { Text = "𝓘𝓷𝓼𝓽𝓻𝓾𝓴𝓬𝓳𝓪", Left = 550, Top = 360, Width = 180 };
        instructionButton.Click += InstructionButton_Click;

        exitButton = new Button { Text = "𝓦𝔂𝓳𝓭ź", Left = 550, Top = 420, Width = 180 };
        exitButton.Click += ExitButton_Click;
        startGameButton.FlatStyle = FlatStyle.Flat;
        startGameButton.FlatAppearance.BorderSize = 0;
        startGameButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
        startGameButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
        startGameButton.BackColor = Color.Transparent;
        instructionButton.BackColor = Color.Transparent;
        exitButton.BackColor = Color.Transparent;
        

        this.Controls.Add(name);
        this.Controls.Add(startGameButton);
        this.Controls.Add(instructionButton);
        this.Controls.Add(exitButton);                    
        this.BackgroundImage = Image.FromFile("bg1.jpg"); 
        this.BackgroundImageLayout = ImageLayout.Stretch; //ustawienie wyswiatlania tla
        
    }
    private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Text = "Celuj i Trafiaj"; ;
        }
    private void StartGameButton_Click(object sender, EventArgs e)
    {
        this.Hide();
        MainForm mainForm = new MainForm();
        //GameLevel1 gameLevel = new GameLevel1(this);       
        //gameLevel.ShowDialog();       
        GameLevel3 gameLevel = new GameLevel3(this);
        gameLevel.ShowDialog();
        //Kalambury kalambury = new Kalambury();
        //kalambury.ShowDialog();
    }
    private void InstructionButton_Click(object sender, EventArgs e)
    {
        this.Hide();        
        InstructionForm instructionForm = new InstructionForm(this);
        instructionForm.ShowDialog();
    }
    private void ExitButton_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }
}
