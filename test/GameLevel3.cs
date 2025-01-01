using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

public class GameLevel3 : Form
{
    private HashSet<Target> movingTargets; // Użyj HashSet do unikalnych celów
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
    private Timer moveTargetTimer;
    private bool isMoving;
    private Timer addTargetTimer;
    private int movingTargetCount;
    private int movingTargetCount2;
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
        moveTargetTimer = new Timer { Interval = 50 }; // Co 50 ms
        moveTargetTimer.Tick += MoveTargetTimer_Tick;
        addTargetTimer = new Timer { Interval = 1000 };
        addTargetTimer.Tick += AddTargetTimer_Tick; // Podpięcie zdarzenia
        
        random = new Random();
        targets = new List<Target>();
        movingTargets = new HashSet<Target>();
        isMoving = false;
        this.FormClosing += GameLevel3_FormClosing;
        StartLevel();
    }

    private void StartLevel()
    {
        score = 0;
        badHits = 0;
        timeRemaining = 45;
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
        for (int i = 0; i < 1; i++) // wybierz cel do poruszania
        {
            Target targetToMove = targets[random.Next(targets.Count)];
            if (!movingTargets.Contains(targetToMove))
                {
                    movingTargets.Add(targetToMove);
                    SetRandomDirection(targetToMove); // Ustaw losowy kierunek dla każdego celu
                    movingTargetCount++;
                }
        }
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
        if (timeRemaining <= 30)
        {
            AddTargetTimer(target);
        }
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
        if(timeRemaining <= 30)
        {
            AddTargetTimer(target);
        }       
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
            movingTargetCount--;
            this.Controls.Remove(clickedTarget);
            targets.Remove(clickedTarget);
            CreateTargetsgood(fileName);
            score += 3;
            scoreLabel.Text = $"𝓟𝓾𝓷𝓴𝓽𝔂: {score}";
            isMoving = false;

            // Wybierz inny cel do poruszania
            
        }
        CheckScore();
    }
    private void Target_subpoint(object sender, EventArgs e)
    {
        var clickedTarget = sender as Target;
        string fileName = clickedTarget.Tag.ToString(); //zczytanie nazwy celu
        if (clickedTarget != null)
        {
            movingTargetCount--;
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
        if (timeRemaining == 35)
        {
            moveTargetTimer.Start();
            addTargetTimer.Start();
            
        }
        if (movingTargetCount2 == 6)
        {
            addTargetTimer.Stop();
        }
        if (timeRemaining <= 0)
        {
            gameTimer.Stop();
            moveTargetTimer.Stop();
            addTargetTimer.Stop();
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
    private void MoveTargetTimer_Tick(object sender, EventArgs e)
{
    foreach (var movingTarget in movingTargets) // Iteruj przez wszystkie poruszające się cele
    {
        if (movingTarget != null) // Sprawdź, czy cel powinien się poruszać
        {
            // Oblicz nową pozycję
            int newX = movingTarget.Location.X + movingTarget.OffsetX;
            int newY = movingTarget.Location.Y + movingTarget.OffsetY;

            // Sprawdź kolizję z innymi celami
            bool collisionDetected = false;
            foreach (var otherTarget in targets)
            {
                if (otherTarget != movingTarget && 
                    new Rectangle(newX, newY, movingTarget.Width, movingTarget.Height)
                    .IntersectsWith(otherTarget.Bounds))
                {
                    collisionDetected = true; // Wykryto kolizję z innym celem
                    break; // Przerwij, jeśli wykryto kolizję
                }
            }

            // Sprawdź kolizję z etykietą
            Rectangle menuPanelBounds = new Rectangle(menuPanel.Location, menuPanel.Size);
            if (menuPanelBounds.IntersectsWith(new Rectangle(newX, newY, movingTarget.Width, movingTarget.Height)))
            {
                collisionDetected = true; // Ustaw flagę kolizji, jeśli cel koliduje z menuPanel
            }
                // Sprawdź kolizję z etykietą timeLabel
                Rectangle timeLabelBounds = new Rectangle(timeLabel.Location, timeLabel.Size);
                if (timeLabelBounds.IntersectsWith(new Rectangle(newX, newY, movingTarget.Width, movingTarget.Height)))
                {
                    collisionDetected = true; // Ustaw flagę kolizji, jeśli cel koliduje z timeLabel
                }

                // Sprawdź, czy cel osiągnął krawędź planszy
                if (newX < 0) // Lewa krawędź
            {
                SetRandomDirection(movingTarget); // Ustaw losowy kierunek
                newX = 0; // Ustaw cel na lewej krawędzi
            }
            else if (newX > this.ClientSize.Width - movingTarget.Width) // Prawa krawędź
            {
                SetRandomDirection(movingTarget); // Ustaw losowy kierunek
                newX = this.ClientSize.Width - movingTarget.Width; // Ustaw cel na prawej krawędzi
            }

            // Sprawdź, czy cel osiągnął górną lub dolną krawędź
            if (newY < 0) // Górna krawędź
            {
                SetRandomDirection(movingTarget); // Ustaw losowy kierunek
                newY = 0; // Ustaw cel na górnej krawędzi
            }
            else if (newY > this.ClientSize.Height - movingTarget.Height) // Dolna krawędź
            {
                SetRandomDirection(movingTarget); // Ustaw losowy kierunek
                newY = this.ClientSize.Height - movingTarget.Height; // Ustaw cel na dolnej krawędzi
            }

            // Ustaw nową pozycję, jeśli nie wykryto kolizji
            if (!collisionDetected)
            {
                movingTarget.Location = new Point(newX, newY); // Ustaw nową pozycję celu
            }
            else
            {
                // Zmień kierunek ruchu na nowy losowy kierunek
                SetRandomDirection(movingTarget);
            }    
                System.Diagnostics.Debug.WriteLine($"Target {movingTarget.Name} moved to ({newX}, {newY})");
        }
    }
        
    }
    private void SetRandomDirection(Target target)
    {
        Random random = new Random();
        target.OffsetX = random.Next(-20, 22); // Ustaw losowy offset X
        target.OffsetY = random.Next(-20, 22); // Ustaw losowy offset Y
    }
    private void AddTargetTimer_Tick(object sender, EventArgs e)
{
    // Wybierz losowy cel z istniejących celów
    if (targets.Count > 0)
    {
            if(movingTargetCount<=8)
            { 
            Target targetToMove = targets[random.Next(targets.Count)];

            // Upewnij się, że cel nie jest już w movingTargets
            if (!movingTargets.Contains(targetToMove))
                {
                    movingTargets.Add(targetToMove); // Dodaj cel do listy celów do poruszania
                    SetRandomDirection(targetToMove); // Ustaw losowy kierunek dla celu
                    movingTargetCount++;
                    movingTargetCount2++;
                }
            }
       }
}
private void AddTargetTimer(Target targetToMove)
{
        if (targets.Count > 0)
        {
            if (movingTargetCount <= 8)
            {
                // Upewnij się, że cel nie jest już w movingTargets
                if (!movingTargets.Contains(targetToMove))
                {
                    movingTargets.Add(targetToMove); // Dodaj cel do listy celów do poruszania
                    SetRandomDirection(targetToMove); // Ustaw losowy kierunek dla celu
                    movingTargetCount++;
                }
            }
        }
    }
    private void PauseButton_Click(object sender, EventArgs e)
    {
        if (gameTimer.Enabled)
        {
            gameTimer.Stop();
            addTargetTimer.Stop();
            MessageBox.Show("Gra zapauzowana. Kliknij OK, aby kontynuować.", "Pauza");
            if (isMoving)
            {
                moveTargetTimer.Stop();               
                isMoving = false;
            }
            else
            {
                moveTargetTimer.Start();                
                isMoving = true;
            }
            gameTimer.Start();
            addTargetTimer.Start();
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
                moveTargetTimer.Stop();
                addTargetTimer.Stop();
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
