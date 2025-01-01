using System;
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
        this.BackgroundImage = Image.FromFile("bglv2.jpg");
        this.BackgroundImageLayout = ImageLayout.Stretch;
        // Pasek menu
        menuPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 50,
            BackColor = Color.Transparent
        };

        // Przyciski w menu
        var pauseButton = new Button { Text = "Pauza", Left = 10, Width = 100 };
        pauseButton.Click += PauseButton_Click;
        pauseButton.Click += PauseButton_Click;
        pauseButton.FlatStyle = FlatStyle.Flat;
        pauseButton.FlatAppearance.BorderSize = 0;
        pauseButton.BackColor = Color.Transparent;
        pauseButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
        pauseButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
        var exitButton = new Button { Text = "Wyjdź", Left = 120, Width = 100 };
        exitButton.Click += ExitButton_Click;
        exitButton.Click += ExitButton_Click;
        exitButton.BackColor = Color.Transparent;
        exitButton.FlatStyle = FlatStyle.Flat;
        exitButton.FlatAppearance.BorderSize = 0;
        exitButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
        exitButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
        menuPanel.Controls.Add(pauseButton);
        menuPanel.Controls.Add(exitButton);

        this.Controls.Add(menuPanel);

        // Elementy gry
        scoreLabel = new Label { Text = "Punkty: 0", Left = 10, Top = 10, AutoSize = true };
        timeLabel = new Label { Text = "Czas: 15", Left = 200, Top = 10, AutoSize = true };
        scoreLabel.BackColor = Color.Transparent;
        timeLabel.BackColor = Color.Transparent;
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
        var instructionForm = new InstructionFormBasic("psa", 35, timeRemaining);
        instructionForm.ShowDialog();
        gameTimer.Start();
        CreateTargetsgood();
        CreateTargetsgood();
        CreateTargetsbad();
        CreateTargetsbad();
        CreateTargetsbad();
        CreateTargetsbad2();
        CreateTargetsbad2();
    }

    private void CreateTargetsgood()
    {
        Target target;
        do
        {
            target = new Target
            {
                Location = new Point(random.Next(100, 1180), random.Next(100, 620)),
                Size = new Size(100, 100)
            };
        } while (IsOverlapping(target));
        target.BackgroundImage = Image.FromFile("dog.png");
        target.BackgroundImageLayout = ImageLayout.Stretch;
        target.Click += Target_addpoint;
        targets.Add(target);
        this.Controls.Add(target);
    }
    private void CreateTargetsbad()
    {
        Target target;
        do
        {
            target = new Target
            {
                Location = new Point(random.Next(100, 1180), random.Next(100, 620)),
                Size = new Size(150, 150)
            };
        } while (IsOverlapping(target));
        target.BackgroundImage = Image.FromFile("rhino.png");
        target.BackgroundImageLayout = ImageLayout.Stretch;
        target.Click += Target_subpoint;
        targets.Add(target);
        this.Controls.Add(target);
    }
    private void CreateTargetsbad2()
    {
        Target target;
        do
        {
            target = new Target
            {
                Location = new Point(random.Next(100, 1180), random.Next(100, 620)),
                Size = new Size(150, 150)
            };
        } while (IsOverlapping(target));
        target.BackgroundImage = Image.FromFile("croc.png");
        target.BackgroundImageLayout = ImageLayout.Stretch;
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
        if (score >= 35)
        {
            gameTimer.Stop();
            var result = MessageBox.Show($"Zdobyłeś wymagane punkty : {score}", "Kliknij Ok aby przejść dalej", MessageBoxButtons.OK);
            if (result == DialogResult.OK)
            {

                this.Hide();
                GameLevel3 gameLevel3 = new GameLevel3(mainForm);
                gameLevel3.Show();
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
                        this.Hide();
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
    private bool IsOverlapping(Target newTarget)
    {
        foreach (var existingTarget in targets)
        {
            // Sprawdź, czy prostokąty celów się nakładają
            if (existingTarget.Bounds.IntersectsWith(newTarget.Bounds))
            {
                return true; // Nakładają się
            }
        }
        return false; // Nie nakładają się
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
    private void PauseButton_Click(object sender, EventArgs e)
    {
        if (gameTimer.Enabled)
        {
            gameTimer.Stop();
            MessageBox.Show("Gra zapauzowana. Kliknij OK, aby kontynuować.", "Pauza");
            gameTimer.Start();
        }
    }


    protected void GameLevel2_FormClosing(object sender, FormClosingEventArgs e)
    {
        gameTimer.Stop();
        var result = MessageBox.Show("Czy na pewno chcesz zamknąć grę?", "Potwierdzenie", MessageBoxButtons.YesNo);
        if (result == DialogResult.No)
        {
            e.Cancel = true; // Anuluj zamykanie formularza
            if (timeRemaining > 0)
            {
                gameTimer.Start();
            }
            else
            {
                this.Close();
                GameLevel2 gameLevel2 = new GameLevel2(mainForm);
                gameLevel2.ShowDialog();
            }

        }
        else
        {
            mainForm.Show();
        }

    }
}