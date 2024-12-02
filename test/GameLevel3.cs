using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public class GameLevel3 : Form
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
    private int badHits;
    public GameLevel3(MainForm mainForm)
    {
        this.mainForm = mainForm;
        this.Text = "Poziom Gry";
        this.ClientSize = new System.Drawing.Size(1280, 720);
        this.DoubleBuffered = true;
        this.BackgroundImage = Image.FromFile("bglv3.jpg"); //ustawienie tła z pliku
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
        pauseButton.FlatStyle = FlatStyle.Flat;
        pauseButton.FlatAppearance.BorderSize = 0;
        pauseButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
        pauseButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
        var exitButton = new Button { Text = "Wyjdź", Left = 120, Width = 100 };
        exitButton.Click += ExitButton_Click;
        pauseButton.BackColor = Color.Transparent;
        exitButton.BackColor = Color.Transparent;
        exitButton.FlatStyle = FlatStyle.Flat;
        exitButton.FlatAppearance.BorderSize = 0;
        exitButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
        exitButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
        menuPanel.Controls.Add(pauseButton);
        menuPanel.Controls.Add(exitButton);

        this.Controls.Add(menuPanel);

        // Elementy gry
        scoreLabel = new Label { Text = "𝓟𝓾𝓷𝓴𝓽𝔂: 0", Left = 10, Top = 10, AutoSize = true };
        timeLabel = new Label { Text = "𝓒𝔃𝓪𝓼: 15", Left = 200, Top = 10, AutoSize = true };
        scoreLabel.BackColor = Color.Transparent;
        timeLabel.BackColor = Color.Transparent;
        this.Controls.Add(scoreLabel);
        this.Controls.Add(timeLabel);

        gameTimer = new Timer { Interval = 1000 };
        gameTimer.Tick += GameTimer_Tick;

        random = new Random();
        targets = new List<Target>();

        this.FormClosing += GameLevel3_FormClosing;
        StartLevel();
    }

    private void StartLevel()
    {
        score = 0;
        badHits = 0;
        timeRemaining = 15;
        var instructionForm = new InstructionFormAdvance("kota", 15, timeRemaining);
        instructionForm.ShowDialog();
        gameTimer.Start();
        for (int i = 0; i < 5; i++)
        {
            CreateTargetsgood("cat.png");
        }
        for (int i = 0; i < 3; i++)
        {
            CreateTargetsbad("dog.png");
        }
        CreateTargetsbad("croc.png");
        CreateTargetsbad("rhino.png");
    }

    private void CreateTargetsgood(string file)
    {
        Target target;
        do
        {
            target = new Target
            {
                Location = new Point(random.Next(100, 1100), random.Next(100, 620)),
                Size = new Size(100, 100) // Ustaw rozmiar celu
            };
        } while (IsOverlapping(target));

        // Upewnij się, że ścieżka do pliku jest poprawna
        target.BackgroundImage = Image.FromFile(file); // Ustaw ścieżkę do swojego pliku PNG
        target.BackgroundImageLayout = ImageLayout.Stretch; // Ustaw sposób wyświetlania obrazu // Ustaw sposób wyświetlania obrazu
        target.Click += Target_addpoint;
        target.Tag = file;
        targets.Add(target);
        this.Controls.Add(target);
    }
    private void CreateTargetsbad(string file)
    {
        Target target;
        do
        {
            target = new Target
            {
                Location = new Point(random.Next(100, 1100), random.Next(100, 620)),
                Size = new Size(100, 100) // Ustaw rozmiar celu
            };
        } while (IsOverlapping(target));

        // Upewnij się, że ścieżka do pliku jest poprawna
        target.BackgroundImage = Image.FromFile(file); // Ustaw ścieżkę do swojego pliku PNG
        target.BackgroundImageLayout = ImageLayout.Stretch; // Ustaw sposób wyświetlania obrazu
        target.Click += Target_subpoint;
        target.Tag = file;
        targets.Add(target);
        this.Controls.Add(target);
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
    private void Target_addpoint(object sender, EventArgs e)
    {
        var clickedTarget = sender as Target;
        string fileName = clickedTarget.Tag.ToString(); //zczytanie nazwy celu
        if (clickedTarget != null)
        {
            this.Controls.Remove(clickedTarget);
            targets.Remove(clickedTarget);
            CreateTargetsgood(fileName);
            score += 3;
            scoreLabel.Text = $"𝓟𝓾𝓷𝓴𝓽𝔂: {score}";
        }
        CheckScore();
    }
    private void Target_subpoint(object sender, EventArgs e)
    {
        var clickedTarget = sender as Target;
        string fileName = clickedTarget.Tag.ToString(); //zczytanie nazwy celu
        if (clickedTarget != null)
        {

            this.Controls.Remove(clickedTarget);
            targets.Remove(clickedTarget);
            CreateTargetsbad(fileName);
            score -= 2;
            scoreLabel.Text = $"𝓟𝓾𝓷𝓴𝓽𝔂: {score}";
            badHits++;
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
                //var gameLevel = new GameLevel2(mainForm);
                //gameLevel.ShowDialog();
            }
        }
    }
    private void GameTimer_Tick(object sender, EventArgs e)
    {
        timeRemaining--;
        timeLabel.Text = $"𝓒𝔃𝓪𝓼: {timeRemaining}";

        if (timeRemaining <= 0)
        {
            gameTimer.Stop();
            var result = MessageBox.Show($"Koniec poziomu! Zdobyte punkty: {score}", "Koniec gry", MessageBoxButtons.OK);
            if (result == DialogResult.OK)
            {
                if (score < 15)
                {
                    if (this.badHits <= 4)
                    {
                        var result2 = MessageBox.Show("Nie zdobyłeś wymaganej ilości punktów, lecz masz możliwość zwiększenia czasu rozgrywki o 15 sekund, musisz jedynie rozwiązać kalambur, aby go rozwiązać wcisnij yes", "Kalambur", MessageBoxButtons.YesNo);
                        if (result2 == DialogResult.Yes)
                        {
                            Kalambury kalambury = new Kalambury(mainForm);
                            kalambury.ShowDialog();
                            int time = kalambury.czas;
                            timeRemaining = +time;
                            gameTimer.Start();
                            badHits = 0;

                        }
                        else
                        {
                            this.Close();
                            mainForm.Show();
                        }
                    }
                    else
                    {


                        var result3 = MessageBox.Show("Nie zdobyłeś wymaganej ilości punktów, aby spróbować jeszcze raz wciśnij tak aby wyjść do menu wcisnij nie", "Koniec gry", MessageBoxButtons.YesNo);
                        if (result3 == DialogResult.Yes)
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
    protected void GameLevel3_FormClosing(object sender, FormClosingEventArgs e)
    {
        gameTimer.Stop();
        var result = MessageBox.Show("Czy na pewno chcesz zamknąć grę?", "Potwierdzenie", MessageBoxButtons.YesNo);
        if (result == DialogResult.No)
        {
            e.Cancel = true; // Anuluj zamykanie formularza
            if(timeRemaining>0)
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
