using System;
using System.Windows.Forms;

public static class Program
{
    [STAThread]
    public static void Main()
    {
        
        Application.EnableVisualStyles(); // Umo�liwia stosowanie styl�w wizualnych Windows
        Application.SetCompatibleTextRenderingDefault(false); // Ustawienia renderowania tekstu
        Application.Run(new MainForm()); // Uruchamia g��wny formularz aplikacji
    }
}