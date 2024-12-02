﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class GameLevel2 : Form
{
    private Timer gameTimer;
    private Label scoreLabel;
    private Label timeLabel;
    private Random random;
    private int score;
    private int timeRemaining;
    private List<Target> targets;
    private Panel menuPanel;
    private MainForm mainForm;

    public GameLevel2(MainForm mainForm)
    {
        this.mainForm = mainForm;
        this.Text = "Poziom Gry";
        this.ClientSize = new System.Drawing.Size(1280, 720);
        this.DoubleBuffered = true;

        // Pasek menu
        menuPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 50,
            BackColor = Color.LightGray
        };

        // Przyciski w menu
        var pauseButton = new Button { Text = "Pauza", Left = 10, Width = 100 };
        pauseButton.Click += PauseButton_Click;

        var exitButton = new Button { Text = "Wyjdź", Left = 120, Width = 100 };
        exitButton.Click += ExitButton_Click;

        menuPanel.Controls.Add(pauseButton);
        menuPanel.Controls.Add(exitButton);

        this.Controls.Add(menuPanel);

        // Elementy gry
        scoreLabel = new Label { Text = "Punkty: 0", Left = 10, Top = 10, AutoSize = true };
        timeLabel = new Label { Text = "Czas: 30", Left = 200, Top = 10, AutoSize = true };

        this.Controls.Add(scoreLabel);
        this.Controls.Add(timeLabel);

        gameTimer = new Timer { Interval = 1000 };
        gameTimer.Tick += GameTimer_Tick;

        random = new Random();
        targets = new List<Target>();

        this.FormClosing += GameLevel2_FormClosing;
        StartLevel();
    }

    private void StartLevel()
    {
        score = 0;
        timeRemaining = 15;
        var instructionForm = new InstructionFormBasic("fioletowe", 20, timeRemaining);
        instructionForm.ShowDialog();
        gameTimer.Start();
        CreateTargetsgood();
        CreateTargetsbad();
    }

    private void CreateTargetsgood()
    {
        var target = new Target
        {
            Location = new Point(random.Next(100, 1180), random.Next(100, 620)),
            BackColor = Color.Red,
            Size = new Size(50, 50)
        };
        target.Click += Target_addpoint;
        targets.Add(target);
        this.Controls.Add(target);
    }
    private void CreateTargetsbad()
    {
        var target = new Target
        {
            Location = new Point(random.Next(100, 1180), random.Next(100, 620)),
            BackColor = Color.Red,
            Size = new Size(50, 50)
        };
        target.Click += Target_subpoint;
        targets.Add(target);
        this.Controls.Add(target);
    }

    private void Target_addpoint(object sender, EventArgs e)
    {
        var clickedTarget = sender as Target;
        if (clickedTarget != null)
        {
            this.Controls.Remove(clickedTarget);
            targets.Remove(clickedTarget);
            CreateTargetsgood();
            score += 3;
            scoreLabel.Text = $"Punkty: {score}";
        }
        CheckScore();
    }
    private void Target_subpoint(object sender, EventArgs e)
    {
        var clickedTarget = sender as Target;
        if (clickedTarget != null)
        {
            this.Controls.Remove(clickedTarget);
            targets.Remove(clickedTarget);
            CreateTargetsbad();
            score -= 2;
            scoreLabel.Text = $"Punkty: {score}";
        }
        CheckScore();
    }
    private void CheckScore()
    {
        if (score >= 15)
        {
            gameTimer.Stop();
            var result = MessageBox.Show($"Zdobyłeś wymagane punkty : {score}", "Kliknij Ok aby przejść dalej", MessageBoxButtons.OK);
            if (result == DialogResult.OK)
            {

                this.Close();
                var gameLevel = new GameLevel2(mainForm);
                gameLevel.ShowDialog();
            }
        }
    }
    private void GameTimer_Tick(object sender, EventArgs e)
    {
        timeRemaining--;
        timeLabel.Text = $"Czas: {timeRemaining}";

        if (timeRemaining <= 0)
        {
            gameTimer.Stop();
            var result = MessageBox.Show($"Koniec poziomu! Zdobyte punkty: {score}", "Koniec gry", MessageBoxButtons.OK);
            if (result == DialogResult.OK)
            {
                if (score < 15)
                {
                    var result2 = MessageBox.Show("Nie zdobyłeś wymaganej ilości punktów, aby spróbować jeszcze raz wciśnij tak aby wyjść do menu wcisnij nie", "Koniec gry", MessageBoxButtons.YesNo);
                    if (result2 == DialogResult.Yes)
                    {
                        this.Close();
                        GameLevel2 gameLevel2 = new GameLevel2(mainForm);
                        gameLevel2.ShowDialog();
                    }
                    else
                    {
                        this.Close();
                        mainForm.Show();
                    }
                }
            }
        }
    }

    private void PauseButton_Click(object sender, EventArgs e)
    {
        if (gameTimer.Enabled)
        {
            gameTimer.Stop();
            MessageBox.Show("Gra zapauzowana. Kliknij OK, aby kontynuować.", "Pauza");
            gameTimer.Start();
        }
    }

    private void ExitButton_Click(object sender, EventArgs e)
    {
        var result = MessageBox.Show("Czy na pewno chcesz zakończyć grę?", "Wyjście", MessageBoxButtons.YesNo);
        if (result == DialogResult.Yes)
        {
            this.Close();
            mainForm.Show();
        }
    }
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        gameTimer.Stop();
        var result = MessageBox.Show("Czy na pewno chcesz zamknąć grę?", "Potwierdzenie", MessageBoxButtons.YesNo);
        if (result == DialogResult.No)
        {
            e.Cancel = true; // Anuluj zamykanie formularza
            gameTimer.Start();
        }
        else
        {
            base.OnFormClosing(e); // Kontynuuj zamykanie formularza
            mainForm.Show();

        }
    }
    protected void GameLevel2_FormClosing(object sender, FormClosingEventArgs e)
    {
        gameTimer.Stop();
        var result = MessageBox.Show("Czy na pewno chcesz zamknąć grę?", "Potwierdzenie", MessageBoxButtons.YesNo);
        if (result == DialogResult.No)
        {
            e.Cancel = true; // Anuluj zamykanie formularza
            gameTimer.Start();
        }
        else
        {
            mainForm.Show();
        }

    }
}