using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class GameLevel : Form
{
    private Timer gameTimer;
    private Label scoreLabel;
    private Label timeLabel;
    private Random random;
    private int score;
    private int timeRemaining;
    private List<Target> targets;

    public GameLevel()
    {
        this.Text = "Poziom Gry";
        this.ClientSize = new System.Drawing.Size(1280, 720);
        this.DoubleBuffered = true;

        scoreLabel = new Label { Text = "Punkty: 0", Left = 10, Top = 10, AutoSize = true };
        timeLabel = new Label { Text = "Czas: 30", Left = 200, Top = 10, AutoSize = true };

        this.Controls.Add(scoreLabel);
        this.Controls.Add(timeLabel);

        gameTimer = new Timer { Interval = 1000 };
        gameTimer.Tick += GameTimer_Tick;

        random = new Random();
        targets = new List<Target>();

        StartLevel();
    }

    private void StartLevel()
    {
        score = 0;
        timeRemaining = 30;
        gameTimer.Start();
        CreateTargets();
    }

    private void CreateTargets()
    {
        for (int i = 0; i < 5; i++)
        {
            var target = new Target
            {
                Location = new Point(random.Next(100, 1180), random.Next(100, 620)),
                BackColor = Color.Red,
                Size = new Size(50, 50)
            };
            target.Click += Target_Click;
            targets.Add(target);
            this.Controls.Add(target);
        }
    }

    private void Target_Click(object sender, EventArgs e)
    {
        var clickedTarget = sender as Target;
        if (clickedTarget != null)
        {
            this.Controls.Remove(clickedTarget);
            targets.Remove(clickedTarget);
            score += 3;
            scoreLabel.Text = $"Punkty: {score}";
        }
    }

    private void GameTimer_Tick(object sender, EventArgs e)
    {
        timeRemaining--;
        timeLabel.Text = $"Czas: {timeRemaining}";

        if (timeRemaining <= 0)
        {
            gameTimer.Stop();
            MessageBox.Show($"Koniec poziomu! Zdobyte punkty: {score}", "Koniec gry");
            this.Close();
        }
    }
}