using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

public class Kalambury : Form
{
    private MainForm mainForm;
    private Button[,] buttons;        // Pole 3x3
    private Label wordLabel;       // Wyświetlane hasło
    private Label wordLabel2;
    private Label wordLabel3;    
    private Button newGameButton;     // Przycisk nowej gry
    private Label scoreLabel;
    private Timer gameTimer;          // Timer
    private Label timerLabel;         // Wyświetlanie czasu
    private int timeRemaining;        // Pozostały czas
    private int score;
    public int czas;

    public Kalambury(MainForm mainForm)
    {
        this.mainForm = mainForm;
        buttons = new Button[3, 3];
        this.czas = 0;
        // Konfiguracja okna
        this.Text = "Kalambury 3x3";
        this.ClientSize = new Size(400, 500);
        this.BackColor = Color.LightGray;
        this.score = 0;
        // Inicjalizacja komponentów
        InitializeGame();
    }

    private void InitializeGame()
    {
        // Wyświetlanie hasła
        wordLabel = new Label
        {
            Text = "Kliknij 'Nowa Gra', aby rozpocząć",
            Font = new Font("Arial", 14, FontStyle.Bold),
            Dock = DockStyle.Top,
            TextAlign = ContentAlignment.MiddleCenter,
            Height = 50
        };
        
        //instrukcja
        wordLabel2 = new Label
        {
            Text = "Zobaczysz 9 zwierząt, twoim zadaniem jest znaleźć 4 pieski ",
            Font = new Font("Arial", 14, FontStyle.Bold),
            Dock = DockStyle.Top,
            TextAlign = ContentAlignment.MiddleCenter,
            Height = 50
        };
        
        wordLabel3 = new Label
        {
            Text = "Bedziesz miał 3 sekundy na zapamiętanie ich położenia. POWODZENIA!",
            Font = new Font("Arial", 14, FontStyle.Bold),
            Dock = DockStyle.Top,
            TextAlign = ContentAlignment.MiddleCenter,
            Height = 50
        };
        this.Controls.Add(wordLabel3);
        this.Controls.Add(wordLabel2);
        this.Controls.Add(wordLabel);
        // Przycisk nowej gry
        newGameButton = new Button
        {
            Text = "Nowa Gra",
            Dock = DockStyle.Bottom,
            Height = 50,
            BackColor = Color.LightGreen
        };
        newGameButton.Click += NewGameButton_Click;
        this.Controls.Add(newGameButton);

        // Timer i wyświetlanie czasu
        timerLabel = new Label
        {
            Text = "Pozostały czas: 0",
            Font = new Font("Arial", 12),
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Bottom,
            Height = 30
        };
        
        scoreLabel = new Label
        {
            Text = "Ilość znalezionych piesków: 0",
            Font = new Font("Arial", 12),
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Bottom,
            Height = 30
        };
        this.Controls.Add(scoreLabel);
        this.Controls.Add(timerLabel);
        // Inicjalizacja timera
        gameTimer = new Timer
        {
            Interval = 1000 // 1 sekunda
        };
        gameTimer.Tick += GameTimer_Tick;
    }
    private void GenerateGrid(string file,int i, int j)
    {
                
                buttons[i,j] = new Button
                {
                    Width = 100,
                    Height = 100,
                    BackgroundImage = Image.FromFile(file), // Ustawienie tła z pliku
                    BackgroundImageLayout = ImageLayout.Stretch, // Ustawienie sposobu wyświetlania obrazu
                    Left = 50+(j * 100),
                    Top = 50+(i * 110),

                };
                
                buttons[i,j].Click += Button_Click; // Obsługa kliknięcia
                buttons[i,j].Tag = GetFileNameWithoutExtension(file);
                this.Controls.Add(buttons[i,j]);

    }
    private void NewGameButton_Click(object sender, EventArgs e)
    {
        StartNewGameWait();
    }
    private async void StartNewGameWait()
    {
        this.wordLabel.Hide();
        this.wordLabel2.Hide();
        this.wordLabel3.Hide();
        GenerateGrid("dog.png", 0, 0);
        GenerateGrid("cat.png", 0, 1);
        GenerateGrid("dog.png", 0, 2);
        GenerateGrid("croc.png", 1, 0);
        GenerateGrid("dog.png", 1, 1);
        GenerateGrid("rhino.png", 1, 2);
        GenerateGrid("dog.png", 2, 0);
        GenerateGrid("cat.png", 2, 1);
        GenerateGrid("rhino.png", 2, 2);
        // Losowanie hasła        



        wordLabel.Text = $"Hasło: Pies";


        // Ustawienie czasu gry
        timeRemaining = 30; // 30 sekund na odgadnięcie
        timerLabel.Text = $"Pozostały czas: {timeRemaining}";
        await Task.Delay(3000);
        StartNewGame();
    }
    private void StartNewGame()
    {
        for(int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                buttons[i, j].BackgroundImage = Image.FromFile("szary.png"); // Ustawienie tła na szary
            }
        }


                wordLabel.Text = $"Hasło: Pies";

     
        // Ustawienie czasu gry
        timeRemaining = 10; // 30 sekund na odgadnięcie
        timerLabel.Text = $"Pozostały czas: {timeRemaining}";
        gameTimer.Start();
    }

    private void GameTimer_Tick(object sender, EventArgs e)
    {
        timeRemaining--;
        timerLabel.Text = $"Pozostały czas: {timeRemaining}";

        if (timeRemaining <= 0)
        {
            gameTimer.Stop();
            MessageBox.Show("Czas się skończył! Spróbuj ponownie.", "Koniec gry");
        }
    }

    private void Button_Click(object sender, EventArgs e)
    {
        Button clickedButton = sender as Button;
        if (clickedButton.Tag.ToString() == "dog")
        {
            clickedButton.BackgroundImage = Image.FromFile("dog.png");
            clickedButton.BackgroundImageLayout = ImageLayout.Stretch;
            score++;
            scoreLabel.Text = $"Ilość znalezionych piesków: {score}";
        }
        Checkscore(score);
    }
    private void Checkscore(int score)
    {
        if(score==4)
        {
            gameTimer.Stop();
            var result = MessageBox.Show("Gratulacje udało ci się zapamietąć oraz odnaleść pieski. W nagrode masz 15 dodatkowych sekund", "Wynik", MessageBoxButtons.OK);
            if(result == DialogResult.OK)
            {
                this.czas = 15;
                this.Close();
            }
            
        }
    }


    public static string GetFileNameWithoutExtension(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            return string.Empty; // Zwróć pusty string, jeśli argument jest pusty lub null
        }

        // Znajdź ostatnią kropkę w nazwie pliku
        int dotIndex = fileName.LastIndexOf('.');
        if (dotIndex == -1)
        {
            return fileName; // Jeśli nie ma kropki, zwróć całą nazwę pliku
        }

        // Zwróć część przed kropką
        return fileName.Substring(0, dotIndex);
    }
    private async Task WaitForThreeSeconds()
    {
        // Oczekiwanie 3 sekundy
        await Task.Delay(3000); // 3000 ms = 3 sekundy
    }
    protected void Kalambury_FormClosing(object sender, FormClosingEventArgs e)
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