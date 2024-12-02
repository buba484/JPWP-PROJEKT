using System;
using System.Drawing;
using System.Windows.Forms;

public class Target : Button
{
    public bool IsCorrect { get; set; } // Właściwość do oznaczania, czy cel jest poprawny

    // Konstruktor
    public Target()
    {
       
        this.FlatStyle = FlatStyle.Flat; // Ustaw styl przycisku na płaski, aby lepiej pasował do tła
        this.BackColor = Color.Transparent; // Ustaw tło na przezroczyste
        this.FlatAppearance.BorderSize = 0; // Usuniecie obramowania                                            // Ustawienie kolorów tła na przezroczyste dla stanów myszki
        this.FlatAppearance.MouseOverBackColor = Color.Transparent; // Kolor tła przy najechaniu
        this.FlatAppearance.MouseDownBackColor = Color.Transparent; // Kolor tła podczas kliknięcia

    }

    // Nadpisanie metody OnPaintBackground, aby nie rysować tła
    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
        // Nie rysuj tła
    }

    
    protected override void OnPaint(PaintEventArgs pevent)
    {
        base.OnPaint(pevent);
        
    }
    protected override void OnMouseEnter(EventArgs e)
    {
        
    }

    // Nadpisanie metody OnMouseLeave, aby nie zmieniać tła przy opuszczeniu
    protected override void OnMouseLeave(EventArgs e)
    {
        
    }
}

